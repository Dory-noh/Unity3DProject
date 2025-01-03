using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    [SerializeField] AudioClip fireClip;
    [SerializeField] AudioClip reloadClip;
    [SerializeField] AudioSource source;
    [SerializeField] Animator animator;
    [SerializeField] GameObject fireEffect;
    [SerializeField] Transform tr;
    [SerializeField] Transform playerTr;
    //[SerializeField] GameObject E_Bullet;
    [SerializeField] EnemyAI enemyAI;
    [SerializeField] MeshRenderer muzzleFlash;
    public Transform E_FirePos;

    private readonly string playerTag = "Player";
    private readonly int hashReload = Animator.StringToHash("Reload");
    private readonly int hashOffset = Animator.StringToHash("Offset");
    public E_GunData gunData_Ak;

    private int curBullet = 10;
    private int maxBullet = 10;
    bool isReloading = false;
    private float nextFire = 0f; //다음에 발사할 시간 계산용 변수
    private readonly float damping = 10; //플레이어를 향해 회전할 속도
    public bool isFire = false;
    WaitForSeconds reloadWs;
    WaitForSeconds muzzleWs;
    

    IEnumerator Start()
    {
        enemyAI = GetComponent<EnemyAI>();
        animator = GetComponent<Animator>();
        fireClip = gunData_Ak.shotClip;
        reloadClip = gunData_Ak.reloadClip;
        fireEffect = Resources.Load<GameObject>("Effect/FlareMobile");
        source = GetComponent<AudioSource>();
        yield return new WaitForSeconds(0.5f);
        tr = GetComponent<Transform>();
        playerTr = GameObject.FindGameObjectWithTag(playerTag).transform;
        //E_Bullet = Resources.Load<GameObject>("Prefab/E_Bullet");
        E_FirePos = transform.GetChild(3).GetChild(0).GetChild(0).transform;
        muzzleFlash = E_FirePos.GetComponentInChildren<MeshRenderer>();
        muzzleFlash.enabled = false;
        reloadWs = new WaitForSeconds(gunData_Ak.reloadTime);
    }


    void Update()
    {
        if (isFire && !enemyAI.isDie)
        {
            if(Time.time >= nextFire && !isReloading)
            {
                Fire();
                nextFire = Time.time + gunData_Ak.fireRate + Random.Range(0.0f, 0.3f);
            } 
            Quaternion rot = Quaternion.LookRotation(playerTr.position - transform.position); //타겟-자기 자신 위치 => 방향
            tr.rotation = Quaternion.Slerp(tr.rotation, rot, damping*Time.deltaTime);
        }
    }
    void Fire()
    {
        //CartrigeEffect.Play();
        //Instantiate(E_Bullet,E_FirePos.position,E_FirePos.rotation);
        var bullet = PoolingManager.p_instance.GetEnemyBullet();
        bullet.transform.position = E_FirePos.position;
        bullet.transform.rotation = E_FirePos.rotation;
        bullet.SetActive(true);
        source.PlayOneShot(fireClip, 1.0f);
        isReloading = (--curBullet % maxBullet == 0);
        muzzleWs = new WaitForSeconds(Random.Range(0.05f, 0.2f));
        StartCoroutine(ShowMuzzleFlash());
        if (isReloading) StartCoroutine(Reloading());
    }
    IEnumerator ShowMuzzleFlash()
    {
        muzzleFlash.enabled=true;
        Quaternion rot = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        muzzleFlash.transform.localRotation = rot; //부모, World 좌표와는 독립적으로 회전을 할 것이기 때문이다.
        float _scale = Random.Range(1f, 2f);
        muzzleFlash.transform.localScale = Vector3.one * _scale;
        yield return muzzleWs;
        muzzleFlash.enabled=false;
    }

    IEnumerator Reloading()
    {
        animator.SetTrigger(hashReload);
        source.PlayOneShot(gunData_Ak.reloadClip, 1.0f);

        yield return reloadWs;

        curBullet = maxBullet;
        isReloading = false;
    }
}
