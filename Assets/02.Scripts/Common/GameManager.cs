using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance = null;
    public static GameManager instance
    {
        get {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<GameManager>();
            }
            return m_instance;
        }
    }
    public bool isGameover { get; set; } = false;
    private bool isPaused = false;
    public CanvasGroup inventroyCG; //CG: CanvasGroup�̶�� ��
    [HideInInspector] public int killCount; //inspectorâ���� �� ���̰� �������� ��� �����ϴ�.
    public Text killCountText;

    private void Awake()
    {
        inventroyCG = GameObject.Find("Inventory").GetComponent<CanvasGroup>();
        OnInventoryOpen(false);
        LoadGameData();
    }
    public void LoadGameData() //������ �ʱ� ������ �ε�
    {
        //          PlayerPreference  ������ ������ ���� #��ųʸ� key value
        killCount = PlayerPrefs.GetInt("KILL_COUNT", 0); //Ű�� �ε�
        killCountText.text = $"Score : <color=#ff0000> {killCount.ToString("0000")} </color>"; //�� �ڸ� �������� ǥ���Ѵ�.
    }

    public void OnPuaseClick()
    {
        isPaused = !isPaused;
        Time.timeScale = (isPaused ? 0.0f : 1.0f);
        //�ð��� ���߾��� �� ȭ���� Ŭ���ϸ� �Ѿ��� �߻�ǰų� �ѼҸ��� ���� ���� �ذ� 
        var playerObj = GameObject.FindWithTag("Player").gameObject;
        var scripts = playerObj.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            script.enabled = !isPaused; //������ ��� true���� ����. script.enable�� false�� �Ǿ�� �ϹǷ� not �����ڸ� ����.
        }
        var canvasGroup = GameObject.Find("Panel-Weapon").GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = !isPaused; //������ ��Ȱ��ȭ
    }

    public void OnInventoryOpen(bool isOpen)
    {
        inventroyCG.alpha = isOpen ? 1.0f : 0.0f;
        Time.timeScale = (isOpen ? 0.0f : 1.0f);
        inventroyCG.interactable = isOpen;
        inventroyCG.blocksRaycasts = isOpen; //ui �̺�Ʈ Ȱ��ȭ
        if (isOpen)
        {
            var canvasGroup = GameObject.Find("Panel-Weapon").GetComponent<CanvasGroup>();
            canvasGroup.blocksRaycasts = false; //������ ��Ȱ��ȭ
        }
    }

    public void KillScoreCount()
    {
        ++killCount;
        PlayerPrefs.SetInt("KILL_COUNT", killCount);
        PlayerPrefs.Save();
        killCountText.text = $"Score : <color=#ff0000> {killCount.ToString("0000")} </color>";
    }

    void Update()
    {
        if (isGameover)
        {
            var canvasGroup = GameObject.Find("Panel-Weapon").GetComponent<CanvasGroup>();
            canvasGroup.blocksRaycasts = false; //������ ��Ȱ��ȭ
            var inventoryGroup = GameObject.Find("InventoryBtn").GetComponent<CanvasGroup>();
            inventoryGroup.blocksRaycasts = false;
            var pauseGroup = GameObject.Find("Panel-Pause").GetComponent<CanvasGroup>();
            pauseGroup.blocksRaycasts = false;
            
        }

    }
    private void OnDisable()
    {
        
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("KILL_COUNT"); //���� ����� killcount�� ����
    }
}
