using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        bloodEffect = Resources.Load<GameObject>("Effect/BloodSprayFX");
        animator = GetComponent<Animator>();
    }
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (isPlayerDie == false)
        {
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, hitNormal); //�ٱ������� �ǰ� Ƣ�� �ϼ���
            var blood = Instantiate(bloodEffect, hitPoint, rot);
            Destroy(blood, 1f);
        }
        base.OnDamage(damage, hitPoint, hitNormal);
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
    public override void Die()
    {
        isPlayerDie = true;
        
        animator.SetTrigger(hashDie);
        animator.SetInteger(hashDieIdx, Random.Range(0, 3));
        GetComponent<CapsuleCollider>().enabled = false;
        rb.useGravity = false;
        rb.isKinematic = true;

        base.Die();
        //GameObject[] enemies = GameObject.FindGameObjectsWithTag("ENEMY");
        ////Hierarchy�� �ִ� ��� enemy�� enemies �迭�� ��´�.
        //for (int i = 0; i < enemies.Length; i++)
        //{
        //    enemies[i].gameObject.SendMessage("OnPlayerDie",
        //    SendMessageOptions.DontRequireReceiver);
        //}
    }
}
