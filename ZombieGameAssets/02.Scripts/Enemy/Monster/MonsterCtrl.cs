using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterCtrl : MonoBehaviour
{
    public Transform PlayerTr;
    public Transform MonsterTr;
    public NavMeshAgent navi;
    public Animator animator;
    public float AttackDist = 2.0f;
    public float TraceDist = 20f;
    public MonsterDamage monsterDamage;
    // Start is called before the first frame update
    void Start()
    {
        PlayerTr = GameObject.FindGameObjectWithTag("Player").transform;
        MonsterTr = GetComponent<Transform>();
        navi = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        monsterDamage = GetComponent<MonsterDamage>();
    }

    // Update is called once per frame
    void Update()
    {
        if (monsterDamage.isDie == true) return;
        float dist = Vector3.Distance(PlayerTr.position, MonsterTr.position);
        if (dist <= AttackDist)
        {
            navi.isStopped = true;
            animator.SetBool("IsAttack", true);
            Quaternion rot = Quaternion.LookRotation(PlayerTr.position - MonsterTr.position);
            MonsterTr.rotation = Quaternion.Slerp(MonsterTr.rotation, rot, Time.deltaTime * 3.0f);
        }
        else if (dist <= TraceDist)
        {
            navi.isStopped = false;
            navi.destination = PlayerTr.position;
            animator.SetBool("IsTrace", true);
            animator.SetBool("IsAttack", false);
        }
        else //플레이어와의 거리가 20 초과인 경우
        {
            navi.isStopped = true;
            animator.SetBool("IsTrace", false);
        }
    }
}
