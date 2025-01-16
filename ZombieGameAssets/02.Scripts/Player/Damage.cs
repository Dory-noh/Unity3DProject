using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Damage : MonoBehaviour
{
    public Image ScreenOpenClose;
    public Image HpBar;
    private float hp;
    private float hpInit = 100;
    
    Image bloodImage;

    private void OnEnable()
    {
        GameManager.OnItemChange += UpdateSetUp;
    }
    void Start()
    {
        HpBar.color = Color.green;
        bloodImage = GameObject.Find("Canvas-UI").transform.GetChild(5).GetComponent<Image>();
        bloodImage.color = Color.clear;

    }
    void UpdateSetUp()
    {
        hpInit = GameManager.Instance.gameData.hp;
        hp += GameManager.Instance.gameData.hp - hp;
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "PUNCH")
        {
            hp -= 10;
            StartCoroutine(ShowBloodImage());
        }
        if(other.gameObject.tag == "SWORD")
        {
            hp -= 15;
            StartCoroutine(ShowBloodImage());
        }
        if(other.gameObject.tag == "SMASH")
        {
            hp -= 8;
            StartCoroutine(ShowBloodImage());
            //Debug.Log("몬스터의 공격!");
        }
        hp = Mathf.Clamp(hp, 0, hpInit);
        HpBarMethod();
        
        if (hp <= 0)
            PlayerDie();
    }

    IEnumerator ShowBloodImage()
    {
        bloodImage.color = new Color((float)63 / 255, (float)9 / 255, (float)9 / 255, Random.Range(0.6f, 0.8f)); //Red임 - alpha값 랜덤 조정
        yield return new WaitForSeconds(1f);
        bloodImage.color = Color.clear;
    }
    void PlayerDie()
    {
        //print($"플레이어 사망 {hp.ToString()}");
        ScreenOpenClose.gameObject.SetActive(true);
        Invoke("SceneMove", 3.0f);
    }

    public void SceneMove()
    {
        SceneManager.LoadScene("EndScene");
    }

    private void HpBarMethod()
    {
        HpBar.fillAmount = (float)hp / (float)hpInit;
        if (HpBar.fillAmount <= 0.3f)
        {
            HpBar.color = Color.red;
        }
        else if (HpBar.fillAmount <= 0.5f)
        {
            HpBar.color = Color.yellow;
        }
    }
}
