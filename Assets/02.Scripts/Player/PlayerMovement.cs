using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    [SerializeField] Transform tr;
    [SerializeField] Animator animator;

    private readonly int hashPosX = Animator.StringToHash("PosX");
    private readonly int hashPosY = Animator.StringToHash("PosY");
    private readonly int hashIsRun = Animator.StringToHash("IsRun");

    public float moveSpeed = 6f;
    public float turnSpeed = 120f;
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        tr = transform;
    }

    void FixedUpdate()
    {

        MoveAndRotate();
        Sprint();
    }

    private void Sprint()
    {
        if (playerInput.sprint)
        {
            moveSpeed = 10f;
        }
        else
        {
            moveSpeed = 6f;
        }
        animator.SetBool(hashIsRun, playerInput.sprint);
    }

    private void MoveAndRotate()
    {
        Vector3 moveDir = Vector3.right * playerInput.h + Vector3.forward * playerInput.v;
        tr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime);
        tr.Rotate(Vector3.up * playerInput.r * turnSpeed * Time.deltaTime);
        animator.SetFloat(hashPosX, moveDir.x, 0.01f, Time.deltaTime);
        animator.SetFloat(hashPosY, moveDir.z, 0.01f, Time.deltaTime);
    }
}
