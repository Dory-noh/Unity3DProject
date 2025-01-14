using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomData : MonoBehaviour
{
    [HideInInspector] public string roomName = "";
    [HideInInspector] public int connectPlayer = 0;
    [HideInInspector] public int maxPlayer = 0;
    public Text txtRoomName;
    public Text txtConnectInfo;
    public void DisplayRoomData()
    {
        txtRoomName.text = roomName;
        txtConnectInfo.text = $"{connectPlayer.ToString()} / {maxPlayer.ToString()}";
    }
}
