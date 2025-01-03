using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : LivingEntity
{
    [SerializeField] GameObject bloodEffect;
    //[SerializeField] EnemyFire enemyFire;
    private readonly string E_bulletTag = "E_BULLET";

    void Start()
    {
        bloodEffect = Resources.Load<GameObject>("Effect/BloodSprayFX");
    }
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (dead == false)
        {
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, hitNormal); //�ٱ������� �ǰ� Ƣ�� �ϼ���
            var blood = Instantiate(bloodEffect, hitPoint, rot);
            Destroy(blood, 1f);
        }
        base.OnDamage(damage, hitPoint, hitNormal);
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.transform.CompareTag(E_bulletTag))
        {
            Destroy(col.gameObject);
            Vector3 hitPoint = col.contacts[0].point;//ù ��°�� ������ ������ ��ġ
            Vector3 hitNormal = col.contacts[0].normal; //ù ��°�� ������ ������ ���� 
            OnDamage(25f, hitPoint, hitNormal);
        }
    }
}
