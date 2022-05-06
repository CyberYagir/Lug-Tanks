using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rooms : MonoBehaviour
{
    [SerializeField] private Transform holder;
    [SerializeField] private Transform item;
    public void UpdateRooms()
    {
        var r = PhotonLobby.lobby.rooms;
        foreach (Transform item in holder)
        {
            Destroy(item.gameObject);
        }
        for (int i = 0; i < r.Count; i++)
        {
            if (r[i].PlayerCount != r[i].MaxPlayers)
            {
                var n = Instantiate(item.gameObject, holder);
                var k = n.GetComponent<RoomItem>();
                k.Init(r[i].Name.Split('_')[0], r[i].Name, r[i].PlayerCount + "/" + r[i].MaxPlayers);
                n.SetActive(true);
            }
        }
    }
}
