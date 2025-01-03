using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum State
{
    idle,
    trace,
    attack,
    patrol,
    damage,
    die
}

public class EnemyAI : MonoBehaviour
{
    static State state = State.idle;

    [SerializeField] private Animator animator;
    [SerializeField] private MoveAgent moveAgent;
    [SerializeField] private Transform playerTr;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform tr;
    [SerializeField] private EnemyFire enemyFire;

    private float dist = 0;
    [SerializeField] private float attackDist = 6f;
    [SerializeField] private float traceDist = 12f;
    private bool isDie = false;

    private readonly string tagPlayer = "Player";
    private readonly int hashIsMove = Animator.StringToHash("IsMove");
    private readonly int hashMoveSpeed = Animator.StringToHash("moveSpeed");
    private readonly int hashFireTrigger = Animator.StringToHash("FireTrigger");
    private readonly int hashDieIdx = Animator.StringToHash("DieIdx");
    private readonly int hashDie = Animator.StringToHash("Die");
    void Start()
    {
        enemyFire = GetComponent<EnemyFire>();
        animator = GetComponent<Animator>();
        playerTr = GameObject.FindGameObjectWithTag(tagPlayer).transform;
        moveAgent = GetComponent<MoveAgent>();
        agent = GetComponent<NavMeshAgent>();
        tr = GetComponent<Transform>();
    }

    void Update()
    {   if (GameManager.instance != null && GameManager.instance.isGameover) return;

        if (!isDie)
        {
            //플레이어와 적 간 거리에 따른 상태 변화, 상태에 따른 함수 호출
            dist = Vector3.Distance(playerTr.position, tr.position);
            if (dist <= attackDist)
            {
                state = State.attack;
            }
            else if (dist <= traceDist)
            {
                state = State.trace;
            }
            else
            {
                state = State.patrol;
            }


            switch (state)
            {
                case State.idle:
                    Idle();
                    break;
                case State.attack:
                    Attack();
                    break;
                case State.trace:
                    Trace();
                    break;
                case State.patrol:
                    Patrol();
                    break;
            }
        }
    }

    private void Idle()
    {
        agent.isStopped = true;
        enemyFire.isFire = false;
    }

    private void Attack()
    {
        agent.isStopped = true;
        enemyFire.isFire = true;
        animator.SetBool(hashIsMove, false);
        animator.SetTrigger(hashFireTrigger);
    }

    private void Trace()
    {
        agent.isStopped = false;
        enemyFire.isFire = false;
        animator.SetBool(hashIsMove, true);
        animator.SetFloat(hashMoveSpeed, agent.speed);
        moveAgent.TraceTarget();
    }

    private void Patrol()
    {
        agent.isStopped = false;
        enemyFire.isFire = false;
        animator.SetBool(hashIsMove, true);
        animator.SetFloat(hashMoveSpeed, agent.speed);
        moveAgent.Patrol();
    }

    public void Die()
    {
        agent.isStopped = true;
        enemyFire.isFire = false;
        animator.SetTrigger(hashDie);
        int dieIdx = Random.Range(0, 3);
        animator.SetInteger(hashDieIdx, dieIdx);
        isDie = true;
    }
}
