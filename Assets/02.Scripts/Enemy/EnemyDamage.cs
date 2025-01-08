using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamage : LivingEntity
{
    [SerializeField] GameObject bloodEffect;
    [SerializeField] EnemyAI enemyAI;
    [SerializeField] private CapsuleCollider capCol;
    [SerializeField] private Rigidbody rb;

    [Header("HpBar")]
    //[SerializeField] GameObject hpBarPrefab;
    [SerializeField] GameObject hpBar;
    [SerializeField] Vector3 hpBarOffset = new Vector3 (0,2.2f, 0);
    private Canvas uiCanvas;
    private Image hpBarImage;

    private readonly string bulletTag = "BULLET";

   

    void Start()
    {
        bloodEffect = Resources.Load<GameObject>("Effect/BloodSprayFX");
        enemyAI = GetComponent<EnemyAI>();
        capCol = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        //StartCoroutine(SetHpBar());
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(SetHpBar());
    }

    IEnumerator SetHpBar()
    {
        yield return new WaitForSeconds(0.3f);

        hpBar = PoolingManager.p_instance.GetHpBar();
        hpBarImage = hpBar.GetComponentsInChildren<Image>()[1]; //getcomponetsinchildren�� �θ��� hpbar�� 0�ε����� �����ϱ� ������ ù ��° �ڽ� �ε����� �������� ���� [1] �Է��Կ�
        hpBar.SetActive(true);
        var _hpBar = hpBar.GetComponent<EnemyHpBar>();
        _hpBar.targetTr = this.gameObject.transform;
        _hpBar.offset = hpBarOffset;

        hpBarImage.fillAmount = Health / InitHealth;
        hpBarImage.color = Color.red;
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (dead == false)
        {
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, hitNormal); //�ٱ������� �ǰ� Ƣ�� �ϼ���
            var blood = Instantiate(bloodEffect, hitPoint, rot);
            Destroy(blood , 1f);
        }
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    public override void Die()
    {
        if(hpBarImage != null)
        hpBarImage.color = Color.clear;
        //if (enemyAI.isDie) return;
        
        enemyAI.Die();
        capCol.enabled = false;
        rb.isKinematic = true;
        //base.Die();
        
        if (dead == false)
            StartCoroutine(PoolPush());
    }

    IEnumerator PoolPush()
    {
        yield return new WaitForSeconds(3f);
        //capCol.enabled = true;
        //rb.isKinematic = false;
        //gameObject.SetActive(false);
        dead = false;
        Health = InitHealth;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.transform.CompareTag(bulletTag))
        {
            col.gameObject.SetActive(false);
            Vector3 hitPoint = col.contacts[0].point;//ù ��°�� ������ ������ ��ġ
                                                     //(PlayerAndEnemyConnect.connect.fireBullet.firePos.position - hitPoint).normalized;
            Vector3 hitNormal = col.contacts[0].normal; //ù ��°�� ������ ������ ���� 
            
            OnDamage(25f, hitPoint, hitNormal);
            GetComponent<Rigidbody>().isKinematic = true;
            hpBarImage.fillAmount = Health / InitHealth; //float����

            if(Health <= 0f)
            {
                //hpBar.SetActive(false);
                Die();
            }
        }
    }

    private void OnDisable()
    {
        if (hpBar != null)
            hpBar.gameObject.SetActive(false);
    }
}
