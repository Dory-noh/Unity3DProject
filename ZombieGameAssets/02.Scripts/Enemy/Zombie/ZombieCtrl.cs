using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
//1. 플레이어와 좀비의 거리 : 플레이어포지션 좀비포지션 
//2. 필요한 컴퍼넌트 : 콜라이더 ,리지디바디 
//3. 추적 :NavMesh agent 
public class ZombieCtrl : MonoBehaviour
{
    public Transform playerTr; //플레이어 위치
    public Transform zombieTr; //좀비 위치 
    public NavMeshAgent navie; //추적 하는 내비
    public float attackdist = 2.8f; //공격 거리 
    public float tracedist = 20f; //추적 범위의 거리 
    public Animator animator;
    public ZombieDamage damage;
    void Start()
    {
        damage = GetComponent<ZombieDamage>();
        playerTr = GameObject.FindWithTag("Player").transform;
        zombieTr = transform;
        //하이라키 에서 Player 태그를 가진 트랜스폼을 가지고 온다.
    }
    void Update()
    {
        if (damage.isDie == true) return;

        // 거리를 구함 좀비 플레이어의 거리 
        float dist =Vector3.Distance(playerTr.position, zombieTr.position); 
        if (dist <= attackdist) //거리가 공격범위 안이라면....
        {
            navie.isStopped = true; //추적 중지 
            animator.SetBool("IsAttack",true);              //타겟 포지션 - 자기 자신 위치 = 방향 -> 타겟 방향으로 회전한다.
            Quaternion rot = Quaternion.LookRotation(playerTr.position - zombieTr.position);
            zombieTr.rotation = Quaternion.Slerp(zombieTr.rotation, rot, Time.deltaTime * 3);
                                //곡면보간 선형보간
        }
        else if(dist <= tracedist)
        {
            navie.isStopped = false; //추적 시작 
            navie.destination = playerTr.position;
            // 추적 대 상  = 플레이어 포지션
            animator.SetBool("IsAttack",false); //공격 애니 x
            animator.SetBool("IsTrace",true ); // 걷기 애니 o
        }
        else
        {
            animator.SetBool("IsTrace", false);
            navie.isStopped = true;
        }

    }
}
