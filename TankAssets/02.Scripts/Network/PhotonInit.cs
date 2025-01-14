using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; //Pun:Photon Unity Network; - 유니티에 최적화된 네크워크 네임 스페이스
using Photon.Realtime;
//using Cinemachine; //범용적인 네트워크 네임 스페이스
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PhotonInit : MonoBehaviourPunCallbacks //PUN 네트워크 관련 라이브러리
{
    public string Version = "V1.0";
    public InputField userId;
    public InputField roomName;
    public GameObject scrollContents; //태어날 위치의 부모 오브젝트
    public GameObject roomItem;

    void Awake()
    {
        if (!PhotonNetwork.IsConnected) //로비로 돌아왔을 때 자동 접속 방지한다.
        {
            //포톤 네트워크의 게임 버전별로 접속
            PhotonNetwork.GameVersion = Version;
            //위의 버전별로 네트워크 - 마스터 서버에 접속
            PhotonNetwork.ConnectUsingSettings();
            roomName.text = $"ROOM_ {Random.Range(0, 999).ToString("000")}"; //text format: 세 자리 숫자 
            roomItem = Resources.Load<GameObject>("Prefab/RoomItem");
        }
    }


    public override void OnConnectedToMaster() //마스터 서버(방장 서버)
    {
        PhotonNetwork.JoinLobby(); //로비에 연결
    }
    
    public override void OnJoinedLobby()
    {
        Debug.Log("Lobby에 입장");
        userId.text = GetUserId();
    }

    string GetUserId()
    {
        string userId = PlayerPrefs.GetString("USER_ID"); //UserId를 저장하기 위해서 예약하였슴니다. 예약 키: USER_ID
        if (string.IsNullOrEmpty(userId)) //id값이 비어있거나 null(특수문자 등 이상한 값을 넣었을 경우)이라면 user이름을 다음과 같이 설정한다.
        {
            userId = $"USER_{Random.Range(0,999).ToString()}";
        }
        return userId;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("No Room!");
        //방이나 룸 접속에 실패했을 경우 자동 호출 콜백 함수
        print("방 접속에 실패하였습니다. 방을 새로 만듭니다.");
        PhotonNetwork.CreateRoom("TankBattle Room", new RoomOptions { IsOpen = true, IsVisible = true, MaxPlayers = 20 });
        //룸을 새로 만듦(방 이름, 방 옵션)                             //방 공개 여부, 방 리스트에 뜨게 할 것인지, 최대 접속자 수
    }
    public override void OnJoinedRoom() //방이 만들어지면 자동으로 호출된다.
    {
        Debug.Log("Enter Room : 방에 입장");
        //CreateTank(); //Tank는 PlayScene에서 태어나야 한다.
        StartCoroutine(LoadBattleField());
    }

    IEnumerator LoadBattleField()
    {
        //씬을 이동하는 동안 포톤 클라우드 서버로부터 네트워크 메시지 수신 중단한다.
        PhotonNetwork.IsMessageQueueRunning = false;
        //비동기적으로 씬을 로딩한다.
        AsyncOperation ao = SceneManager.LoadSceneAsync("PlayScene");
        yield return ao; //비동기적으로 리턴한다.
        //동기적: A가 끝나야 B가 시작됨 //비동기적: A가 어떻게 진행되든 상관없이 B가 실행될 수 있다.
    }

    public void OnClickJoinRandomRoom()
    {
        //로컬 플레이어의 이름 설정
        PhotonNetwork.NickName = userId.text;
        //플레이어 이름 저장
        PlayerPrefs.SetString("USER_ID", userId.text);
        //무작위로 추출된 룸으로 입장
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnclickCreateRoom()
    {
        string _roomName = roomName.text;
        if (string.IsNullOrEmpty(_roomName)) {
            _roomName = $"ROOM_ {Random.Range(0, 999).ToString()}";
        }

        PhotonNetwork.NickName = userId.text;
        PlayerPrefs.SetString("USER_ID", userId.text);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true; //룸 공개/비공개 여부
        roomOptions.IsVisible = true; //룸 목록 리스트에 보이게 할 지 안 할지 여부
        roomOptions.MaxPlayers = 20; //최대 접속자 수

        PhotonNetwork.CreateRoom(_roomName, roomOptions, TypedLobby.Default);
    }                               //방 이름, 방 설정, 로비가 하나인 기본 설정 값

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"Failed to create Room = {returnCode} : {message}");
    }

    //생성된 방 목록이 변경되거나 새롭게 방을 만들었을 때 자동으로 호출되는 메서드
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo roomInfo in roomList)
        {
            if (!roomInfo.RemovedFromList)
            {
                GameObject room = (GameObject)Instantiate(roomItem);
                room.transform.SetParent(scrollContents.transform, false);

                RoomData roomData = room.GetComponent<RoomData>();
                roomData.roomName = roomInfo.Name;
                roomData.maxPlayer = roomInfo.MaxPlayers;
                roomData.connectPlayer = roomInfo.PlayerCount;
                roomData.DisplayRoomData();//텍스트 정보를 표시한다.
                roomData.GetComponent<Button>().onClick.AddListener(delegate { OnClickRoomItem(roomData.roomName); });
                //delegate를 이용하여 이벤트화 함. - OnClick Event를 자동으로 만들어준다.
            }
        }
    }
    public void OnClickRoomItem(string roomName)
    {
        PhotonNetwork.NickName = userId.text;
        PlayerPrefs.SetString("USER_ID", userId.text);
        PhotonNetwork.JoinRoom(roomName); //누르자마자 방 이름으로 접속한다.
    }

    public void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());
    }

    //void CreateTank()
    //{
    //    float pos = Random.Range(-100f, 100f);
    //    PhotonNetwork.Instantiate("Prefab/Tank", new Vector3(pos, 5f, pos), Quaternion.identity, 0, null);
    //    //개인전, 전달되는 것 없음
    //}
}
