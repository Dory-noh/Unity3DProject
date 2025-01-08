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

    int nextIdx = 0;
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
        nextIdx = Random.Range(0, patrolPoints.Count);
    }

    private void Update()
    {
        if (agent.isStopped == false)
        {
            Quaternion rot;
            if (agent.desiredVelocity != Vector3.zero)
            {
                rot = Quaternion.LookRotation(agent.desiredVelocity);
            }
            else
            {
                rot = Quaternion.identity;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, damping * Time.deltaTime);
        }
    }


    public void Patrol()
    {
        agent.destination = patrolPoints[nextIdx].position;
        if(Vector3.Distance(transform.position, patrolPoints[nextIdx].position) <= patrolPointDistance)
        {
            //nextIdx++;
            //if (nextIdx > patrolPoints.Count) nextIdx = 0;
            nextIdx = Random.Range(0, patrolPoints.Count);
            //랜덤하게 이동하도록 함.
        }

    }
    public void Stop()
    {
        GetComponent<NavMeshAgent>().isStopped = true;

    }
}
