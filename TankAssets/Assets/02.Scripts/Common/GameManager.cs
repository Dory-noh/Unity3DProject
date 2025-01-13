using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public bool isGameOver = false;
    [SerializeField] GameObject apachePrefab = null;
    [SerializeField] List<Transform> spawnList;
    [SerializeField] List<GameObject> apacheList;

    GameObject apacheGroup;
    int maxEnemy = 10;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject); //다음 씬으로 넘어가도 게임 매니저는 존재한다.
        var spawnObj = GameObject.Find("SpawnPoints");
        if(spawnObj != null)
        {
            spawnObj.GetComponentsInChildren<Transform>(spawnList);
            spawnList.RemoveAt(0);
        }
    }
    void Start()
    {
        apacheGroup = new GameObject("apacheGroup");
        if(spawnList.Count > 0)
        {
            InvokeRepeating("SpawnApache", 0.01f, 5f);
        }   
    }

    void SpawnApache()  
    {
        if(apacheList.Count < maxEnemy)
        {
            int idx = Random.Range(0, spawnList.Count);
            Instantiate(apachePrefab, spawnList[idx].position, spawnList[idx].rotation, apacheGroup.transform);
            apacheList.Add(apachePrefab);
        }
    }
}
