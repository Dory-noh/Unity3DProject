using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput_m : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] PlayerMovement playerMovement;

    private readonly int hashPosX = Animator.StringToHash("PosX");
    private readonly int hashPosY = Animator.StringToHash("PosY");
    private readonly int hashDie = Animator.StringToHash("Die");
    

    public float PosX, PosY, RotX, RotY = 0;
    public bool isRun = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void OnEnable()
    {
        PosX = 0f;
        PosY = 0;
    }
    void Update()
    {
        if (GameManager.Instance.isGameover == true && GameManager.Instance != null) return;
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 dir = ctx.ReadValue<Vector2>();
        PosX = dir.x;
        PosY = dir.y;
        //Debug.Log($"이동 {dir.x} {dir.y}");
        animator.SetFloat(hashPosX, PosX);
        animator.SetFloat(hashPosY, PosY);
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        Vector2 dir = ctx.ReadValue<Vector2>();
        RotX = dir.x;
        RotY = dir.y;
        //Debug.Log($"회전 {dir.x} {dir.y}");
    }

    public void OnFire(InputAction.CallbackContext ctx)
    {
        if(ctx.phase == InputActionPhase.Started)
            playerMovement.FireBullet();
    }


    public void OnRun(InputAction.CallbackContext ctx)
    {
        if(ctx.phase == InputActionPhase.Started)
        {
            isRun = true;
            //Debug.Log("달리기 시작");
        }
        else if(ctx.phase == InputActionPhase.Canceled)
        {
            isRun = false;
            //Debug.Log("달리기 멈춤");
        }
    }

    public void OnDie()
    {
        Debug.Log("플레이어 죽음");
        animator.SetFloat(hashPosX, 0f);
        animator.SetFloat(hashPosY, 0f);
        animator.SetTrigger(hashDie);
        
    }
}
