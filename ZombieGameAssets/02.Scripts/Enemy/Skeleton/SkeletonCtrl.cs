using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//1.거리 : 플레이어 위치 랑 스켈레톤 위치
//2.플레이어 추적 : NavMeshAgent 컴퍼넌트
//3. 공격 거리 추적 거리 
//4. 애니메이터를 이용해서 애니메이션 연동
public class SkeletonCtrl : MonoBehaviour
{
    public Transform playerTr;
    public Transform skeletonTr;
    public NavMeshAgent navi;
    public Animator animator;
    public float attackDist = 3.0f;
    public float traceDist = 20f;
    public SkeletonDamage skeletonDamage;
    void Start()
    {             //하이라키에서 Player태그를 가진 오브젝트의 위치를 찾음
        playerTr = GameObject.FindWithTag("Player").transform;
        skeletonTr = GetComponent<Transform>();
        // 현재 자신의 오브젝트 안에 있는 해당 컴퍼넌트를 넣는 다. 초기화 한다.
        navi = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        skeletonDamage = GetComponent<SkeletonDamage>();
    }
    void Update()
    {
        if (skeletonDamage.isDie == true) return;

        float dist = Vector3.Distance(playerTr.position, skeletonTr.position);
        if (dist <= attackDist) //공격 범위 안이면 
        {
            navi.isStopped = true;// 네비 추적 중지 
            animator.SetBool("IsAttack", true);
            Quaternion rot = Quaternion.LookRotation(playerTr.position - skeletonTr.position);
            skeletonTr.rotation = Quaternion.Slerp(skeletonTr.rotation, rot, Time.deltaTime * 3.0f);
        }
        else if (dist <= traceDist) //추적 범위 안이면
        {
            navi.isStopped = false; //추적 활성화 
            navi.destination = playerTr.position; //추적 대상은 플레이어가 된다.
            animator.SetBool("IsAttack", false);
            animator.SetBool("IsTrace",true);
        }
        else
        {
            navi.isStopped = true;
            animator.SetBool("IsTrace",false);
        }
    }
}
