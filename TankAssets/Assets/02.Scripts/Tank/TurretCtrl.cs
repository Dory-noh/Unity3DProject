using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretCtrl : MonoBehaviour
{
    private Transform tr;
    private float rotSpeed = 5f;
    RaycastHit hit;
    Ray ray;

    void Start()
    {
        tr = transform;    
    }

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //ī�޶� ��ũ������ ���콺 ������ �������� ������ �߻�
        Debug.DrawRay(ray.origin, ray.direction*100f, Color.red); //Scene������ ����
        if (Physics.Raycast(ray, out hit, 60f, 1<<6)) //out: ��� ���� �Ű����� //terrain�� ����-����, ����, �� ���� ���� �� ��
        {
            //���� ������ ���� ��ǥ�� ��ũ�� ���� ��ǥ�� �ٲ۴�.
            Vector3 relative = tr.InverseTransformDirection(hit.point).normalized;
                                //���� ray�� ���� ������ ��ũ�� �´� ���� ��ǥ�� �ٲ۴�.
            float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg; //pi*2/360 
                                                                               //��ź��Ʈ�Լ�(���� ����.x, ���� ����.z - y�� �����̴ϱ�� ) * pi*2/360 
            tr.Rotate(0f, angle * Time.deltaTime * rotSpeed, 0f); //y�����θ� ȸ���Ѵ�.
        }
        //else
        //{
        //    Quaternion resetAngle = Quaternion.Euler(0, 0, 0);
        //    tr.rotation = Quaternion.Slerp(tr.rotation, resetAngle, Time.deltaTime * rotSpeed);
        //}
    }
}
