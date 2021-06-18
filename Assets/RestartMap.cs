using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartMap : MonoBehaviour
{
    public int map;
    public Image preview;
    public Sprite[] maps;
    public Slider sliderTime;
    public void ChangeMap()
    {
        map++;
        if (map >= maps.Length)
        {
            map = 0;
        }
        preview.sprite = maps[map];
    }

    public void RestartRoom()
    {
        object[] content = new object[] {map, (int)sliderTime.value};
        PhotonNetwork.RaiseEvent(0, content, new Photon.Realtime.RaiseEventOptions() { Receivers = Photon.Realtime.ReceiverGroup.All }, SendOptions.SendReliable);
    }
}
