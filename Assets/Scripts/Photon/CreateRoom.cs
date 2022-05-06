using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CreateRoom : MonoBehaviour
{
    [SerializeField] private Image preview;
    [SerializeField] private TMP_Dropdown dropDown;
    [SerializeField] private Slider players, time;
    [SerializeField] private Toggle ispublic;
    [SerializeField] private TMP_InputField mapName;
    [SerializeField] private string mode;
    [SerializeField] private Sprite[] maps;
    [SerializeField] private int map;
    [SerializeField] private int mapCount;
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
