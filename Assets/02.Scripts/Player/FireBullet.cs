using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FireBullet : MonoBehaviour
{
    public enum WeaponType { RIFLE, SHOTGUN };
    WeaponType curWeapon = WeaponType.RIFLE;
    public Transform firePos; //네트워크 게임에서는 태그나 이름으로 찾아선 안 된다.
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
    [Header("무기 교체 UI")]
    public Sprite[] weaponIcons; //변결할 무기 이미지
    public Image weaponImage; //교체할 무기 이미지 UI
    public MeshRenderer[] weaponMeshes = new MeshRenderer[2] ;
    
    private readonly int hashReload = Animator.StringToHash("Reload");
    private int curBullet = 10; //현재 총알 수
    private readonly int maxBullet = 10; //탄창에 들어있는 최대 총알 수
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
        muzzleFlash = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<ParticleSystem>(); //find로 찾으면 나중에 네트워크 게임 시 다른 플레이어의 FIrePos와 겹쳐 제대로 잡지 못할 가능성이 높다. -> 자식으로 찾아줘야 한다.
        muzzleFlash = GetComponentsInChildren<ParticleSystem>()[0]; //현재 오브젝트가 가지고 있는 ParticleSystem들을 배열로 만들고 그중 0번째 인덱스를 muzzleFlash와 연결한다.
        CartrigeEffect = GetComponentsInChildren<ParticleSystem>()[1];
        animator = GetComponent<Animator>();
        reloadWs = new WaitForSeconds(gunDatas[(int)curWeapon].reloadTime); //scriptable data들은 다 static data이다.
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
        var _bullet = PoolingManager.p_instance.GetBullet(); //비활성화 상태 총알 반환
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
