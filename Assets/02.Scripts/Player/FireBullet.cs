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
    [SerializeField] Animator animator;
    public GunData gunData_r;
    public GunData gunData_s;

    private readonly string strBulletCtrl = "BulletCtrl";
    private readonly int hashReload = Animator.StringToHash("Reload");
    [SerializeField] private int curBullet = 10; //���� �Ѿ� ��
    private readonly int maxBullet = 10; //źâ�� ����ִ� �ִ� �Ѿ� ��
    private WaitForSeconds reloadWs;
    private bool isReloading = false;
    
    void Start()
    {
        source = GetComponent<AudioSource>();
        firePos = transform.GetChild(0).GetChild(0).GetChild(0).transform;
        bulletPrefab = Resources.Load<GameObject>(strBulletCtrl);
        playerInput = GetComponent<PlayerInput>();
        muzzleFlash = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<ParticleSystem>(); //find�� ã���� ���߿� ��Ʈ��ũ ���� �� �ٸ� �÷��̾��� FIrePos�� ���� ����� ���� ���� ���ɼ��� ����. -> �ڽ����� ã����� �Ѵ�.
        muzzleFlash = GetComponentsInChildren<ParticleSystem>()[0]; //���� ������Ʈ�� ������ �ִ� ParticleSystem���� �迭�� ����� ���� 0��° �ε����� muzzleFlash�� �����Ѵ�.
        CartrigeEffect = GetComponentsInChildren<ParticleSystem>()[1];
        animator = GetComponent<Animator>();
        reloadWs = new WaitForSeconds(gunData_r.reloadTime); //scriptable data���� �� static data�̴�.
    }

    void Update()
    {
        if (playerInput.fire == true && !isReloading && !playerInput.sprint)
        {
            Fire();
        }
    }

    void Fire()
    {
        muzzleFlash.Play();
        CartrigeEffect.Play();
        Instantiate(bulletPrefab, firePos.position, firePos.rotation);
        source.PlayOneShot(gunData_r.shotClip);
        isReloading = (--curBullet % maxBullet == 0);
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
