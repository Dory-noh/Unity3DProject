using PlasticGui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : LivingEntity 
{
    [SerializeField] GameObject bloodEffect;
    [SerializeField] Rigidbody rb;
    [SerializeField] CapsuleCollider capCol;
    [SerializeField] Enemy2AI enemyAI;

    private readonly string bulletTag = "BULLET";
    private void Start()
    {
        bloodEffect = Resources.Load<GameObject>("Effect/BloodSprayFX");
        rb = GetComponent<Rigidbody>();
        capCol = GetComponent<CapsuleCollider>();
        enemyAI = GetComponent<Enemy2AI>();
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
        enemyAI.Die();
        capCol.enabled = false;
        rb.isKinematic = true;
        //base.Die();
        if(dead==false)
        StartCoroutine(PoolPush());
    }
    IEnumerator PoolPush()
    {
        yield return new WaitForSeconds(3.0f);
        capCol.enabled = true;
        rb.isKinematic = false;
        gameObject.SetActive(false);
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
        }   
    }

}
