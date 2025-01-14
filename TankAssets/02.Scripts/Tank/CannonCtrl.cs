using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CannonCtrl : MonoBehaviourPun, IPunObservable
{
    [SerializeField] TankInput input;
    public float rotSpeed = 1000f;
    public float upAngle = -30f;
    public float downAngle = 5f; //제한 값
    public float currentRotate = 0f;
    Transform tr;
    private Quaternion curRot;

    void Start()
    {
        tr = transform;
        input = GetComponentInParent<TankInput>();
        curRot = tr.localRotation;
        photonView.Synchronization = ViewSynchronization.Unreliable;
        photonView.ObservedComponents[0] = this;
    }

    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            var Wheel = -Input.GetAxis(input.m_scrollWheel); //휠을 내리면 Cannon이 올라가도록 한다.
            float angle = Time.deltaTime * rotSpeed * Wheel;
            if (Wheel <= 0.01f) //휠의 값이 양수일 때
            {
                currentRotate += angle;
                if (currentRotate > upAngle)
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
                if (currentRotate < downAngle)
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
        else //다른 네트워크 유저면
        {
            tr.localRotation = Quaternion.Slerp(tr.localRotation, curRot, Time.deltaTime * 3f);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(tr.localRotation);
        }
        else if (stream.IsReading)
        {
            curRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
