using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FireBullet : MonoBehaviour
{
    public enum WeaponType { RIFLE, SHOTGUN };
    WeaponType curWeapon = WeaponType.RIFLE;
    public Transform firePos; //��Ʈ��ũ ���ӿ����� �±׳� �̸����� ã�Ƽ� �� �ȴ�.
    [SerializeField] PlayerInput playerInput;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] ParticleSystem CartrigeEffect;
    [SerializeField] AudioSource source;
    [SerializeField] Animator animator;
    [SerializeField] PlayerDamage damage;
    [Header("magazineUI")]
    [SerializeField] Image magazineImage;
    [SerializeField] Text magazineText;

    public GunData[] gunDatas;
    [Header("���� ��ü UI")]
    public Sprite[] weaponIcons; //������ ���� �̹���
    public Image weaponImage; //��ü�� ���� �̹��� UI
    public MeshRenderer[] weaponMeshes = new MeshRenderer[2] ;
    
    private readonly int hashReload = Animator.StringToHash("Reload");
    private int curBullet = 10; //���� �Ѿ� ��
    private readonly int maxBullet = 10; //źâ�� ����ִ� �ִ� �Ѿ� ��
    private WaitForSeconds reloadWs;
    private bool isReloading = false;
    
    void Start()
    {
        weaponMeshes = gameObject.transform.GetComponentsInChildren<MeshRenderer>();
        magazineImage = GameObject.Find("Panel-Magazine").transform.GetChild(2).GetComponent<Image>();
        magazineText = GameObject.Find("Panel-Magazine").transform.GetChild(0).GetComponent<Text>();
        damage = GetComponent<PlayerDamage>();
        source = GetComponent<AudioSource>();
        firePos = transform.GetChild(0).GetChild(0).GetChild(0).transform;
        
        playerInput = GetComponent<PlayerInput>();
        muzzleFlash = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<ParticleSystem>(); //find�� ã���� ���߿� ��Ʈ��ũ ���� �� �ٸ� �÷��̾��� FIrePos�� ���� ����� ���� ���� ���ɼ��� ����. -> �ڽ����� ã����� �Ѵ�.
        muzzleFlash = GetComponentsInChildren<ParticleSystem>()[0]; //���� ������Ʈ�� ������ �ִ� ParticleSystem���� �迭�� ����� ���� 0��° �ε����� muzzleFlash�� �����Ѵ�.
        CartrigeEffect = GetComponentsInChildren<ParticleSystem>()[1];
        animator = GetComponent<Animator>();
        reloadWs = new WaitForSeconds(gunDatas[(int)curWeapon].reloadTime); //scriptable data���� �� static data�̴�.
        DisplayMagazine();
    }

    void Update()
    {
        if (damage.isPlayerDie) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (playerInput.fire == true && !isReloading && !playerInput.sprint)
        {
            Fire();
        }
        
    }

    private void DisplayMagazine()
    {
        magazineText.text = $"<color=#ff0000>{curBullet}</color> / {maxBullet}";
        magazineImage.fillAmount = (float)curBullet / maxBullet;
    }

    void Fire()
    {
        muzzleFlash.Play();
        CartrigeEffect.Play();
        //Instantiate(bulletPrefab, firePos.position, firePos.rotation);
        var _bullet = PoolingManager.p_instance.GetBullet(); //��Ȱ��ȭ ���� �Ѿ� ��ȯ
        source.PlayOneShot(gunDatas[(int)curWeapon].shotClip);
        
        isReloading = (--curBullet % maxBullet == 0);
        _bullet.transform.position = firePos.position;
        _bullet.transform.rotation = firePos.rotation;
        _bullet.gameObject.SetActive(true);
        if (isReloading) StartCoroutine(Reloading());
        DisplayMagazine();
    }

    IEnumerator Reloading()
    {
        animator.SetTrigger(hashReload);
        source.PlayOneShot(gunDatas[(int)curWeapon].reloadClip, 1.0f);

        yield return reloadWs;
        curBullet = maxBullet;
        isReloading = false;
        DisplayMagazine();
    }

    public void OnChangeWeapon()
    {
        curWeapon = (WeaponType)((int)++curWeapon % 2);
        weaponImage.sprite = weaponIcons[(int)curWeapon];
        reloadWs = new WaitForSeconds(gunDatas[(int)curWeapon].reloadTime);
        if (curWeapon == 0)
        {
            weaponMeshes[0].enabled = true;
            weaponMeshes[1].enabled = false;
        }
        else
        {
            weaponMeshes[0].enabled = false;
            weaponMeshes[1].enabled = true;
        }
    }
}
