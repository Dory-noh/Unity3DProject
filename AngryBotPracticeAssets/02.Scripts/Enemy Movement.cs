using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;

public class EnemyMovement : MonoBehaviour
{
    enum State
    {
        PATROL,
        TRACE,
        ATTACK,
        DIE
    }

    [SerializeField] Transform player;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform firePos;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] State enemyState;
    [SerializeField] Animator animator;
    [SerializeField] List<Transform> patrolPoints;
    [SerializeField] Image hpBar;
    [SerializeField] GameObject CanvasHp;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip clip;
    [SerializeField] float attackRange = 3f;
    [SerializeField] float traceRange = 10f;
    [SerializeField] float walkSpeed = 1.5f;
    [SerializeField] float runSpeed = 3.5f;
    [SerializeField] int patrolIdx = 0;
    bool isFire = false;
    private float hp = 0f;
    private readonly float MaxHp = 100f;

    [SerializeField] readonly int hashIsMove = Animator.StringToHash("IsMove");
    [SerializeField] readonly int hashForwardSpeed = Animator.StringToHash("ForwardSpeed");
    [SerializeField] readonly int hashFire = Animator.StringToHash("Fire");
    [SerializeField] readonly int hashDie = Animator.StringToHash("Die");
    [SerializeField] readonly int hashDieIdx = Animator.StringToHash("DieIdx");

    void Start()
    {
        firePos = transform.GetChild(3).GetChild(0).GetChild(0).transform;
        bulletPrefab = Resources.Load<GameObject>("E_Bullet");
        muzzleFlash = firePos.GetChild(0).GetComponent<ParticleSystem>();
        source = firePos.GetComponent<AudioSource>();
        clip = Resources.Load<AudioClip>("gun");
        player = GameObject.Find("Player").GetComponent<CharacterController>().transform;
        agent = GetComponent<NavMeshAgent>();
        CanvasHp = transform.GetChild(4).gameObject;
        hpBar = transform.GetChild(4).GetChild(1).GetComponent<Image>();
        animator = GetComponent<Animator>();
        GameObject.Find("Enemy Spawn Points").transform.GetComponentsInChildren<Transform>(patrolPoints);
        patrolPoints.RemoveAt(0);
        hp = MaxHp;
        hpBar.fillAmount = (float)hp / MaxHp;
        hpBar.color = Color.green;
        enemyState = State.PATROL;
    }


    void Update()
    {
        if (enemyState == State.DIE || (GameManager.Instance.isGameover == true && GameManager.Instance != null)) return;
        Debug.DrawRay(firePos.position, transform.forward, Color.red);
        CheckState();
        ActState();
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((agent.destination - transform.position).normalized), Time.deltaTime * 100f);
        CanvasHp.transform.rotation = transform.rotation;
        
    }

    private void ActState()
    {
        switch (enemyState)
        {
            case State.PATROL:
                agent.isStopped = false;
                agent.speed = walkSpeed;
                animator.SetBool(hashIsMove, true);
                animator.SetFloat(hashForwardSpeed, agent.speed);
                Patrol();
                break;
            case State.TRACE:
                agent.isStopped = false;
                agent.speed = runSpeed;
                animator.SetBool(hashIsMove, true);
                animator.SetFloat(hashForwardSpeed, agent.speed);
                break;
            case State.ATTACK:
                agent.isStopped = true;
                animator.SetBool(hashIsMove, false);
                animator.SetFloat(hashForwardSpeed, 0f);
                StartCoroutine(FireBullet());
                break;
        }
    }

    private void Patrol()
    {
        agent.destination = patrolPoints[patrolIdx].position;
        float distance = Vector3.Distance(transform.position, patrolPoints[patrolIdx].position);
        if (!agent.pathPending && agent.remainingDistance < 1f)
        {
            patrolIdx = (patrolIdx + 1) % patrolPoints.Count;
        }
    }

    private void CheckState()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        
        if (distance <= attackRange)
        {
            agent.destination = player.position;
            enemyState = State.ATTACK;
        }
        else if (distance <= traceRange)
        {
            agent.destination = player.position;
            enemyState = State.TRACE;
        }
        else
        {
            enemyState = State.PATROL;
        }
    }
    IEnumerator FireBullet()
    {
        if(isFire == false)
        {
            isFire = true;
            animator.SetTrigger(hashFire);
            if (muzzleFlash.isPlaying == false) muzzleFlash.Play();
                source.PlayOneShot(clip, 1f);
            Instantiate(bulletPrefab, firePos.position, firePos.rotation);
            yield return new WaitForSeconds(0.7f);
            isFire = false;
        }
        
    }
    public void OnDamaged()
    {
        if (enemyState == State.DIE) return;
        Debug.Log(gameObject.name + " " + hp);
        hp -= 10f;
        hp = Mathf.Clamp(hp, 0, MaxHp);
        ChangeHpBar();
        if (hp <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private void ChangeHpBar()
    {
        
            hpBar.fillAmount = hp / MaxHp;
        if (hpBar.fillAmount <= 0.3f)
        {
            hpBar.color = Color.red;
        }
        else if (hpBar.fillAmount <= 0.5f)
        {
            hpBar.color = Color.yellow;
        }
        else
        {
            hpBar.color = Color.green;
        }
    }

    IEnumerator Die()
    {
        enemyState = State.DIE;
        animator.SetBool(hashIsMove, false);
        animator.SetTrigger(hashDie);
        animator.SetInteger(hashDieIdx, Random.Range(0, 3));
        agent.enabled = false;
        yield return new WaitForSeconds(3f);
        agent.enabled = true;
        hp = MaxHp;
        hpBar.fillAmount = (float)hp / MaxHp;
        hpBar.color = Color.green;
        agent.isStopped = true;
        gameObject.SetActive(false);
        enemyState = State.PATROL; 
        
    }
    public void OnDie()
    {
        hp = MaxHp;
        hpBar.fillAmount = (float)hp / MaxHp;
        hpBar.color = Color.green;
        agent.isStopped = true;
        agent.speed = 0f;
        animator.SetBool(hashIsMove,false);
        animator.SetFloat(hashForwardSpeed, 0f);
        gameObject.SetActive(false);
        enemyState = State.PATROL;
        agent.enabled = true;
    }
}

