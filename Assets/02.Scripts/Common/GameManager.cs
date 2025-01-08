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
    public CanvasGroup inventroyCG; //CG: CanvasGroup이라는 뜻
    [HideInInspector] public int killCount; //inspector창에만 안 보이고 전역에서 사용 가능하다.
    public Text killCountText;

    private void Awake()
    {
        inventroyCG = GameObject.Find("Inventory").GetComponent<CanvasGroup>();
        OnInventoryOpen(false);
        LoadGameData();
    }
    public void LoadGameData() //게임의 초기 데이터 로드
    {
        //          PlayerPreference  저장할 데이터 예약 #딕셔너리 key value
        killCount = PlayerPrefs.GetInt("KILL_COUNT", 0); //키값 로드
        killCountText.text = $"Score : <color=#ff0000> {killCount.ToString("0000")} </color>"; //네 자리 형식으로 표현한다.
    }

    public void OnPuaseClick()
    {
        isPaused = !isPaused;
        Time.timeScale = (isPaused ? 0.0f : 1.0f);
        //시간을 멈추었을 때 화면을 클릭하면 총알이 발사되거나 총소리가 나는 문제 해결 
        var playerObj = GameObject.FindWithTag("Player").gameObject;
        var scripts = playerObj.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            script.enabled = !isPaused; //중지될 경우 true값을 가짐. script.enable은 false가 되어야 하므로 not 연산자를 붙임.
        }
        var canvasGroup = GameObject.Find("Panel-Weapon").GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = !isPaused; //중지시 비활성화
    }

    public void OnInventoryOpen(bool isOpen)
    {
        inventroyCG.alpha = isOpen ? 1.0f : 0.0f;
        Time.timeScale = (isOpen ? 0.0f : 1.0f);
        inventroyCG.interactable = isOpen;
        inventroyCG.blocksRaycasts = isOpen; //ui 이벤트 활성화
        if (isOpen)
        {
            var canvasGroup = GameObject.Find("Panel-Weapon").GetComponent<CanvasGroup>();
            canvasGroup.blocksRaycasts = false; //중지시 비활성화
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
            canvasGroup.blocksRaycasts = false; //중지시 비활성화
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
        PlayerPrefs.DeleteKey("KILL_COUNT"); //게임 종료시 killcount값 제거
    }
}
