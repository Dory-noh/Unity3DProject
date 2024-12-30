using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] private Transform firePos; //��Ʈ��ũ ���ӿ����� �±׳� �̸����� ã�Ƽ� �� �ȴ�.
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
        muzzleFlash = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<ParticleSystem>(); //find�� ã���� ���߿� ��Ʈ��ũ ���ӽ� �ٸ� �÷��̾��� FIrePos�� ���� ����� ���� ���� ���ɼ��� ����. -> �ڽ����� ã����� �Ѵ�.
        muzzleFlash = GetComponentsInChildren<ParticleSystem>()[0]; //���� ������Ʈ�� ������ �ִ� ParticleSystem���� �迭�� ����� �� �� 0��° �ε����� muzzleFlash�� �����Ѵ�.
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
