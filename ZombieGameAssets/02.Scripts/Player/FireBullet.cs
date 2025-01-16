using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class FireBullet : MonoBehaviour
{
    public static int bulletCount = 0;
    public static int bulletMax = 10;
    public GameObject bulletPrefab; //�Ѿ�
    public Transform firePos; //�߻���ġ 
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
    {   //�ʱ�ȭ ����
        handAni = GetComponent<HandAnimation>();
        timePrev = Time.time; //���� �ð� 
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
        //0 ,1 ������ 2. ��
        if (Input.GetMouseButton(0) && weaponChange.isHaveM4A1) //���콺 ���ʹ�ư�� �����ٸ�  //����
        {
            if (handAni.isRunning == false
                && (Time.time - timePrev) >= 0.1f)// ������ �Ѿ� �� ����
                    // ����� - ���Ž�  = �귯�� �ð�
            {
                if (handAni.isReload == false)
                {
                    Fire();
                    muzzleFlash.Play();
                    Cartrige.Play();

                    timePrev = Time.time;

                    //Invoke() �޼��� �Լ� 
                    Invoke("FireEffectDisable", 0.1f);
                }
               //���� �ʿ��� �Լ��� ���ϴ� �ð��� ȣ�� �ϴ� �Լ�
            }         

            
                 
        }
        else if (Input.GetMouseButtonDown(0) && !weaponChange.isHaveM4A1) //���콺 ���ʹ�ư�� �����ٸ�  //�ܹ�
        {
            if (handAni.isRunning == false)
            // ����� - ���Ž�  = �귯�� �ð�
            {
                Fire();
                muzzleFlash.Play();
                Cartrige.Play();
                //Invoke() �޼��� �Լ� 
                Invoke("FireEffectDisable", 0.1f);
                //���� �ʿ��� �Լ��� ���ϴ� �ð��� ȣ�� �ϴ� �Լ�
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
        //�����Ҵ� 
        var _bullet = PoolingManager.P_Instance.GetBullet(); //���� �ִ� �͸� ��ȯ
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
