using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Fire : MonoBehaviour
{
    [SerializeField] Transform playerTr;
    [SerializeField] Transform tr;
    [SerializeField] Animator animator;
    [SerializeField] E2_GunData E2_GunData;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip fireClip;
    [SerializeField] AudioClip reloadClip;
    [SerializeField] Transform firePos;
    [SerializeField] Enemy2AI enemy2AI;
    //[SerializeField] GameObject bulletPrefab;
    [SerializeField] MeshRenderer muzzleFlash;

    public bool isFire = false;
    bool isReload = false;
    private float nextFire = 0;
    private readonly float damping = 10f;
    private int maxBullet = 10;
    private int curBullet = 0;
    WaitForSeconds muzzleWs;
    private WaitForSeconds reloadWs;
    private readonly string playerTag = "Player";
    private readonly int hashFire = Animator.StringToHash("Fire");
    private readonly int hashReload = Animator.StringToHash("Reload");
    void Start()
    {
        enemy2AI = GetComponent<Enemy2AI>();
        
        
        //bulletPrefab = Resources.Load<GameObject>("Prefab/E2_Bullet");
        
        reloadClip = E2_GunData.reloadClip;
        fireClip = E2_GunData.shotClip;
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        
        
        
        
    }
    private void OnEnable()
    {
        playerTr = GameObject.FindGameObjectWithTag(playerTag).transform;
        tr = transform;
        curBullet = maxBullet;
        reloadWs = new WaitForSeconds(E2_GunData.reloadTime);
        firePos = transform.GetChild(1).GetChild(0).GetChild(0).transform;
        muzzleFlash = firePos.GetChild(0).GetComponent<MeshRenderer>();
        muzzleFlash.enabled = false;
    }

    void Update()
    {
        if (isFire&&!enemy2AI.isDie)
        {
            if(Time.time >= nextFire && !isReload)
            {
                Fire();
                nextFire = Time.time + E2_GunData.fireRate + Random.Range(0f,0.3f);
            }
            Quaternion rot = Quaternion.LookRotation(playerTr.position - tr.position);
            tr.rotation = Quaternion.Slerp(tr.rotation, rot, damping * Time.deltaTime);
        }
        
    }

    public void Fire()
    {
        //Instantiate(bulletPrefab, firePos.position, firePos.rotation);
        var bullet = PoolingManager.p_instance.GetEnemy2Bullet();
        bullet.transform.position = firePos.position;
        bullet.transform.rotation = firePos.rotation;
        bullet.gameObject.SetActive(true);
        source.PlayOneShot(fireClip, 1f);
        isReload = (--curBullet % maxBullet) == 0;
        animator.SetTrigger(hashFire);
        //소리와 이펙트 효과
        muzzleWs = new WaitForSeconds(Random.Range(0.05f, 0.2f));
        StartCoroutine(ShowMuzzleFalsh());
        if (isReload) StartCoroutine(ReloadBullet());
    }


    IEnumerator ShowMuzzleFalsh()
    {
        muzzleFlash.enabled = true;
        Quaternion rot = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        muzzleFlash.transform.localRotation = rot;
        float _scale = Random.Range(1f, 2f);
        muzzleFlash.transform.localScale = Vector3.one * _scale;
        yield return muzzleWs;
        muzzleFlash.enabled = false;
    }

    IEnumerator ReloadBullet()
    {
        animator.SetTrigger(hashReload);
        source.PlayOneShot(reloadClip, 1f);
        yield return reloadWs;
        curBullet = maxBullet;
        isReload = false;

    }

}
