using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum State
{
    idle,
    patrol,
    trace,
    attack,
    die
}

public class Enemy2AI : MonoBehaviour
{
    [SerializeField] State state = State.idle;
    [SerializeField] Transform playerTr;
    [SerializeField] Transform tr;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] MoveAgent2 moveAgent;
    [SerializeField] Animator animator;
    [SerializeField] Enemy2Fire enemyFire;
    [SerializeField] private CapsuleCollider capCol;
    [SerializeField] private Rigidbody rb;

    private float dist;

    private float attactDistance = 4.5f;
    private float traceDistance = 13f;

    float patrolSpeed = 4f;
    float traceSpeed = 8f;

    public bool isDie = false;
    private readonly string playerTag = "Player";
    private readonly int hashIsStop = Animator.StringToHash("IsStop");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashDieIdx = Animator.StringToHash("DieIdx");
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");
    private readonly int hashOffeset = Animator.StringToHash("Offset");
    private readonly int hashWalkSpeed = Animator.StringToHash("WalkSpeed");

    void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag(playerTag).transform;
        
        tr = transform;
        capCol = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();

        
        enemyFire.isFire = false;
    }
    private void OnEnable()
    {
        enemyFire = GetComponent<Enemy2Fire>();
        moveAgent = GetComponent<MoveAgent2>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        state = State.idle;
        isDie = false;
        animator.SetFloat(hashOffeset, Random.Range(1f, 3f));
        animator.SetFloat(hashWalkSpeed, Random.Range(1f, 2f));
        PlayerDamage.onDeath += OnPlayerDie;
       BarrelCtrl.OnEnemyDie += Die;
    }
    void Update()
    {
        if (GameManager.instance.isGameover) return;
        if (!isDie)
        {
            //�Ÿ��� ���� ���� ǥ�� idle-attack-trace-patrol
            dist = Vector3.Distance(transform.position, playerTr.position);
            if (dist <= attactDistance)
            {
                state = State.attack;
            }
            else if (dist <= traceDistance)
            {
                state = State.trace;
            }
            else
            {
                state = State.patrol;
            }
            //���¿� ���� �޼��� ȣ��
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
    public void Die()
    {
        if (isDie == true) return;
        isDie = true;
        if (gameObject.activeSelf)
        {
            //Debug.Log($"{gameObject.name} ���");
            GameManager.instance.KillScoreCount();
        }
        if (agent.isActiveAndEnabled) agent.isStopped = true;
        if(enemyFire!=null) enemyFire.isFire = false;
        animator.SetTrigger(hashDie);
        animator.SetInteger(hashDieIdx, Random.Range(0, 3));
        //StopAllCoroutines();
        //gameObject.SetActive(false);
        
        if (gameObject.activeSelf)
        StartCoroutine(PoolPush());
        
    }

    IEnumerator PoolPush()
    {
        yield return new WaitForSeconds(3.0f);
        capCol.enabled = true;
        rb.isKinematic = false;
        gameObject.SetActive(false);
        state = State.patrol;
    }

    private void Patrol()
    {
        agent.isStopped = false;
        animator.SetBool(hashIsStop, false);
        animator.SetFloat(hashSpeed, patrolSpeed);
        enemyFire.isFire = false;
        moveAgent.Patrol();
    }

    private void Trace()
    {
        agent.isStopped = false;
        animator.SetBool(hashIsStop, false);
        animator.SetFloat(hashSpeed, traceSpeed);
        enemyFire.isFire = false;
        agent.destination = playerTr.position;
    }

    private void Attack()
    {
        agent.isStopped = true;
        enemyFire.isFire = true;
        animator.SetBool(hashIsStop, true);
    }

    private void Idle()
    {
        agent.isStopped = true;
        enemyFire.isFire = false;
        animator.SetBool(hashIsStop, true);
    }

    public void OnPlayerDie()
    {
        moveAgent.Stop();
        enemyFire.isFire = false;
        StopAllCoroutines();
        animator.SetTrigger(hashPlayerDie);
    }
    private void OnDisable()
    {
        PlayerDamage.onDeath -= OnPlayerDie;
    }
}
