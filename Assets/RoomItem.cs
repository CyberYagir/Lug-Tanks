using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomItem : MonoBehaviour
{
    public TMP_Text r_name, r_count;
    public string roomname;
    public void Connect()
    {
        PhotonLobby.lobby.JoinRoom(roomname);
    }
}
