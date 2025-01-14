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
        DontDestroyOnLoad(gameObject); //���� ������ �Ѿ�� ���� �Ŵ����� �����Ѵ�.
        var spawnObj = GameObject.Find("SpawnPoints");
        if(spawnObj != null)
        {
            spawnObj.GetComponentsInChildren<Transform>(spawnList);
            spawnList.RemoveAt(0);
        }
        PhotonNetwork.IsMessageQueueRunning = true; //PlayScene���� ���� �ٽ� �޽����� �����մϴ�.
    }
    void Start()
    {
        CreateTank();
        apacheGroup = new GameObject("apacheGroup");
        if(spawnList.Count > 0 && PhotonNetwork.IsMasterClient) //�� �÷��̾�� Apache�� 10�뾿 ���ܳ��� �ȵǴϱ� 
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
        //������, ���޵Ǵ� �� ����
    }
}
