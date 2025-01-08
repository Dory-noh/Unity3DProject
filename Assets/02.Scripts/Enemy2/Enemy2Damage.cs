using PlasticGui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamage : LivingEntity 
{
    [SerializeField] GameObject bloodEffect;
    [SerializeField] Rigidbody rb;
    [SerializeField] CapsuleCollider capCol;
    [SerializeField] Enemy2AI enemyAI;

    [Header("HpBar")]
    //[SerializeField] GameObject hpBarPrefab;
    [SerializeField] GameObject hpBar;
    [SerializeField] Vector3 hpBarOffset = new Vector3(0, 2.2f, 0);
    private Canvas uiCanvas;
    private Image hpBarImage;
    

    private readonly string bulletTag = "BULLET";
    private void Start()
    {
        bloodEffect = Resources.Load<GameObject>("Effect/BloodSprayFX");
        rb = GetComponent<Rigidbody>();
        capCol = GetComponent<CapsuleCollider>();
        enemyAI = GetComponent<Enemy2AI>();
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
        
        hpBarImage = hpBar.GetComponentsInChildren<Image>()[1]; //getcomponetsinchildren은 부모인 hpbar을 0인덱스로 포함하기 때문에 첫 번째 자식 인덱스를 가져오기 위해 [1] 입력함요
        hpBar.SetActive(true);

        var _hpBar = hpBar.GetComponent<EnemyHpBar>();
        _hpBar.targetTr = this.gameObject.transform;
        _hpBar.offset = hpBarOffset;

        hpBarImage.fillAmount = Health / InitHealth;
        hpBarImage.color = Color.red;
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if(dead == false)
        {
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, hitNormal);
            var blood = Instantiate(bloodEffect, hitPoint, rot);
            Destroy(blood, 1f);
        }
        base.OnDamage(damage, hitPoint, hitNormal);
    }
    public override void Die()
    {
        if (hpBarImage != null)
            hpBarImage.color = Color.clear;
        //if (enemyAI.isDie) return;
        
        enemyAI.Die();
        capCol.enabled = false;
        rb.isKinematic = true;
        hpBarImage.color = Color.clear;
        //if (hpBar != null)
        //{
        //    hpBar.gameObject.SetActive(false);
        //    hpBarImage.color = Color.clear;
        //}

        //base.Die();
        
        if (dead==false)
        StartCoroutine(PoolPush());
    }
    IEnumerator PoolPush()
    {
        yield return new WaitForSeconds(3.0f);
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
            Vector3 hitPoint = col.contacts[0].point;
            Vector3 hitNormal = col.contacts[0].normal;
            OnDamage(30f, hitPoint, hitNormal);
            GetComponent<Rigidbody>().isKinematic = true;
            hpBarImage.fillAmount = Health / InitHealth; //float형임
            if (Health <= 0f)
            {
                Die();
                
            }
        }   
    }
    private void OnDisable()
    {
        if(hpBar!=null)
        hpBar.gameObject.SetActive(false);
    }
}
