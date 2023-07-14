using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Photon.Game;
using UnityEngine;
using UnityEngine.UI;

public class RestartMap : MonoBehaviour
{
    [SerializeField] private int map;
    [SerializeField] private Image preview;
    [SerializeField] private Sprite[] maps;
    [SerializeField] private Slider sliderTime;
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
        var rm = new ExitGames.Client.Photon.Hashtable();
        rm.Add("Mode", PhotonNetwork.CurrentRoom.CustomProperties["Mode"]);
        rm.Add("Map", map);
        rm.Add("Time", (int)sliderTime.value);
        if (PhotonNetwork.CurrentRoom.CustomProperties["Mode"] == "TDM"){
            var redK = (int)PhotonNetwork.CurrentRoom.CustomProperties["RedKills"];
            var blueK = (int)PhotonNetwork.CurrentRoom.CustomProperties["BlueKills"];
            rm.Add("BlueKills", blueK);
            rm.Add("RedKills", redK);
        }
        PhotonNetwork.CurrentRoom.SetCustomProperties(rm);
        Timer.Instance.SetTimer();
        object[] content = new object[] {};
        PhotonNetwork.RaiseEvent(0, content, new Photon.Realtime.RaiseEventOptions() { Receivers = Photon.Realtime.ReceiverGroup.All }, SendOptions.SendReliable);
    }
}
