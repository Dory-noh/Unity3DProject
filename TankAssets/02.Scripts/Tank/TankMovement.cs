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
    private Vector3 curPos = Vector3.zero; //�ٸ� ������ ��ġ���� �޴� ����
    private Quaternion curRot = Quaternion.identity; //�ٸ� ������ ȸ������ �޴� ����
    void Awake()
    {
        tr = transform;
        input = GetComponent<TankInput>();
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0f, -0.5f, 0f); //��ũ ���� �߽� ���
        photonView.Synchronization = ViewSynchronization.Unreliable;
        photonView.ObservedComponents[0] = this;
        //pv = GetComponent<PhotonView>();
        ////��� ����� UDP
        //pv.Synchronization = ViewSynchronization.UnreliableOnChange;
        //pv.ObservedComponents[0] = this;
        ////photon view�� ���� ��ũ��Ʈ�� �����Ͽ�, ��ũ��Ʈ�� �ִ� �����͸� ��Ʈ��ũ�� ���� ����ȭ�Ѵ�.
        //�� ������ MonoBehaviour'Pun'�� ������� ���� �� ���
        curPos = tr.position;
        curRot = tr.rotation;
    }

    void FixedUpdate() //�����̴� �� Fixed Update�� ���ش�.
    {
        //���� ������ ��� ����������.
        if (GameManager.Instance != null && GameManager.Instance.isGameOver)
        {
            return;
        }
        if (photonView.IsMine) //photonView�� ������ ���̶��
        {
            tr.Translate(Vector3.forward * input.move * moveSpeed * Time.deltaTime);
            tr.Rotate(Vector3.up * input.rotate * rotSpeed * Time.deltaTime);
        }
        else //�ٸ� �������
        {
                            //������ �����Ӱ� �ٸ� ��Ʈ��ũ�� �������� �ε巴�� �����Լ��� �����δ�.
            tr.position = Vector3.Lerp(tr.position, curPos, Time.deltaTime * 3f);
            tr.rotation = Quaternion.Slerp(tr.rotation, curRot, Time.deltaTime * 3f);
        }
    }

    //���� ��ũ�� �����Ӱ� �ٸ� ��Ʈ��ũ ���� ��ũ�� �������� ���� ���̰� �ϱ� ���� �ۼ����ϴ� �޼���
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) //stream: ���
        {
            //���� �̵��� ȸ���� �ٸ� ��Ʈ��ũ �������� �۽��Ѵ�.
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
        }
        else if (stream.IsReading)
        {
            //�ٸ� ��Ʈ��ũ ������ ������-�̵��� ȸ��-�� �����Ѵ�.
            curPos = (Vector3)stream.ReceiveNext();
            curRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
