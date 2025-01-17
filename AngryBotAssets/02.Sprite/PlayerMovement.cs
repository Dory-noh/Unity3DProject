using Cinemachine;
using Photon.Pun;
using Photon.Realtime; //��Ʈ��ũ ���̺귯��
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMovement : MonoBehaviourPunCallbacks, IPunObservable
                            //�̷��� ���� �� photonView��밡��
{
    private CharacterController controller;
    private new Transform transform;
    private Animator animator;
    private new Camera camera;
    private PlayerInput input;
    private PhotonView pv = null;
    private CinemachineVirtualCamera followCamera;

    private Vector3 receivePos; //Remote �ٸ� ��Ʈ��ũ ������ �����Ӱ� ����
    private Quaternion receiveRot;//Remote �ٸ� ��Ʈ��ũ ������ ȸ���� ����
    public float damping = 10f;

    //������ Plane�� RayCasting�ϱ� ���� ����. �ٴڿ� Ray�� ���� �����ϵ��� �������
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
        //������ �ٴ��� ���ΰ��� ��ġ�� �������� �����Ѵ�.
        plane = new Plane(transform.up, transform.position);
        pv = GetComponent<PhotonView>();
        followCamera = GameObject.Find("FollowCamera").GetComponent<CinemachineVirtualCamera>();
        if (pv.IsMine)
        {
            followCamera.Follow = transform;
            followCamera.LookAt = transform;
        }
        //receive�� �ʱ�ȭ ���־�� �Ѵ�.
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
            //Slerp: ��� ���� Lerp: ���� ���� 
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
        controller.SimpleMove(moveDir * moveSpeed); //simpleMove�ȿ� Time.deltatime ����ռ�
        float forward = Vector3.Dot(moveDir, transform.forward);
        float strafe = Vector3.Dot(moveDir, transform.right);
        animator.SetFloat(hashPosX, strafe);
        animator.SetFloat(hashPosY, forward);
    }

    void Turn()
    {
        ray = camera.ScreenPointToRay(Input.mousePosition);
        float enter = 0.0f;
        //������ �ٴڿ� ���̸� �߻��ؼ� �浹�� ������ �Ÿ��� enter ������ ��ȯ�Ѵ�.
        plane.Raycast(ray, out enter);
        //������ �ٴڿ� ���̰� �浹�� ��ǥ�� ����
        hitPoint = ray.GetPoint(enter);
        //ȸ���ؾ� �� ������ ���͸� ���
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
