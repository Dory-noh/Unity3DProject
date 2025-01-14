using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TankMovement : MonoBehaviourPun, IPunObservable
{
    [SerializeField] TankInput input;
    [SerializeField] Rigidbody rb;
    private Transform tr;
    public float moveSpeed = 12f;
    public float rotSpeed = 90f;
    //private PhotonView pv = null;
    private Vector3 curPos = Vector3.zero; //다른 유저의 위치값을 받는 변수
    private Quaternion curRot = Quaternion.identity; //다른 유저의 회전값을 받는 변수
    void Awake()
    {
        tr = transform;
        input = GetComponent<TankInput>();
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0f, -0.5f, 0f); //탱크 무게 중심 잡기
        photonView.Synchronization = ViewSynchronization.Unreliable;
        photonView.ObservedComponents[0] = this;
        //pv = GetComponent<PhotonView>();
        ////통신 방식은 UDP
        //pv.Synchronization = ViewSynchronization.UnreliableOnChange;
        //pv.ObservedComponents[0] = this;
        ////photon view는 현재 스크립트를 관찰하여, 스크립트에 있는 데이터를 네트워크를 통해 동기화한다.
        //위 내용은 MonoBehaviour'Pun'을 사용하지 않을 때 사용
        curPos = tr.position;
        curRot = tr.rotation;
    }

    void FixedUpdate() //움직이는 건 Fixed Update로 해준다.
    {
        //게임 오버인 경우 빠져나간다.
        if (GameManager.Instance != null && GameManager.Instance.isGameOver)
        {
            return;
        }
        if (photonView.IsMine) //photonView가 본인의 것이라면
        {
            tr.Translate(Vector3.forward * input.move * moveSpeed * Time.deltaTime);
            tr.Rotate(Vector3.up * input.rotate * rotSpeed * Time.deltaTime);
        }
        else //다른 유저라면
        {
                            //본인의 움직임과 다른 네트워크의 움직임을 부드럽게 보간함수로 움직인다.
            tr.position = Vector3.Lerp(tr.position, curPos, Time.deltaTime * 3f);
            tr.rotation = Quaternion.Slerp(tr.rotation, curRot, Time.deltaTime * 3f);
        }
    }

    //본인 탱크의 움직임과 다른 네트워크 유저 탱크의 움직임이 서로 보이게 하기 위해 송수신하는 메서드
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) //stream: 통신
        {
            //나의 이동과 회전을 다른 네트워크 유저에게 송신한다.
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
        }
        else if (stream.IsReading)
        {
            //다른 네트워크 유저의 움직임-이동과 회전-을 수신한다.
            curPos = (Vector3)stream.ReceiveNext();
            curRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
