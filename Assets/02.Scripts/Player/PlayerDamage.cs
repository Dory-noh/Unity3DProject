using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamage : LivingEntity
{
    [SerializeField] GameObject bloodEffect;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody rb;
    //[SerializeField] EnemyFire enemyFire;
    private readonly string E_bulletTag = "E_BULLET";
    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashDieIdx = Animator.StringToHash("DieIdx");
    public bool isPlayerDie = false;
    [SerializeField] Image BloodScreen;
    [SerializeField] Text imgTxt;
    [SerializeField] Image hpBar;
    WaitForSeconds screenWs = new WaitForSeconds(1.0f);
    void Start()
    {
        BloodScreen = GameObject.Find("Canvas-UI").transform.GetChild(0).GetComponent<Image>();
        rb = GetComponent<Rigidbody>();
        bloodEffect = Resources.Load<GameObject>("Effect/BloodSprayFX");
        animator = GetComponent<Animator>();
        imgTxt = GameObject.Find("Canvas-UI").transform.GetChild(1).GetChild(0).GetComponent<Text>();
        hpBar = GameObject.Find("Canvas-UI").transform.GetChild(1).GetChild(2).GetComponent<Image>();
        hpBar.color = Color.green;
    }
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (isPlayerDie == false)
        {
            StartCoroutine(ShowBloodScreen());
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, hitNormal); //�ٱ������� �ǰ� Ƣ�� �ϼ���
            var blood = Instantiate(bloodEffect, hitPoint, rot);
            Destroy(blood, 1f);
            
        }
        base.OnDamage(damage, hitPoint, hitNormal);
        ShowHpBar();
    }

    private void ShowHpBar()
    {
        hpBar.fillAmount = Health / InitHealth; //�� �� float ���̹Ƿ� ����� ����ȯ�� ������ ����.
        if (hpBar.fillAmount < 0.3f)
        {
            hpBar.color = Color.red;
            imgTxt.color = Color.red;
        }
        else if (hpBar.fillAmount < 0.5f)
        {
            hpBar.color = Color.blue;
            imgTxt.color = Color.blue;
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.transform.CompareTag(E_bulletTag) && isPlayerDie == false)
        {
            //Destroy(col.gameObject);
            col.gameObject.SetActive(false);
            Vector3 hitPoint = col.contacts[0].point;//ù ��°�� ������ ������ ��ġ
            Vector3 hitNormal = col.contacts[0].normal; //ù ��°�� ������ ������ ���� 
            OnDamage(5f, hitPoint, hitNormal);
            if(Health <= 0f)
            {
                Die();
            }
        }
    }

    IEnumerator ShowBloodScreen()
    {
        BloodScreen.color = new Color(1, 0, 0, Random.Range(0.3f, 0.4f)); //Red�� - alpha�� ���� ����
        yield return screenWs;
        BloodScreen.color = Color.clear;
    }

    public override void Die()
    {
        isPlayerDie = true;
        
        animator.SetTrigger(hashDie);
        animator.SetInteger(hashDieIdx, Random.Range(0, 3));
        GetComponent<CapsuleCollider>().enabled = false;
        rb.useGravity = false;
        rb.isKinematic = true;
        base.Die();
        GameManager.instance.isGameover = true;
        //GameObject[] enemies = GameObject.FindGameObjectsWithTag("ENEMY");
        ////Hierarchy�� �ִ� ��� enemy�� enemies �迭�� ��´�.
        //for (int i = 0; i < enemies.Length; i++)
        //{
        //    enemies[i].gameObject.SendMessage("OnPlayerDie",
        //    SendMessageOptions.DontRequireReceiver);
        //}
    }


}
