using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager p_instance;



    //�Ѿ� ������Ʈ Ǯ�� - �÷��̾�
    [SerializeField] private GameObject playerBullet;
    [SerializeField] private List<GameObject> bulletList;

    //�Ѿ� ������Ʈ Ǯ�� - enemy1
    [SerializeField] private GameObject enemyBullet;
    [SerializeField] private List<GameObject> enemyBulletList;
    [SerializeField] private GameObject enemyBullet2;
    [SerializeField] private List<GameObject> enemyBullet2List;

    //�� ������Ʈ Ǯ��
    [SerializeField] private List<Transform> spwanPointsList;
    [SerializeField] private GameObject[] enemyPrefab = new GameObject[2];
    [SerializeField] private List<GameObject> enemyList;


    private readonly string strBulletCtrl = "Prefab/Bullet";
    private readonly string strEnemyBulletCtrl = "Prefab/E_Bullet";
    private readonly string strEnemyBullet2Ctrl = "Prefab/E2_Bullet";

    private void Awake()
    {
        //�ν��Ͻ� �ʱ�ȭ
        if (p_instance == null)
            p_instance = this;
        else if (p_instance != this)
        {
            Destroy(gameObject);
        }
        //�Ѿ� ����
        playerBullet = Resources.Load<GameObject>(strBulletCtrl);
        enemyBullet = Resources.Load<GameObject>(strEnemyBulletCtrl);
        enemyBullet2 = Resources.Load<GameObject>(strEnemyBullet2Ctrl);

        //�� ����
        enemyPrefab[0] = Resources.Load<GameObject>("Prefab/Enemy");
        enemyPrefab[1] = Resources.Load<GameObject>("Prefab/Enemy2");
        //�� ���� ��ġ ����
        var spawnPos = GameObject.Find("SpawnPoints").transform;
        if(spawnPos != null)
        {
            spawnPos.GetComponentsInChildren<Transform>(spwanPointsList);
            spwanPointsList.RemoveAt(0);
        }
        //������Ʈ Ǯ��
        CreatePooling();
        CreateEnemyPooling();
        CreateEnemyBulletPooling();
        CreateEnemyBullet2Pooling();
    }

    private void OnEnable()
    {
        StartCoroutine(EnemySpawn());
        //InvokeRepeating("EnemySpawn", 0.02f, 3f);//0.02�� ó�� ȣ��. �� ���� 3�� �������� ȣ���Ѵ�.
    }
    //�÷��̾� �Ѿ� ������Ʈ Ǯ�� �޼���
    private void CreatePooling()
    {
        GameObject playerBulletGroup = new GameObject("PlayerBulletGroup");
        for (int i = 0; i < 10; i++)
        {
            var bullet = Instantiate(playerBullet, playerBulletGroup.transform);
            bullet.name = $"{i+1} ��";
            bullet.SetActive(false);
            bulletList.Add(bullet);
        }
    }
    public GameObject GetBullet()
    {
        foreach(var bullet in bulletList)
        {
            if (bullet.activeSelf == false) //activeSelf: ������Ʈ�� ���¸� �˷��ִ� ������Ƽ
            {
                return bullet; //���� �͸� ��ȯ�Ѵ�.
            }
        }
        return null;
    }
    //��1 �Ѿ� ������Ʈ Ǯ�� �޼���
    private void CreateEnemyBulletPooling()
    {
        GameObject enemyBulletGroup = new GameObject("EnemyBulletGroup");
        for(int i = 0; i < 10; i++)
        {
            var bullet = Instantiate(enemyBullet, enemyBulletGroup.transform);
            bullet.name = $"enemy {i + 1}��";
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
    //��2 �Ѿ� ������Ʈ Ǯ�� �޼���
    private void CreateEnemyBullet2Pooling()
    {
        GameObject enemyBullet2Group = new GameObject("EnemyBullet2Group");
        for(int i = 0; i < 10; i++)
        {
            var bullet = Instantiate(enemyBullet2, enemyBullet2Group.transform);
            bullet.name = $"enemy2 {i + 1}��";
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
    //�� ������Ʈ Ǯ�� �޼���
    void CreateEnemyPooling()
    {
        var EnemyGroup = new GameObject("EnemyGroup");
        for(int i = 0; i < 10; i++)
        {
            int idx = Random.Range(0, 2);
            var enemy = Instantiate(enemyPrefab[idx], EnemyGroup.transform);
            enemy.name = $"{i + 1} ��";
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
