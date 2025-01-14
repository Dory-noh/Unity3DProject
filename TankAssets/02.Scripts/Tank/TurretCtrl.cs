using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretCtrl : MonoBehaviourPun, IPunObservable
{
    private Transform tr;
    private float rotSpeed = 5f;
    RaycastHit hit;
    Ray ray;
    private Quaternion curRot = Quaternion.identity;
    void Start()
    {
        tr = transform;
        photonView.Synchronization = ViewSynchronization.Unreliable;
        photonView.ObservedComponents[0] = this; //����?
        curRot = tr.localRotation; //�θ�(��ũ)�� �ٸ� �������� ������ ���� �� local��ǥ ���
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
        void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;
        if (photonView.IsMine)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //ī�޶� ��ũ������ ���콺 ������ �������� ������ �߻�
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red); //Scene������ ����
            if (Physics.Raycast(ray, out hit, 60f, 1 << 6)) //out: ��� ���� �Ű����� //terrain�� ����-����, ����, �� ���� ���� �� ��
            {
                //���� ������ ���� ��ǥ�� ��ũ�� ���� ��ǥ�� �ٲ۴�.
                Vector3 relative = tr.InverseTransformPoint(hit.point).normalized;
                //���� ray�� ���� ������ ��ũ�� �´� ���� ��ǥ�� �ٲ۴�.
                float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg; //pi*2/360 
                tr.localRotation = Quaternion.Lerp(tr.localRotation, Quaternion.Euler(0f, angle, 0f), Time.deltaTime * rotSpeed);                                                                //��ź��Ʈ�Լ�(���� ����.x, ���� ����.z - y�� �����̴ϱ�� ) * pi*2/360 
                //tr.Rotate(0f, angle * Time.deltaTime * rotSpeed, 0f); //y�����θ� ȸ���Ѵ�.
            }
        }
        else //�ٸ� ��Ʈ��ũ ������ ���̸�
        {
            tr.localRotation = Quaternion.Slerp(tr.localRotation, curRot, Time.deltaTime*3f);
        }

    }
}
