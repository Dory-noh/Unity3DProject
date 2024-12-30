using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] private Transform firePos; //네트워크 게임에서는 태그나 이름으로 찾아선 안 된다.
    [SerializeField] PlayerInput playerInput;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] ParticleSystem CartrigeEffect;
    [SerializeField] AudioSource source;
    public GunData gunData_r;
    public GunData gunData_s;

    readonly string strBulletCtrl = "BulletCtrl";
    void Start()
    {
        source = GetComponent<AudioSource>();
        firePos = transform.GetChild(0).GetChild(0).GetChild(0).transform;
        bulletPrefab = Resources.Load<GameObject>(strBulletCtrl);
        playerInput = GetComponent<PlayerInput>();
        muzzleFlash = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<ParticleSystem>(); //find로 찾으면 나중에 네트워크 게임시 다른 플레이어의 FIrePos와 겹쳐 제대로 잡지 못할 가능성이 높다. -> 자식으로 찾아줘야 한다.
        muzzleFlash = GetComponentsInChildren<ParticleSystem>()[0]; //현재 오브젝트가 가지고 있는 ParticleSystem들을 배열로 만들고 그 중 0번째 인덱스를 muzzleFlash와 연결한다.
        CartrigeEffect = GetComponentsInChildren<ParticleSystem>()[1];
    }

    void Update()
    {
        if (playerInput.fire == true)
        {
            muzzleFlash.Play();
            CartrigeEffect.Play();
            Fire();
        }
    }

    void Fire()
    {
        Instantiate(bulletPrefab, firePos.position, firePos.rotation);
        source.PlayOneShot(gunData_r.shotClip);
    }
}
