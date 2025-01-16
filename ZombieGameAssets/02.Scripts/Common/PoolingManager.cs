using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager P_Instance = null; //매니저니까 싱글톤 만듦
    public GameObject bullet_prefab;
    
    int maxBullet = 20;
    List<GameObject> bulletList = new List<GameObject>();

    [SerializeField] private Canvas uiCanvas;
    [SerializeField] GameObject hpBarPrefab;
    [SerializeField] private List<GameObject> hpBarList;

    private void Awake()
    {
        if(P_Instance == null)
        {
            P_Instance = this; //this는 GetComponent<PoolingManager>()와 같음
        }
        else if(P_Instance != this)
        {
            Destroy(gameObject);
        }
        StartCoroutine(BulletPooling());
    }
    void Start()
    {
        uiCanvas = GameObject.Find("UI - Canvas").GetComponent<Canvas>();
        hpBarPrefab = Resources.Load<GameObject>("HpBar");
        StartCoroutine(HpBarPooling());
    }

    void Update()
    {
        
    }

    IEnumerator HpBarPooling()
    {
        yield return null;
        
        
        if (hpBarPrefab != null)
        {
            for(int i = 0; i < 20; i++)
            {
                var hpBar = Instantiate(hpBarPrefab, uiCanvas.transform);
                hpBar.name = $"{i + 1}번째 hpBar";
                hpBar.SetActive(false);
                hpBarList.Add(hpBar);
            }
        }
    }
    public GameObject GetHpBar()
    {
        foreach(var hpBar in hpBarList)
        {
            if(hpBar.activeSelf == false)
            {
                return hpBar;
            }
        }
        return null;
    }

    IEnumerator BulletPooling()
    {
        yield return null;
        GameObject bulletPoolingObj = new GameObject("bulletPoolingObj");
        for(int i = 0; i < maxBullet; i++)
        {
            var bullet = Instantiate(bullet_prefab,bulletPoolingObj.transform);
            bullet.name = $"{i+1}: 개";
            bullet.gameObject.SetActive(false); //꺼둔 채로 생성
            bulletList.Add(bullet);//리스트에 담기
        }
    }
    public GameObject GetBullet() //returntype 은 GameObject
    {
        for(int i = 0; i < bulletList.Count; i++)
        {
            if(bulletList[i].activeSelf == false) //꺼져있는 것만 반환한다.켜져있으면 null 리턴
            {
                return bulletList[i];
            }
        }
        return null;
    }
}
