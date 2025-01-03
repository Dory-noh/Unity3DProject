using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager p_instance;



    //총알 오브젝트 풀링 - 플레이어
    [SerializeField] private GameObject playerBullet;
    [SerializeField] private List<GameObject> bulletList;

    //총알 오브젝트 풀링 - enemy1
    [SerializeField] private GameObject enemyBullet;
    [SerializeField] private List<GameObject> enemyBulletList;
    [SerializeField] private GameObject enemyBullet2;
    [SerializeField] private List<GameObject> enemyBullet2List;

    //적 오브젝트 풀링
    [SerializeField] private List<Transform> spwanPointsList;
    [SerializeField] private GameObject[] enemyPrefab = new GameObject[2];
    [SerializeField] private List<GameObject> enemyList;


    private readonly string strBulletCtrl = "Prefab/Bullet";
    private readonly string strEnemyBulletCtrl = "Prefab/E_Bullet";
    private readonly string strEnemyBullet2Ctrl = "Prefab/E2_Bullet";

    private void Awake()
    {
        //인스턴스 초기화
        if (p_instance == null)
            p_instance = this;
        else if (p_instance != this)
        {
            Destroy(gameObject);
        }
        //총알 연결
        playerBullet = Resources.Load<GameObject>(strBulletCtrl);
        enemyBullet = Resources.Load<GameObject>(strEnemyBulletCtrl);
        enemyBullet2 = Resources.Load<GameObject>(strEnemyBullet2Ctrl);

        //적 연결
        enemyPrefab[0] = Resources.Load<GameObject>("Prefab/Enemy");
        enemyPrefab[1] = Resources.Load<GameObject>("Prefab/Enemy2");
        //적 생성 위치 연결
        var spawnPos = GameObject.Find("SpawnPoints").transform;
        if(spawnPos != null)
        {
            spawnPos.GetComponentsInChildren<Transform>(spwanPointsList);
            spwanPointsList.RemoveAt(0);
        }
        //오브젝트 풀링
        CreatePooling();
        CreateEnemyPooling();
        CreateEnemyBulletPooling();
        CreateEnemyBullet2Pooling();
    }

    private void OnEnable()
    {
        StartCoroutine(EnemySpawn());
        //InvokeRepeating("EnemySpawn", 0.02f, 3f);//0.02초 처음 호출. 그 이후 3초 간격으로 호출한다.
    }
    //플레이어 총알 오브젝트 풀링 메서드
    private void CreatePooling()
    {
        GameObject playerBulletGroup = new GameObject("PlayerBulletGroup");
        for (int i = 0; i < 10; i++)
        {
            var bullet = Instantiate(playerBullet, playerBulletGroup.transform);
            bullet.name = $"{i+1} 발";
            bullet.SetActive(false);
            bulletList.Add(bullet);
        }
    }
    public GameObject GetBullet()
    {
        foreach(var bullet in bulletList)
        {
            if (bullet.activeSelf == false) //activeSelf: 오브젝트의 상태를 알려주는 프로퍼티
            {
                return bullet; //꺼진 것만 반환한다.
            }
        }
        return null;
    }
    //적1 총알 오브젝트 풀링 메서드
    private void CreateEnemyBulletPooling()
    {
        GameObject enemyBulletGroup = new GameObject("EnemyBulletGroup");
        for(int i = 0; i < 10; i++)
        {
            var bullet = Instantiate(enemyBullet, enemyBulletGroup.transform);
            bullet.name = $"enemy {i + 1}발";
            bullet.SetActive(false);
            enemyBulletList.Add(bullet);
        }
    }
    public GameObject GetEnemyBullet()
    {
        foreach (var bullet in enemyBulletList)
        {
            if (bullet.activeSelf == false)
            {
                return bullet;
            }
        }
        return null;
    }
    //적2 총알 오브젝트 풀링 메서드
    private void CreateEnemyBullet2Pooling()
    {
        GameObject enemyBullet2Group = new GameObject("EnemyBullet2Group");
        for(int i = 0; i < 10; i++)
        {
            var bullet = Instantiate(enemyBullet2, enemyBullet2Group.transform);
            bullet.name = $"enemy2 {i + 1}발";
            bullet.SetActive (false);
            enemyBullet2List.Add(bullet);
        }
    }
    public GameObject GetEnemy2Bullet()
    {
        foreach(var bullet in enemyBullet2List)
        {
            if (bullet.activeSelf == false)
                return bullet;
        }
        return null;
    }
    //적 오브젝트 풀링 메서드
    void CreateEnemyPooling()
    {
        var EnemyGroup = new GameObject("EnemyGroup");
        for(int i = 0; i < 10; i++)
        {
            int idx = Random.Range(0, 2);
            var enemy = Instantiate(enemyPrefab[idx], EnemyGroup.transform);
            enemy.name = $"{i + 1} 명";
            enemy.SetActive(false);
            enemyList.Add(enemy);
        }
    }
    IEnumerator EnemySpawn()
    {
        yield return new WaitForSeconds(0.02f);
        while (true)
        {
            
            foreach (var _enemy in enemyList)
            {
                if (GameManager.instance.isGameover) break;
                if (_enemy.activeSelf == false)
                {
                    _enemy.transform.position = spwanPointsList[Random.Range(0, spwanPointsList.Count)].position;
                    _enemy.gameObject.SetActive(true);
                    break;
                }
            }
            yield return new WaitForSeconds(3f);
        }
       
    }
}
