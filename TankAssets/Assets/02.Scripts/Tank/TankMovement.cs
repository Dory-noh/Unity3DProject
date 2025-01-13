using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    [SerializeField] TankInput input;
    private Transform tr;
    public float moveSpeed = 12f;
    public float rotSpeed = 90f; 

    void Start()
    {
        input = GetComponent<TankInput>();
        tr = transform;

    }

    void FixedUpdate() //�����̴� �� Fixed Update�� ���ش�.
    {
        //���� ������ ��� ����������.
        if (GameManager.Instance != null && GameManager.Instance.isGameOver)
        {
            return;
        }
        tr.Translate(Vector3.forward * input.move * moveSpeed * Time.deltaTime);
        tr.Rotate(Vector3.up * input.rotate * rotSpeed * Time.deltaTime);
    }
}
