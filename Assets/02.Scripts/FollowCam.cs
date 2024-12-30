using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float Height = 15; //ī�޶� ����
    [SerializeField] float Distance = 20; //ī�޶� �Ÿ�
    [SerializeField] float moveDamping = 10f; //ī�޶� ������ ��� ��ȭ�Ͽ��� ������/�ε巴�� �Ѵ�.
    [SerializeField] float rotateDamping = 15f;
    [SerializeField] Transform tr;
    public float targetOffset = 2.0f;

     void Start()
    {
        tr = GetComponent<Transform>();
    }

    void LateUpdate()//�÷��̾ �̵��� �Ŀ� ī�޶� ���� ���� �ϴϱ� LateUpdate ���(LateUpdate�� Update, FixedUpdate ���� ������ ����ȴ�.)
    {
        var camPos = target.position - (target.forward * Distance) //Ÿ�� �ڿ� Distance��ŭ ��������
                                     + (target.up * Height); //Ÿ�� ���� Height��ŭ �ö󰡼�
        tr.position = Vector3.Slerp(tr.position, camPos, Time.deltaTime * moveDamping); //A���� B�� ������ �� moveDamping(*Time.deltaTime)��ŭ �ε巴�� �����δ�.
        tr.rotation = Quaternion.Slerp(tr.rotation, target.rotation, Time.deltaTime * rotateDamping);//A���� B�� rotateDamping(*Time.deltaTime)��ŭ �ε巴�� ȸ���Ѵ�.
        tr.LookAt(target.position+(target.up*targetOffset)); //��ġ�� ���ϰų� ��, �Ÿ��� ���Ѵ�.
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(target.position+(target.up*targetOffset),0.1f);
        Gizmos.DrawLine(target.position + (target.up * targetOffset), tr.position);
    }
}
