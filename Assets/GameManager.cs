using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks {

    public Player playerPrefab;

    public Player LocalPlayer;

    public Transform[] spawns;

    public float time;

    public GameObject tabMenu;
    private void Awake()
    {
        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("Menu");
            return;
        }
    }


    public void RespawnPlayer()
    {
        if (!Timer.timer_.end)
            Player.RefreshInstance(ref LocalPlayer, playerPrefab, true);
    }
    private void Update()
    {
        tabMenu.SetActive(Input.GetKey(KeyCode.Tab));
        if (LocalPlayer == null)
        {
            time += Time.deltaTime;
            if (time > 2.5f)
            {
                RespawnPlayer();
            }
        }
        else
        {
            time = 0;
            if (Timer.timer_.end)
            {
                PhotonNetwork.Destroy(LocalPlayer.gameObject);
            }
        }
    }
    public void Disconnect()
    {
        if (LocalPlayer != null)
        {
            PhotonNetwork.Destroy(LocalPlayer.gameObject);
        }
        PhotonNetwork.LeaveRoom();
        Destroy(GameObject.Find("Manager"));
        Cursor.visible = true;
        SceneManager.LoadScene("Menu");
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        //Player.RefreshInstance(ref LocalPlayer, playerPrefab);
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (otherPlayer.IsLocal)
        {
            Disconnect();
        }
        base.OnPlayerLeftRoom(otherPlayer);
    }
}
