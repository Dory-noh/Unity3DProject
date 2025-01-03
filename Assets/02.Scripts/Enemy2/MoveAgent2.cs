using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MoveAgent2 : MonoBehaviour
{
    [SerializeField] List<Transform> patrolPoints = new List<Transform>();
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform playerTr;
    [SerializeField] Transform tr;

    int currentIdx = 0;
    float patrolPointDistance = 3.0f;
    
    float damping = 8f;

    private readonly string playerTag = "Player";

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerTr = GameObject.FindGameObjectWithTag(playerTag).transform;
        tr = transform;
        var points = GameObject.Find("PatrolPoints");
        if (points != null)
        {
            points.transform.GetComponentsInChildren<Transform>(patrolPoints);
            patrolPoints.RemoveAt(0);
        }
    }

    private void Update()
    {
        if (agent.isStopped == false)
        {
            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, damping * Time.deltaTime);
        }
    }


    public void Patrol()
    {
        agent.destination = patrolPoints[currentIdx].position;
        if(Vector3.Distance(transform.position, patrolPoints[currentIdx].position) <= patrolPointDistance)
        {
            currentIdx++;
            if (currentIdx > patrolPoints.Count) currentIdx = 0;
        }
        
    }
}
