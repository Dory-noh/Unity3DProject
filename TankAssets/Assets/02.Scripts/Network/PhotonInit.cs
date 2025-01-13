using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; //Pun:Photon Unity Network; - 유니티에 최적화된 네크워크 네임 스페이스
using Photon.Realtime;
using Cinemachine; //범용적인 네트워크 네임 스페이스

public class PhotonInit : MonoBehaviourPunCallbacks //CallBack: 자동 호출
{
    public string Version = "V1.0";
    void Awake()
    {
        //포톤 네트워크의 게임 버전별로 접속
        PhotonNetwork.GameVersion = Version;
        //위의 버전별로 네트워크 - 마스터 서버에 접속
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster() //마스터 서버(방장 서버)
    {
        PhotonNetwork.JoinLobby(); //로비에 연결
    }
    
    public override void OnJoinedLobby()
    {
        Debug.Log("Lobby에 입장");
        PhotonNetwork.JoinRandomRoom(); //아무 방에나 접속한다.
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //방이나 룸 접속에 실패했을 경우 자동 호출 콜백 함수
        print("방 접속에 실패하였습니다. 방을 새로 만듭니다.");
        PhotonNetwork.CreateRoom("TankBattle Room", new RoomOptions { IsOpen = true, IsVisible = true, MaxPlayers = 20 });
        //룸을 새로 만듦(방 이름, 방 옵션)                             //방 공개 여부, 방 리스트에 뜨게 할 것인지, 최대 접속자 수
    }
    public override void OnJoinedRoom() //방이 만들어지면 자동으로 호출된다.
    {
        Debug.Log("Enter Room : 방에 입장");
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
        //개인전, 전달되는 것 없음
    }
}
