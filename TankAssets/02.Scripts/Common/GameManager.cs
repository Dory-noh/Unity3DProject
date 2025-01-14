using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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
        PhotonNetwork.IsMessageQueueRunning = true; //PlayScene으로 오면 다시 메시지를 수신합니다.
    }
    void Start()
    {
        CreateTank();
        apacheGroup = new GameObject("apacheGroup");
        if(spawnList.Count > 0 && PhotonNetwork.IsMasterClient) //각 플레이어마다 Apache가 10대씩 생겨나면 안되니까 
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

    void CreateTank()
    {
        float pos = Random.Range(-100f, 100f);
        PhotonNetwork.Instantiate("Prefab/Tank", new Vector3(pos, 5f, pos), Quaternion.identity, 0);
        //개인전, 전달되는 것 없음
    }
}
