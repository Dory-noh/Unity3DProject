using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonCtrl : MonoBehaviour
{
    [SerializeField] TankInput input;
    public float rotSpeed = 1000f;
    public float upAngle = -30f;
    public float downAngle = 5f; //제한 값
    public float currentRotate = 0f;
    Transform tr;
    void Start()
    {
        input = GetComponentInParent<TankInput>();
        tr = transform;
    }

    void FixedUpdate()
    {
        var Wheel = -Input.GetAxis(input.m_scrollWheel); //휠을 내리면 Cannon이 올라가도록 한다.
        float angle = Time.deltaTime * rotSpeed * Wheel;
        if(Wheel <= 0.01f) //휠의 값이 양수일 때
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
        else //휠의 값이 음수일 때
        {
            currentRotate += angle;
            if(currentRotate < downAngle)
            {
                tr.Rotate(angle, 0f, 0f);
            }
            else
            {
                currentRotate = downAngle;//더 이상 내려가지 않음
                tr.Rotate(angle, 0f, 0f);
            }
        }
    }
}
