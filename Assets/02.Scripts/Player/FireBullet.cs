using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour
{
    public Transform firePos; //네트워크 게임에서는 태그나 이름으로 찾아선 안 된다.
    [SerializeField] PlayerInput playerInput;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] ParticleSystem CartrigeEffect;
    [SerializeField] AudioSource source;
    [SerializeField] Animator animator;
    [SerializeField] PlayerDamage damage;
    public GunData gunData_r;
    public GunData gunData_s;

    
    private readonly int hashReload = Animator.StringToHash("Reload");
    [SerializeField] private int curBullet = 10; //현재 총알 수
    private readonly int maxBullet = 10; //탄창에 들어있는 최대 총알 수
    private WaitForSeconds reloadWs;
    private bool isReloading = false;
    
    void Start()
    {
        damage = GetComponent<PlayerDamage>();
        source = GetComponent<AudioSource>();
        firePos = transform.GetChild(0).GetChild(0).GetChild(0).transform;
        
        playerInput = GetComponent<PlayerInput>();
        muzzleFlash = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<ParticleSystem>(); //find로 찾으면 나중에 네트워크 게임 시 다른 플레이어의 FIrePos와 겹쳐 제대로 잡지 못할 가능성이 높다. -> 자식으로 찾아줘야 한다.
        muzzleFlash = GetComponentsInChildren<ParticleSystem>()[0]; //현재 오브젝트가 가지고 있는 ParticleSystem들을 배열로 만들고 그중 0번째 인덱스를 muzzleFlash와 연결한다.
        CartrigeEffect = GetComponentsInChildren<ParticleSystem>()[1];
        animator = GetComponent<Animator>();
        reloadWs = new WaitForSeconds(gunData_r.reloadTime); //scriptable data들은 다 static data이다.
    }

    void Update()
    {
        if (damage.isPlayerDie) return;
        if (playerInput.fire == true && !isReloading && !playerInput.sprint)
        {
            Fire();
        }
    }

    void Fire()
    {
        muzzleFlash.Play();
        CartrigeEffect.Play();
        //Instantiate(bulletPrefab, firePos.position, firePos.rotation);
        var _bullet = PoolingManager.p_instance.GetBullet(); //비활성화 상태 총알 반환
        source.PlayOneShot(gunData_r.shotClip);
        isReloading = (--curBullet % maxBullet == 0);
        _bullet.transform.position = firePos.position;
        _bullet.transform.rotation = firePos.rotation;
        _bullet.gameObject.SetActive(true);
        if (isReloading) StartCoroutine(Reloading());

    }

    IEnumerator Reloading()
    {
        animator.SetTrigger(hashReload);
        source.PlayOneShot(gunData_r.reloadClip, 1.0f);

        yield return reloadWs;

        curBullet = maxBullet;
        isReloading = false;
    }
}
