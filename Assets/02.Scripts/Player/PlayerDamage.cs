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
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, hitNormal); //바깥쪽으로 피가 튀게 하세요
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
            Vector3 hitPoint = col.contacts[0].point;//첫 번째로 접촉한 지점의 위치
            Vector3 hitNormal = col.contacts[0].normal; //첫 번째로 접촉한 지점의 방향 
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
        ////Hierarchy에 있는 모든 enemy를 enemies 배열에 담는다.
        //for (int i = 0; i < enemies.Length; i++)
        //{
        //    enemies[i].gameObject.SendMessage("OnPlayerDie",
        //    SendMessageOptions.DontRequireReceiver);
        //}
    }
}
