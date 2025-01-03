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
            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, hitNormal); //�ٱ������� �ǰ� Ƣ�� �ϼ���
            var blood = Instantiate(bloodEffect, hitPoint, rot);
            Destroy(blood , 1f);
        }
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    public override void Die()
    {
        enemyAI.Die();
        rb.Sleep();
        capCol.enabled = false;
        Destroy(gameObject, 5f);
        base.Die();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.transform.CompareTag(bulletTag))
        {
            Destroy(col.gameObject);
            Vector3 hitPoint = col.contacts[0].point;//ù ��°�� ������ ������ ��ġ
                                                     //(PlayerAndEnemyConnect.connect.fireBullet.firePos.position - hitPoint).normalized;
            Vector3 hitNormal = col.contacts[0].normal; //ù ��°�� ������ ������ ���� 
            OnDamage(25f, hitPoint, hitNormal);
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
