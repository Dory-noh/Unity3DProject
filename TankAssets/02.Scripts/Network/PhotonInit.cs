using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; //Pun:Photon Unity Network; - ����Ƽ�� ����ȭ�� ��ũ��ũ ���� �����̽�
using Photon.Realtime;
//using Cinemachine; //�������� ��Ʈ��ũ ���� �����̽�
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PhotonInit : MonoBehaviourPunCallbacks //PUN ��Ʈ��ũ ���� ���̺귯��
{
    public string Version = "V1.0";
    public InputField userId;
    public InputField roomName;
    public GameObject scrollContents; //�¾ ��ġ�� �θ� ������Ʈ
    public GameObject roomItem;

    void Awake()
    {
        if (!PhotonNetwork.IsConnected) //�κ�� ���ƿ��� �� �ڵ� ���� �����Ѵ�.
        {
            //���� ��Ʈ��ũ�� ���� �������� ����
            PhotonNetwork.GameVersion = Version;
            //���� �������� ��Ʈ��ũ - ������ ������ ����
            PhotonNetwork.ConnectUsingSettings();
            roomName.text = $"ROOM_ {Random.Range(0, 999).ToString("000")}"; //text format: �� �ڸ� ���� 
            roomItem = Resources.Load<GameObject>("Prefab/RoomItem");
        }
    }


    public override void OnConnectedToMaster() //������ ����(���� ����)
    {
        PhotonNetwork.JoinLobby(); //�κ� ����
    }
    
    public override void OnJoinedLobby()
    {
        Debug.Log("Lobby�� ����");
        userId.text = GetUserId();
    }

    string GetUserId()
    {
        string userId = PlayerPrefs.GetString("USER_ID"); //UserId�� �����ϱ� ���ؼ� �����Ͽ����ϴ�. ���� Ű: USER_ID
        if (string.IsNullOrEmpty(userId)) //id���� ����ְų� null(Ư������ �� �̻��� ���� �־��� ���)�̶�� user�̸��� ������ ���� �����Ѵ�.
        {
            userId = $"USER_{Random.Range(0,999).ToString()}";
        }
        return userId;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("No Room!");
        //���̳� �� ���ӿ� �������� ��� �ڵ� ȣ�� �ݹ� �Լ�
        print("�� ���ӿ� �����Ͽ����ϴ�. ���� ���� ����ϴ�.");
        PhotonNetwork.CreateRoom("TankBattle Room", new RoomOptions { IsOpen = true, IsVisible = true, MaxPlayers = 20 });
        //���� ���� ����(�� �̸�, �� �ɼ�)                             //�� ���� ����, �� ����Ʈ�� �߰� �� ������, �ִ� ������ ��
    }
    public override void OnJoinedRoom() //���� ��������� �ڵ����� ȣ��ȴ�.
    {
        Debug.Log("Enter Room : �濡 ����");
        //CreateTank(); //Tank�� PlayScene���� �¾�� �Ѵ�.
        StartCoroutine(LoadBattleField());
    }

    IEnumerator LoadBattleField()
    {
        //���� �̵��ϴ� ���� ���� Ŭ���� �����κ��� ��Ʈ��ũ �޽��� ���� �ߴ��Ѵ�.
        PhotonNetwork.IsMessageQueueRunning = false;
        //�񵿱������� ���� �ε��Ѵ�.
        AsyncOperation ao = SceneManager.LoadSceneAsync("PlayScene");
        yield return ao; //�񵿱������� �����Ѵ�.
        //������: A�� ������ B�� ���۵� //�񵿱���: A�� ��� ����ǵ� ������� B�� ����� �� �ִ�.
    }

    public void OnClickJoinRandomRoom()
    {
        //���� �÷��̾��� �̸� ����
        PhotonNetwork.NickName = userId.text;
        //�÷��̾� �̸� ����
        PlayerPrefs.SetString("USER_ID", userId.text);
        //�������� ����� ������ ����
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
        roomOptions.IsOpen = true; //�� ����/����� ����
        roomOptions.IsVisible = true; //�� ��� ����Ʈ�� ���̰� �� �� �� ���� ����
        roomOptions.MaxPlayers = 20; //�ִ� ������ ��

        PhotonNetwork.CreateRoom(_roomName, roomOptions, TypedLobby.Default);
    }                               //�� �̸�, �� ����, �κ� �ϳ��� �⺻ ���� ��

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log($"Failed to create Room = {returnCode} : {message}");
    }

    //������ �� ����� ����ǰų� ���Ӱ� ���� ������� �� �ڵ����� ȣ��Ǵ� �޼���
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
                roomData.DisplayRoomData();//�ؽ�Ʈ ������ ǥ���Ѵ�.
                roomData.GetComponent<Button>().onClick.AddListener(delegate { OnClickRoomItem(roomData.roomName); });
                //delegate�� �̿��Ͽ� �̺�Ʈȭ ��. - OnClick Event�� �ڵ����� ������ش�.
            }
        }
    }
    public void OnClickRoomItem(string roomName)
    {
        PhotonNetwork.NickName = userId.text;
        PlayerPrefs.SetString("USER_ID", userId.text);
        PhotonNetwork.JoinRoom(roomName); //�����ڸ��� �� �̸����� �����Ѵ�.
    }

    public void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());
    }

    //void CreateTank()
    //{
    //    float pos = Random.Range(-100f, 100f);
    //    PhotonNetwork.Instantiate("Prefab/Tank", new Vector3(pos, 5f, pos), Quaternion.identity, 0, null);
    //    //������, ���޵Ǵ� �� ����
    //}
}
