using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PatrolPoints : MonoBehaviour
{
    [SerializeField] private List<Transform> patrolPoints;
    [SerializeField] Color lineColor = Color.yellow;
    void Start()
    {
        
    }
    private void OnDrawGizmos()
    {
        //patrolPoints리스트에 Patrol 위치 Componetn 넣기
        transform.GetComponentsInChildren<Transform>(patrolPoints);
        patrolPoints.RemoveAt(0);


        for(int i = 0; i < patrolPoints.Count; i++) {
            Vector3 currentNode = patrolPoints[i].position;
            Vector3 PreviousNode = Vector3.zero;
            if (i == 0) PreviousNode = patrolPoints[patrolPoints.Count - 1].position;
            else if (i > 0)
            {
                PreviousNode = patrolPoints[i - 1].position;
            }
            Gizmos.color = lineColor;
            Gizmos.DrawSphere(currentNode, 0.2f);
            Gizmos.DrawLine(currentNode, PreviousNode);
        }
    }
    void Update()
    {
        
    }
}
