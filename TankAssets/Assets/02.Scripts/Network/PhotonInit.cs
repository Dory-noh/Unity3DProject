using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; //Pun:Photon Unity Network; - ����Ƽ�� ����ȭ�� ��ũ��ũ ���� �����̽�
using Photon.Realtime;
using Cinemachine; //�������� ��Ʈ��ũ ���� �����̽�

public class PhotonInit : MonoBehaviourPunCallbacks //CallBack: �ڵ� ȣ��
{
    public string Version = "V1.0";
    void Awake()
    {
        //���� ��Ʈ��ũ�� ���� �������� ����
        PhotonNetwork.GameVersion = Version;
        //���� �������� ��Ʈ��ũ - ������ ������ ����
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster() //������ ����(���� ����)
    {
        PhotonNetwork.JoinLobby(); //�κ� ����
    }
    
    public override void OnJoinedLobby()
    {
        Debug.Log("Lobby�� ����");
        PhotonNetwork.JoinRandomRoom(); //�ƹ� �濡�� �����Ѵ�.
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //���̳� �� ���ӿ� �������� ��� �ڵ� ȣ�� �ݹ� �Լ�
        print("�� ���ӿ� �����Ͽ����ϴ�. ���� ���� ����ϴ�.");
        PhotonNetwork.CreateRoom("TankBattle Room", new RoomOptions { IsOpen = true, IsVisible = true, MaxPlayers = 20 });
        //���� ���� ����(�� �̸�, �� �ɼ�)                             //�� ���� ����, �� ����Ʈ�� �߰� �� ������, �ִ� ������ ��
    }
    public override void OnJoinedRoom() //���� ��������� �ڵ����� ȣ��ȴ�.
    {
        Debug.Log("Enter Room : �濡 ����");
        CreateTank();
    }
    public void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());
    }

    void CreateTank()
    {
        float pos = Random.Range(-100f, 100f);
        PhotonNetwork.Instantiate("Prefab/Tank",new Vector3(pos,5f,pos), Quaternion.identity,0,null);
        //������, ���޵Ǵ� �� ����
    }
}
