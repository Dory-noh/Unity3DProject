using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime; //����API: ���� ��Ʈ��ũ ����� ���� �޼����� ����
using Photon.Pun; 

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private readonly string version = "1.0";
    private string userId = "Dory";
    void Awake()
    {
        //������ Ŭ���̾�Ʈ�� �� �ڵ� ����ȭ
        PhotonNetwork.AutomaticallySyncScene = true; //���� �� ������ Ŭ���̾�Ʈ�� �� ����ȭ
        PhotonNetwork.GameVersion = version;
        PhotonNetwork.NickName = userId;
        PhotonNetwork.ConnectUsingSettings(); //���� Ŭ������ ������ ������ ����
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Master!");
        Debug.Log($"PhotonNetWork.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        Debug.Log($"PhotonNetWork.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log($"JoinRandomFailed : {returnCode.ToString()} {message.ToString()}");
        //�� ���� �� ����
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        PhotonNetwork.JoinOrCreateRoom("My Room", roomOptions, TypedLobby.Default, null);
    }

    public override void OnCreatedRoom() //���� ��������� �� �ڵ� ȣ��Ǵ� �ݹ� �Լ�
    {
        Debug.Log("Created Room");
        Debug.Log($"Room Name = {PhotonNetwork.CurrentRoom.Name}");
    }

    public override void OnJoinedRoom() //�� ������ �Ϸ�� �� ȣ��Ǵ� �ݹ� �Լ�
    {
        Debug.Log($"PhotonNetwork.InRoom = {PhotonNetwork.InRoom}");
        Debug.Log($"PlayerCount: {PhotonNetwork.CurrentRoom.PlayerCount}");

        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {                                                    //View ID
            Debug.Log($"{player.Value.NickName}, {player.Value.ActorNumber}");
        }
        Transform[] points = GameObject.Find("SpawnPoints").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);
        PhotonNetwork.Instantiate("Player", points[idx].position, points[idx].rotation);
    }
    private void OnGUI()
    {
        GUILayout.Label($"���� ��Ʈ��ũ�� �ʴ� ���� Ƚ���� : {PhotonNetwork.SendRate.ToString()}");
        //foreach (var player in PhotonNetwork.CurrentRoom.Players)
        //{                                                    //View ID
        //    GUILayout.Label($"{player.Value.NickName}, {player.Value.ActorNumber}");
        //}
        
    }
}
