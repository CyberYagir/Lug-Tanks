using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CreateRoom : MonoBehaviour
{
    public Image preview;
    public TMP_Dropdown dropDown;
    public Slider players, time;
    public Toggle ispublic;
    public TMP_InputField mapName;
    public string mode;

    public void SetMode(string newMode)
    {
        mode = newMode;
    }
    public void Create()
    {
        PhotonLobby.lobby.CreateRoom(mapName.text, ispublic.isOn, (byte)players.value, (int)time.value, 0, mode);
    }

    public void Change()
    {
        preview.sprite = dropDown.options[dropDown.value].image;
    }

}
