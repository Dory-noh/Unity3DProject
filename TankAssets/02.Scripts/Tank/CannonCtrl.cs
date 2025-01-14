using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CannonCtrl : MonoBehaviourPun, IPunObservable
{
    [SerializeField] TankInput input;
    public float rotSpeed = 1000f;
    public float upAngle = -30f;
    public float downAngle = 5f; //���� ��
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
            var Wheel = -Input.GetAxis(input.m_scrollWheel); //���� ������ Cannon�� �ö󰡵��� �Ѵ�.
            float angle = Time.deltaTime * rotSpeed * Wheel;
            if (Wheel <= 0.01f) //���� ���� ����� ��
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
            else //���� ���� ������ ��
            {
                currentRotate += angle;
                if (currentRotate < downAngle)
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
        else //�ٸ� ��Ʈ��ũ ������
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
