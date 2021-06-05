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

public class GameManager : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public static GameManager manager;
    public Player playerPrefab;

    public Player LocalPlayer;

    public Transform[] spawns;

    public float time;

    public GameObject tabMenu;
    private void Awake()
    {
        manager = this;
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

    public void RespawnAll(Photon.Realtime.Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (var item in FindObjectsOfType<LineScript>())
            {
                if (item.gameObject.GetPhotonView().OwnerActorNr == otherPlayer.ActorNumber)
                {
                    var line = item.GetComponent<LineRenderer>();
                    var n = PhotonNetwork.Instantiate(item.prefabName, item.transform.position, item.transform.rotation);
                    n.GetPhotonView().RPC("SetLinePoses", RpcTarget.All, line.GetPosition(0), line.GetPosition(1));
                    n.GetComponent<LineScript>().Start_Destroy();
                }
            }
            foreach (var item in FindObjectsOfType<DeadTank>())
            {
                if (item.gameObject.GetPhotonView().OwnerActorNr == otherPlayer.ActorNumber)
                {
                    var n = PhotonNetwork.Instantiate("TankDead", item.transform.position, item.transform.rotation);
                    n.GetPhotonView().RPC("Set", RpcTarget.All, item.weapon, item.corpus, item.rot, item.GetComponent<Rigidbody>().velocity, item.GetComponent<Rigidbody>().mass, item.GetComponent<Rigidbody>().drag, Vector3.zero, item.GetComponent<Rigidbody>().angularVelocity, false);
                    n.GetComponent<DeadTank>().StartDestroy();
                }
            }
        }
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (otherPlayer.IsLocal)
        {
            Disconnect();
        }
        else
        {
            RespawnAll(otherPlayer);
        }
        base.OnPlayerLeftRoom(otherPlayer);
    }
    void IInRoomCallbacks.OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        RespawnAll(PhotonNetwork.CurrentRoom.GetPlayer(PhotonNetwork.CurrentRoom.masterClientId));
        foreach (var item in FindObjectsOfType<Bonus>())
        {
            var n = PhotonNetwork.Instantiate("Bonus", item.transform.position, item.transform.rotation);
            n.GetPhotonView().RPC("SetParent", RpcTarget.AllBuffered, item.bonus_id);
        }
    }
}
