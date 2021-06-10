using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Window
{
    public Vector2 posHide, posOpen;
    public bool isOpen;
    public RectTransform rectTransform;

}

public class Menu : MonoBehaviour
{
    public Image icon;
    public Window[] windows;
    public PHPMenuManager menuManager;

    private void Update()
    {
        for (int i = 0; i < windows.Length; i++)
        {
            if (windows[i].isOpen)
            {
                windows[i].rectTransform.anchoredPosition = Vector2.Lerp(windows[i].rectTransform.anchoredPosition, windows[i].posOpen, 5f * Time.deltaTime);
            }
            else
            {
                windows[i].rectTransform.anchoredPosition = Vector2.Lerp(windows[i].rectTransform.anchoredPosition, windows[i].posHide, 5f * Time.deltaTime);
            }
        }

        if (WebData.playerData != null && PhotonNetwork.IsConnected)
        {
            menuManager.gameObject.SetActive(false);
            FindObjectOfType<Tank>().tankOptions.corpus = WebData.playerData.corpus;
            FindObjectOfType<Tank>().tankOptions.weapon = WebData.playerData.weapon;
            if (PhotonNetwork.InLobby)
                windows[0].isOpen = true;
        }
        else
        {
            windows[0].isOpen = false;
            menuManager.gameObject.SetActive(true);
        }
    }

    public void OpenWindow(int n)
    {
        for (int i = 1; i < windows.Length; i++)
        {
            if (i == n)
            {
                windows[i].isOpen = !windows[i].isOpen;
            }
            else
            {
                windows[i].isOpen = false;
            }
        }
    }
}
