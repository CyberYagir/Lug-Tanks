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
    public Sprite[] maps;
    public int map;
    public int mapCount;
    public void SetMode(string newMode)
    {
        mode = newMode;
    }
    public void ChangeMap()
    {
        map++;
        if (map >= mapCount)
        {
            map = 0;
        }

        preview.sprite = maps[map];
    }
    public void Create()
    {
        PhotonLobby.lobby.CreateRoom(mapName.text, ispublic.isOn, (byte)players.value, (int)time.value, map, mode);
    }

    public void Change()
    {
        preview.sprite = dropDown.options[dropDown.value].image;
    }

}
