using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonCtrl : MonoBehaviour
{
    [SerializeField] TankInput input;
    public float rotSpeed = 1000f;
    public float upAngle = -30f;
    public float downAngle = 5f; //���� ��
    public float currentRotate = 0f;
    Transform tr;
    void Start()
    {
        input = GetComponentInParent<TankInput>();
        tr = transform;
    }

    void FixedUpdate()
    {
        var Wheel = -Input.GetAxis(input.m_scrollWheel); //���� ������ Cannon�� �ö󰡵��� �Ѵ�.
        float angle = Time.deltaTime * rotSpeed * Wheel;
        if(Wheel <= 0.01f) //���� ���� ����� ��
        {
            currentRotate += angle;
            if(currentRotate > upAngle)
            {
                tr.Rotate(angle, 0f, 0f);
            }
            else
            {
                currentRotate = upAngle;
                tr.Rotate(angle, 0f, 0f);
            }
        }
        else //���� ���� ������ ��
        {
            currentRotate += angle;
            if(currentRotate < downAngle)
            {
                tr.Rotate(angle, 0f, 0f);
            }
            else
            {
                currentRotate = downAngle;//�� �̻� �������� ����
                tr.Rotate(angle, 0f, 0f);
            }
        }
    }
}
