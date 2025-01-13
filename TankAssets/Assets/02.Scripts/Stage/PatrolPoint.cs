using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPoint : MonoBehaviour
{
    public Color lineColor = Color.white;
    [SerializeField] List<Transform> patrolList;
    public float _radius = 0.6f;
    private void OnDrawGizmos()
    {
        //Transform[] patrolTransform = GetComponentsInChildren<Transform>();
        patrolList = new List<Transform>(); //���� �Ҵ�
        //for(int i = 0; i < patrolTransform.Length; i++)
        //{
        //    if(patrolTransform[i] != transform) patrolList.Add(patrolTransform[i]);
        //}
        transform.GetComponentsInChildren<Transform>(patrolList);
        patrolList.RemoveAt(0);
        for(int i = 0; i < patrolList.Count; i++)
        {
            Vector3 currentPos = patrolList[i].position;
            Vector3 prevPos = Vector3.zero;
            if(i > 0)
            {
                prevPos = patrolList[i-1].position; //�� �� ����Ʈ ��ġ�� prevPos�� ����
            }
            else if(i == 0 && patrolList.Count > 1) //PatrolPoint�� 2�� �̻��� �� ���� ��� ǥ��
            {
                prevPos = patrolList[patrolList.Count-1].position;
            }
            Gizmos.DrawLine(prevPos, currentPos);
            Gizmos.DrawSphere(currentPos, _radius);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
