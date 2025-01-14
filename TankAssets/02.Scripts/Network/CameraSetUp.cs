using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraSetUp : MonoBehaviourPun //MonoBehavior이자 Pun이다.
{
    void Start()
    {
        if (photonView.IsMine) //해당 객체가 현재 클라이언트의 소유이면
        {
            CinemachineVirtualCamera FollowCam = FindObjectOfType<CinemachineVirtualCamera>();
            FollowCam.Follow = transform;
            FollowCam.LookAt = transform;
        }
    }

}
