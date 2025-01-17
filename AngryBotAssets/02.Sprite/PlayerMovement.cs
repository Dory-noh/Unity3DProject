using Cinemachine;
using Photon.Pun;
using Photon.Realtime; //네트워크 라이브러리
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMovement : MonoBehaviourPunCallbacks, IPunObservable
                            //이렇게 썼을 때 photonView사용가능
{
    private CharacterController controller;
    private new Transform transform;
    private Animator animator;
    private new Camera camera;
    private PlayerInput input;
    private PhotonView pv = null;
    private CinemachineVirtualCamera followCamera;

    private Vector3 receivePos; //Remote 다른 네트워크 유저의 움직임값 받음
    private Quaternion receiveRot;//Remote 다른 네트워크 유저의 회전값 받음
    public float damping = 10f;

    //가상의 Plane에 RayCasting하기 위한 변수. 바닥에 Ray를 쏴서 감지하도록 만들거임
    private Plane plane;
    private Ray ray;
    private Vector3 hitPoint;

    public float moveSpeed = 10f;
    private readonly int hashPosX = Animator.StringToHash("Pos X");
    private readonly int hashPosY = Animator.StringToHash("Pos Y");
    void Start()
    {
        input = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController>();
        transform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        camera = Camera.main;
        //가상의 바닥을 주인공의 위치를 기준으로 생성한다.
        plane = new Plane(transform.up, transform.position);
        pv = GetComponent<PhotonView>();
        followCamera = GameObject.Find("FollowCamera").GetComponent<CinemachineVirtualCamera>();
        if (pv.IsMine)
        {
            followCamera.Follow = transform;
            followCamera.LookAt = transform;
        }
        //receive값 초기화 해주어야 한다.
        receivePos = transform.position;
        receiveRot = transform.rotation;
    }

    void Update()
    {
        if (pv.IsMine)
        {
            Move();
            Turn();
        }
        else
        {
            //Slerp: 곡면 보간 Lerp: 선형 보간 
            transform.position = Vector3.Slerp(transform.position, receivePos, Time.deltaTime * damping);
            transform.rotation = Quaternion.Slerp(transform.rotation, receiveRot, Time.deltaTime * damping);
        }
    }

    private void Move()
    {
        Vector3 cameraForward = camera.transform.forward;
        Vector3 cameraRight = camera.transform.right;
        cameraForward.y = 0.0f;
        cameraRight.y = 0.0f;
        Vector3 moveDir = transform.forward * input.v + transform.right * input.h;
        moveDir.Set(moveDir.x, 0, moveDir.z);
        controller.SimpleMove(moveDir * moveSpeed); //simpleMove안에 Time.deltatime 들어잇숨
        float forward = Vector3.Dot(moveDir, transform.forward);
        float strafe = Vector3.Dot(moveDir, transform.right);
        animator.SetFloat(hashPosX, strafe);
        animator.SetFloat(hashPosY, forward);
    }

    void Turn()
    {
        ray = camera.ScreenPointToRay(Input.mousePosition);
        float enter = 0.0f;
        //가상의 바닥에 레이를 발사해서 충돌한 지점의 거리를 enter 변수로 변환한다.
        plane.Raycast(ray, out enter);
        //가상의 바닥에 레이가 충돌한 좌표값 추출
        hitPoint = ray.GetPoint(enter);
        //회전해야 할 방향의 벡터를 계산
        Vector3 lookDir = hitPoint - transform.position;
        lookDir.y = 0.0f;
        transform.localRotation = Quaternion.LookRotation(lookDir);
        //float r = Input.GetAxis("Mouse X");
        //transform.Rotate(Vector3.up * r * Time.deltaTime * 90f);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        if (stream.IsReading)
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
