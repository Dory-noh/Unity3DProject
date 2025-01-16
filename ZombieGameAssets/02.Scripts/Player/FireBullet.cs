using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class FireBullet : MonoBehaviour
{
    public static int bulletCount = 0;
    public static int bulletMax = 10;
    public GameObject bulletPrefab; //총알
    public Transform firePos; //발사위치 
    public AudioSource source;
    public AudioClip gunClip;
    public HandAnimation handAni;
    public Animation _animation;
    public ParticleSystem muzzleFlash;
    public ParticleSystem Cartrige;
    public WeaponChange weaponChange;
    public float damage;
    Text bulletCoutText;
    Image bulletCountImage;

    private float timePrev;
    private void OnEnable()
    {
        GameManager.OnItemChange += UpdateSetUp;
    }
    void Start()
    {   //초기화 대입
        handAni = GetComponent<HandAnimation>();
        timePrev = Time.time; //현재 시간 
        muzzleFlash.Stop();
        Cartrige.Stop();
        _animation = GetComponent<Animation>();
        weaponChange = GetComponent<WeaponChange>();
        bulletCoutText = GameObject.Find("Canvas-UI").transform.GetChild(6).GetChild(0).GetComponent<Text>();
        bulletCountImage = GameObject.Find("Canvas-UI").transform.GetChild(6).GetChild(2).GetComponent<Image>();
        
        ShowBulletCount();
    }
    void UpdateSetUp()
    {
        damage = GameManager.Instance.gameData.damage;
    }
    public void ShowBulletCount()
    {
        bulletCoutText.text = $"{bulletMax - bulletCount} / {bulletMax}";
        bulletCountImage.fillAmount = (float)(bulletMax - bulletCount) / bulletMax;
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        //0 ,1 오른쪽 2. 휠
        if (Input.GetMouseButton(0) && weaponChange.isHaveM4A1) //마우스 왼쪽버튼을 눌렀다면  //연발
        {
            if (handAni.isRunning == false
                && (Time.time - timePrev) >= 0.1f)// 나오는 총알 수 조절
                    // 현재시 - 과거시  = 흘러간 시간
            {
                if (handAni.isReload == false)
                {
                    Fire();
                    muzzleFlash.Play();
                    Cartrige.Play();

                    timePrev = Time.time;

                    //Invoke() 메서드 함수 
                    Invoke("FireEffectDisable", 0.1f);
                }
               //내가 필요한 함수를 원하는 시간에 호출 하는 함수
            }         

            
                 
        }
        else if (Input.GetMouseButtonDown(0) && !weaponChange.isHaveM4A1) //마우스 왼쪽버튼을 눌렀다면  //단발
        {
            if (handAni.isRunning == false)
            // 현재시 - 과거시  = 흘러간 시간
            {
                Fire();
                muzzleFlash.Play();
                Cartrige.Play();
                //Invoke() 메서드 함수 
                Invoke("FireEffectDisable", 0.1f);
                //내가 필요한 함수를 원하는 시간에 호출 하는 함수
            }



        }

    }
    void FireEffectDisable()
    {
        muzzleFlash.Stop();
        Cartrige.Stop();
    }
    private void Fire()
    {
        //what       //where         //how rotion
        //Instantiate(bulletPrefab, firePos.position, firePos.rotation);
        //동적할당 
        var _bullet = PoolingManager.P_Instance.GetBullet(); //꺼져 있는 것만 반환
        _bullet.transform.position = firePos.position;
        _bullet.transform.rotation = firePos.rotation;
        _bullet.transform.gameObject.SetActive(true);

        source.PlayOneShot(gunClip, 1.0f);
        _animation.Play("fire");
        ++bulletCount;
        bulletCount = Mathf.Clamp(bulletCount, 0, 10);
        ShowBulletCount();
    }
}
