using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : LivingEntity
{
    [SerializeField] GameObject bloodEffect;
    [SerializeField] FireBullet fireBullet;
    [SerializeField] EnemyAI enemyAI;
    [SerializeField] private CapsuleCollider capCol;
    [SerializeField] private Rigidbody rb;

    private readonly string bulletTag = "BULLET";
    private readonly string playerTag = "Player";

    void Start()
    {
        bloodEffect = Resources.Load<GameObject>("Effect/BloodSprayFX");
        fireBullet = GameObject.FindGameObjectWithTag(playerTag).GetComponent<FireBullet>();
        enemyAI = GetComponent<EnemyAI>();
        capCol = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();

    }
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (dead == false)
        {
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, hitNormal); //바깥쪽으로 피가 튀게 하세요
            var blood = Instantiate(bloodEffect, hitPoint, rot);
            Destroy(blood , 1f);
        }
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    public override void Die()
    {
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
        capCol.enabled = true;
        rb.isKinematic = false;
        gameObject.SetActive(false);
        Health = InitHealth;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.transform.CompareTag(bulletTag))
        {
           
            Vector3 hitPoint = col.contacts[0].point;//첫 번째로 접촉한 지점의 위치
                                                     //(PlayerAndEnemyConnect.connect.fireBullet.firePos.position - hitPoint).normalized;
            Vector3 hitNormal = col.contacts[0].normal; //첫 번째로 접촉한 지점의 방향 
            col.gameObject.SetActive(false);
            OnDamage(25f, hitPoint, hitNormal);
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
