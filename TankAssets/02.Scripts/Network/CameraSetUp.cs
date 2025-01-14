using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraSetUp : MonoBehaviourPun //MonoBehavior���� Pun�̴�.
{
    void Start()
    {
        if (photonView.IsMine) //�ش� ��ü�� ���� Ŭ���̾�Ʈ�� �����̸�
        {
            CinemachineVirtualCamera FollowCam = FindObjectOfType<CinemachineVirtualCamera>();
            FollowCam.Follow = transform;
            FollowCam.LookAt = transform;
        }
    }

}
