using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))] //NavMeshAgent 필수! 누군가 삭제하려고 하면 경고를 띄운다.
public class MoveAgent : MonoBehaviour
{
    [SerializeField] private Transform playerTr;
    
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private List<Transform> patrolLists;
    [SerializeField] private Transform tr;
    

    private float damping;
    [SerializeField] float walkSpeed = 3f;
    [SerializeField] float runSpeed = 6.0f;
    private Vector3 _traceTarget;
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set
        {
            _traceTarget = value;
            damping = 7f;
            agent.speed = runSpeed;
            TraceTarget();
        }
        
    }
    void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale) return;
        agent.destination = pos;
        agent.isStopped = false;
    }
    Vector3 currentDestination = Vector3.zero;
    int nextIdx = 0;
    


    private readonly string tagPlayer = "Player";
    

    private bool _isPatrolling = false;
    private bool isPatrolling
    {
        get { return _isPatrolling; }
        set
        {
            _isPatrolling = true;
            if (isPatrolling)
            {
                damping = 5f;
                agent.speed = walkSpeed;
                Patrol();
            }
        }
    }
    void Start()
    {
        tr = transform;
        playerTr = GameObject.FindGameObjectWithTag(tagPlayer).transform;
        agent = GetComponent<NavMeshAgent>();
        
        var patrolPoints = GameObject.Find("PatrolPoints");
        if(patrolPoints != null)
        {
            patrolPoints.transform.GetComponentsInChildren<Transform>(patrolLists);
            patrolLists.RemoveAt(0);
        }
        agent.speed = walkSpeed;
        nextIdx = Random.Range(0, patrolLists.Count);
        currentDestination = patrolLists[nextIdx].position;
        isPatrolling = true;
        
    }
    public void Stop()
    {
        GetComponent<NavMeshAgent>().isStopped = true;
    }

    void Update()
    {
        
       if(agent.isStopped == false)
        {

            Quaternion rot;
            if (agent.desiredVelocity != Vector3.zero)
                rot = Quaternion.LookRotation(agent.desiredVelocity); //현재 이동 방향을 바라봄
            else
                rot = Quaternion.identity;
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, damping * Time.deltaTime);
        }
        //if (!isPatrolling) return;
        //if(agent.velocity.sqrMagnitude)

    }
    public void TraceTarget()
    {
        agent.destination = playerTr.position;
        agent.speed = runSpeed;
        tr.LookAt(playerTr);
    }
    public void Patrol()
    {
        agent.destination = currentDestination;
        if (Vector3.Distance(agent.transform.position, currentDestination) < 3.0f)
        {
            nextIdx = Random.Range(0, patrolLists.Count);
            // nextIdx++;
            //if (nextIdx == patrolLists.Count) nextIdx = 0;
            currentDestination = patrolLists[nextIdx].position;
        }
    }
}
