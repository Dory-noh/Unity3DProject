using DataInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; //#싱글톤 전지역변수이다. GameManger가 어디든 쉽게·빨리 접근 가능하도록 싱글톤으로 만든다.
    //좀비들이 3초 간격으로 태어난다. 스켈레톤은 5초 간격으로 태어난다. 몬스터는 10초 간격으로 태어난다.
    public DataManager2 dataManager;
    public GameObject zombiePrefab;
    public GameObject skeletonPrefab;
    public GameObject monsterPrefab;

    public Transform[] spawnPoints;
    private float[] timePrev;
    private int maxZombie = 10;
    private int maxSkeleton = 5;
    private int maxMonster = 3;
    public GameData gameData;
    public Text killTxt;
    private bool pause;

    //델리게이트 및 이벤트 선언
    public delegate void ItemChangeDelegate();
    public static event ItemChangeDelegate OnItemChange;
    CanvasGroup canvasGroup;
    public GameObject slotList;
    public GameObject[] itemObjects;

    private void Awake() //start보다 먼저 호출된다. Start()보다 미리 초기화할 것들 - 맵 로딩 (맵이 생기고 그 다음에 적이 생성되야 한다~ 등 생성 순서를 지켜야 할 때 쓴다.)
    {
        Instance = this; //자기 자신 클래스 //Instance = new GameManager();;
        //싱글톤 기법. 매니터에 접근을 쉽게 하기 위해서 사용
        //다른 클래스에 빨리 접근하기 위해 싱글톤 기법을 사용한다.
        //싱글톤 기법을 사용하면 객체(인스턴스) 생성은 한 번만 가능하다. 여러 번 생성하는 것을 막기 위해 사용한다.
        pause = false;

    }
    void Start() //적 태어나게 하기
    {
        spawnPoints = GameObject.Find("SpawnPoints").GetComponentsInChildren<Transform>();
        //자기 자신과 자기 자신의 자식들의 트랜스폼을 가져와 spawnPoints에 넣는다.
        timePrev = new float[]{ Time.time, Time.time, Time.time};
        canvasGroup = GameObject.Find("Inventory").GetComponent<CanvasGroup>();
        canvasGroup.alpha = -1.0f;
        canvasGroup.blocksRaycasts = false;
        ShowInventory(false);
        slotList = GameObject.Find("SlotList");
        dataManager = GetComponent<DataManager2>();
        dataManager.Initialize();
        LoadGameData();
        

    }

    

    public void LoadGameData()
    {

        //killCount = PlayerPrefs.GetInt("KILL_COUNT", 0);
        GameData data = dataManager.Load();
        gameData.hp = data.hp;
        gameData.damage = data.damage;
        gameData.speed = data.speed;
        gameData.killCount = data.killCount;
        gameData.equipItems = data.equipItems;
        if(gameData.equipItems.Count > 0)
        {
            InventorySetUp();
        }
        killTxt.text = killTxt.text = $"Kill: <color=#ff0000>{gameData.killCount.ToString()}</color>";
    }
    public void AddItem(Item item)
    {
        if (gameData.equipItems.Contains(item)) return;
        gameData.equipItems.Add(item);
        switch (item.itemType)
        {
            case Item.ItemType.HP:
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.hp += item.value;
                else
                    gameData.hp += gameData.hp * item.value;
                break;
            case Item.ItemType.DAMAGE:
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.damage += item.value;
                else
                    gameData.damage += gameData.damage * item.value;
                break;
            case Item.ItemType.SPEED:
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.speed += item.value;
                else
                    gameData.speed += gameData.speed * item.value;
                break;
            case Item.ItemType.GRENADE:
                break;
        }
        OnItemChange();
    }
    public void RemoveItem(Item item)
    {
        gameData.equipItems.Remove(item);
        switch (item.itemType)
        {
            case Item.ItemType.HP:
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.hp -= item.value;
                else
                    gameData.hp = gameData.hp / (1.0f + item.value);
                break;
            case Item.ItemType.DAMAGE:
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.damage -= item.value;
                else
                    gameData.damage = gameData.damage / (1+item.value);
                break;
            case Item.ItemType.SPEED:
                if (item.itemCalc == Item.ItemCalc.VALUE)
                    gameData.speed -= item.value;
                else
                    gameData.speed = gameData.speed / (1+item.value);
                break;
            case Item.ItemType.GRENADE:
                break;
        }
        OnItemChange();
    }

    void InventorySetUp()
    {
        var slots = slotList.GetComponentsInChildren<Transform>();
        for (int i = 0; i < gameData.equipItems.Count; i++)
        {
            for(int j = 1; j < slots.Length; j++)
            {
                if (slots[j].childCount > 0) continue;
                int itemIndex = (int)gameData.equipItems[i].itemType; //현재 equipItems[i]가 가리키고 있는 아이템의 itemType enum값
                itemObjects[itemIndex].GetComponent<Transform>().SetParent(slots[j]);
                itemObjects[itemIndex].GetComponent<ItemInfo>().itemData = gameData.equipItems[i]; //?
                break;
            }
        }
    }

    public void ShowInventory(bool isOpen)
    {
        Time.timeScale = isOpen ? 0.0f : 1.0f;
        
        canvasGroup.alpha = isOpen ? 1.0f : -1f;
        canvasGroup.blocksRaycasts = isOpen ? true : false;
        var PlayerObj = GameObject.FindGameObjectWithTag("Player");
        var scripts = PlayerObj.GetComponentsInChildren<MonoBehaviour>();
        foreach (var script in scripts)
        {
            script.enabled = !isOpen;
        }
    }

    void Update()
    {
        if (Time.time - timePrev[0] >= 3.0) //지나간 시간
        {
            int zombieCount = GameObject.FindGameObjectsWithTag("ZOMBIE").Length;
            //하이라키에서 ZOMBIE태그 가진 오브젝트 수 리턴
            if(maxZombie > zombieCount)
            {
                SpawnZombie();
            }
            timePrev[0] = Time.time;

        }
        if (Time.time - timePrev[1] >= 5.0)
        {
            int skeletonCount = GameObject.FindGameObjectsWithTag("SKELETON").Length;
            if(maxSkeleton > skeletonCount) SpawnSkeleton();
            timePrev[1] = Time.time;
        }
        if (Time.time - timePrev[2] >= 10.0)
        {
            int monsterCount = GameObject.FindGameObjectsWithTag("MONSTER").Length;
            if(maxMonster > monsterCount) SpawnMonster();
            timePrev[2] = Time.time;
        }
    }

    public void pauseGame()
    {
        pause = !pause;
        Time.timeScale = pause ? 0.0f : 1.0f;
        //플레이어 총 쏘기 잠금
        var PlayerObj = GameObject.FindGameObjectWithTag("Player");
        var scripts = PlayerObj.GetComponentsInChildren<MonoBehaviour>();
        foreach (var script in scripts)
        {
            script.enabled = !pause;
        }
    }

    private void SpawnMonster()
    {
        int idx = Random.Range(1, spawnPoints.Length);
        Instantiate(monsterPrefab, spawnPoints[idx].position, spawnPoints[idx].rotation);
    }

    private void SpawnSkeleton()
    {
        int idx = Random.Range(1, spawnPoints.Length);
        Instantiate(skeletonPrefab, spawnPoints[idx].position, spawnPoints[idx].rotation);
    }

    void SpawnZombie()
    {
        int idx = Random.Range(1, spawnPoints.Length); //0인덱스엔 부모가 있으므로 스킵하도록 함.
        Instantiate(zombiePrefab, spawnPoints[idx].position, spawnPoints[idx].rotation);
    }

    public void Killscore(int _score)
    {
        gameData.killCount += _score;
        //PlayerPrefs.SetInt("KILL_COUNT", gameData.killCount);
        //PlayerPrefs.Save();
        killTxt.text = $"Kill: <color=#ff0000>{gameData.killCount.ToString()}</color>"; //문자하고 정수를 표시하는 것은 문자만 표시하는 것보다 느리다.
       
    }

    private void OnApplicationQuit()
    {
        //PlayerPrefs.DeleteKey("KILL_COUNT");
        dataManager.Save(gameData);
    }
}
