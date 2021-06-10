using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rooms : MonoBehaviour
{
    public Transform holder;
    public Transform item;
    public void UpdateRooms()
    {
        var r = PhotonLobby.lobby.rooms;
        foreach (Transform item in holder)
        {
            Destroy(item.gameObject);
        }
        for (int i = 0; i < r.Count; i++)
        {
            var n = Instantiate(item.gameObject, holder);
            var k = n.GetComponent<RoomItem>();
            k.r_name.text = r[i].Name.Split('_')[0];
            k.roomname = r[i].Name;
            k.r_count.text = r[i].PlayerCount + "/" + r[i].MaxPlayers;
            n.SetActive(true);
        }
    }
}
