using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime; //포톤API: 포톤 네트워크 사용을 위한 메서드의 집합
using Photon.Pun; 

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private readonly string version = "1.0";
    private string userId = "Dory";
    void Awake()
    {
        //마스터 클라이언트의 씬 자동 동기화
        PhotonNetwork.AutomaticallySyncScene = true; //같은 방 입장한 클라이언트간 씬 동기화
        PhotonNetwork.GameVersion = version;
        PhotonNetwork.NickName = userId;
        PhotonNetwork.ConnectUsingSettings(); //포톤 클라우드의 마스터 서버로 접속
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
        //룸 생성 및 조인
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        PhotonNetwork.JoinOrCreateRoom("My Room", roomOptions, TypedLobby.Default, null);
    }

    public override void OnCreatedRoom() //룸이 만들어졌을 때 자동 호출되는 콜백 함수
    {
        Debug.Log("Created Room");
        Debug.Log($"Room Name = {PhotonNetwork.CurrentRoom.Name}");
    }

    public override void OnJoinedRoom() //룸 생성이 완료된 후 호출되는 콜백 함수
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
        GUILayout.Label($"포톤 네트워크의 초당 전송 횟수는 : {PhotonNetwork.SendRate.ToString()}");
        //foreach (var player in PhotonNetwork.CurrentRoom.Players)
        //{                                                    //View ID
        //    GUILayout.Label($"{player.Value.NickName}, {player.Value.ActorNumber}");
        //}
        
    }
}
