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

    void FixedUpdate() //움직이는 건 Fixed Update로 해준다.
    {
        //게임 오버인 경우 빠져나간다.
        if (GameManager.Instance != null && GameManager.Instance.isGameOver)
        {
            return;
        }
        tr.Translate(Vector3.forward * input.move * moveSpeed * Time.deltaTime);
        tr.Rotate(Vector3.up * input.rotate * rotSpeed * Time.deltaTime);
    }
}
