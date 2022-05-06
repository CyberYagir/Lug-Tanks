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
    [SerializeField] private Window[] windows;
    [SerializeField] private PHPMenuManager menuManager;


    private Tank tank;
    
    public void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        tank = FindObjectOfType<Tank>();
    }
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

        if (WebData.tankData != null && PhotonNetwork.IsConnected)
        {
            menuManager.gameObject.SetActive(false);
            tank.tankOptions.corpus = WebData.tankData.corpus;
            tank.tankOptions.weapon = WebData.tankData.weapon;
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
