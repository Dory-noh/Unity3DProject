using DataInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; //#�̱��� �����������̴�. GameManger�� ���� ���ԡ����� ���� �����ϵ��� �̱������� �����.
    //������� 3�� �������� �¾��. ���̷����� 5�� �������� �¾��. ���ʹ� 10�� �������� �¾��.
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

    //��������Ʈ �� �̺�Ʈ ����
    public delegate void ItemChangeDelegate();
    public static event ItemChangeDelegate OnItemChange;
    CanvasGroup canvasGroup;
    public GameObject slotList;
    public GameObject[] itemObjects;

    private void Awake() //start���� ���� ȣ��ȴ�. Start()���� �̸� �ʱ�ȭ�� �͵� - �� �ε� (���� ����� �� ������ ���� �����Ǿ� �Ѵ�~ �� ���� ������ ���Ѿ� �� �� ����.)
    {
        Instance = this; //�ڱ� �ڽ� Ŭ���� //Instance = new GameManager();;
        //�̱��� ���. �Ŵ��Ϳ� ������ ���� �ϱ� ���ؼ� ���
        //�ٸ� Ŭ������ ���� �����ϱ� ���� �̱��� ����� ����Ѵ�.
        //�̱��� ����� ����ϸ� ��ü(�ν��Ͻ�) ������ �� ���� �����ϴ�. ���� �� �����ϴ� ���� ���� ���� ����Ѵ�.
        pause = false;

    }
    void Start() //�� �¾�� �ϱ�
    {
        spawnPoints = GameObject.Find("SpawnPoints").GetComponentsInChildren<Transform>();
        //�ڱ� �ڽŰ� �ڱ� �ڽ��� �ڽĵ��� Ʈ�������� ������ spawnPoints�� �ִ´�.
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
                int itemIndex = (int)gameData.equipItems[i].itemType; //���� equipItems[i]�� ����Ű�� �ִ� �������� itemType enum��
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
        if (Time.time - timePrev[0] >= 3.0) //������ �ð�
        {
            int zombieCount = GameObject.FindGameObjectsWithTag("ZOMBIE").Length;
            //���̶�Ű���� ZOMBIE�±� ���� ������Ʈ �� ����
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
        //�÷��̾� �� ��� ���
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
        int idx = Random.Range(1, spawnPoints.Length); //0�ε����� �θ� �����Ƿ� ��ŵ�ϵ��� ��.
        Instantiate(zombiePrefab, spawnPoints[idx].position, spawnPoints[idx].rotation);
    }

    public void Killscore(int _score)
    {
        gameData.killCount += _score;
        //PlayerPrefs.SetInt("KILL_COUNT", gameData.killCount);
        //PlayerPrefs.Save();
        killTxt.text = $"Kill: <color=#ff0000>{gameData.killCount.ToString()}</color>"; //�����ϰ� ������ ǥ���ϴ� ���� ���ڸ� ǥ���ϴ� �ͺ��� ������.
       
    }

    private void OnApplicationQuit()
    {
        //PlayerPrefs.DeleteKey("KILL_COUNT");
        dataManager.Save(gameData);
    }
}
