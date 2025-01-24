using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager p_Instance;
    [SerializeField] GameObject EnemyPrefab;
    [SerializeField] List<GameObject> enemyList = new List<GameObject>();
    [SerializeField] List<Transform> respawnPoints;
    private void Awake()
    {
        if(p_Instance == null) p_Instance = this;
        else if(p_Instance != this) Destroy(gameObject);
    }
    void Start()
    {
        GameObject.Find("Enemy Spawn Points").transform.GetComponentsInChildren<Transform>(respawnPoints);
        respawnPoints.RemoveAt(0);

        EnemyPrefab = Resources.Load<GameObject>("Enemy");
        for(int i = 0; i < 10; i++)
        {
            var enemy = Instantiate(EnemyPrefab, gameObject.transform);
            enemy.SetActive(false);
            enemyList.Add(enemy);
        }
        StartCoroutine(RespawnEnemy());
    }

    IEnumerator RespawnEnemy()
    {
        while (true)
        {
            var enemy = GetEnemy();
            if(enemy != null) {
                yield return new WaitForSeconds(5f);
            }
            else
            {
                yield return null;
            }
            
        }
    }

    public GameObject GetEnemy()
    {
        if (GameManager.Instance.isGameover == true && GameManager.Instance != null)
        {
            StartCoroutine(RemoveEnemy());
            return null;
        }
        else
        {
            foreach (var enemy in enemyList)
            {
                if (enemy.activeSelf == false)
                {
                    int idx = Random.Range(0, respawnPoints.Count);
                    enemy.transform.position = respawnPoints[idx].position;
                    enemy.transform.rotation = respawnPoints[idx].rotation;
                    enemy.SetActive(true);
                    return enemy;
                }
            }
        }
        
        return null;
    }

    IEnumerator RemoveEnemy()
    {
        yield return new WaitForSeconds(0.5f);
        foreach (var enemy in enemyList)
        {
            if (enemy.activeSelf == true)
            {
                enemy.GetComponent<EnemyMovement>().OnDie();
            }
        }
    }
}
