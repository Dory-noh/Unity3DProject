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
        photonView.ObservedComponents[0] = this; //음냐?
        curRot = tr.localRotation; //부모(탱크)랑 다른 움직임을 가지고 싶을 때 local좌표 사용
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
            //카메라 스크린에서 마우스 포지션 방향으로 광선을 발사
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red); //Scene에서만 보임
            if (Physics.Raycast(ray, out hit, 60f, 1 << 6)) //out: 출력 전용 매개변수 //terrain만 감지-나무, 마을, 집 등은 감지 안 함
            {
                //맞은 지점의 월드 좌표를 탱크의 로컬 좌표로 바꾼다.
                Vector3 relative = tr.InverseTransformPoint(hit.point).normalized;
                //광선 ray가 맞은 지점을 탱크에 맞는 로컬 좌표로 바꾼다.
                float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg; //pi*2/360 
                tr.localRotation = Quaternion.Lerp(tr.localRotation, Quaternion.Euler(0f, angle, 0f), Time.deltaTime * rotSpeed);                                                                //역탄젠트함수(로컬 지점.x, 로컬 지점.z - y는 위쪽이니깐요 ) * pi*2/360 
                //tr.Rotate(0f, angle * Time.deltaTime * rotSpeed, 0f); //y쪽으로만 회전한다.
            }
        }
        else //다른 네트워크 유저의 것이면
        {
            tr.localRotation = Quaternion.Slerp(tr.localRotation, curRot, Time.deltaTime*3f);
        }

    }
}
