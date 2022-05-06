using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomItem : MonoBehaviour
{
    [SerializeField] private TMP_Text r_name, r_count;
    [SerializeField] private string roomname;
    public void Connect()
    {
        PhotonLobby.lobby.JoinRoom(roomname);
    }

    public void Init(string rname, string text, string count)
    {
        roomname = rname;
        r_name.text = text;
        r_count.text = count;
    }
}
