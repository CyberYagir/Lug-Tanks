using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;

[System.Serializable]
public class Map {
    public GameObject map;
    public List<TeamSpawn> teamSpawns;
}
[System.Serializable]
public class TeamSpawn
{
    public Transform[] spawns;
}

public class GameManager : MonoBehaviourPunCallbacks, IInRoomCallbacks, IOnEventCallback
{
    public static GameManager Instance;
    public Player playerPrefab;

    public Player LocalPlayer;
    public List<Map> maps;
    [HideInInspector]

    public float time;
    public static int map = 0;

    public GameObject tabMenu;

    public static bool pause;
    private void Awake()
    {
        pause = false;
        Instance = this;
        if (!PhotonNetwork.IsConnected)
        {   
            SceneManager.LoadScene("Menu");
            return;
        }

        ChangeMap();
    }
    public void ChangeMap()
    {
        map = (int)PhotonNetwork.CurrentRoom.CustomProperties["Map"];
        for (int i = 0; i < maps.Count; i++)
        {
            maps[i].map.SetActive(false);
        }
        maps[map].map.SetActive(true);
        if ((string)PhotonNetwork.CurrentRoom.CustomProperties["Mode"] == "TDM")
        {
            var newC = new ExitGames.Client.Photon.Hashtable();
            newC.Add("k", 0);
            newC.Add("d", 0);

            int blue = 0;
            int red = 0;

            List<Photon.Realtime.Player> players = new List<Photon.Realtime.Player>();
            foreach (var pl in PhotonNetwork.CurrentRoom.Players)
            {
                players.Add(pl.Value);
            }
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].CustomProperties["Team"] != null)
                {
                    if ((int)players[i].CustomProperties["Team"] == 1)
                    {
                        red++;
                    }
                    if ((int)players[i].CustomProperties["Team"] == 2)
                    {
                        blue++;
                    }
                }
            }
            if (blue > red)
            {
                newC.Add("Team", 1);
            }
            else
            {
                newC.Add("Team", 2);
            }

            PhotonNetwork.LocalPlayer.SetCustomProperties(newC);
        }

    }
    public void RespawnPlayer()
    {
        if (!Timer.Instance.end)
            Player.RefreshInstance(ref LocalPlayer, playerPrefab, true);
    }
    private void Update()
    {
        tabMenu.SetActive(Input.GetKey(KeyCode.Tab) || Timer.Instance.end);
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
            if (Timer.Instance.end)
            {
                PhotonNetwork.Destroy(LocalPlayer.gameObject);
            }
        }
        if (pause || Timer.Instance.end || tabMenu.active )
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
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
                    n.GetPhotonView().RPC("Set", RpcTarget.All, item.weapon, item.corpus, item.GetRot(), item.GetComponent<Rigidbody>().velocity, item.GetComponent<Rigidbody>().mass, item.GetComponent<Rigidbody>().drag, Vector3.zero, item.GetComponent<Rigidbody>().angularVelocity, false);
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
            n.GetPhotonView().RPC("SetParent", RpcTarget.AllBuffered, item.bonus_id, item.box_type);
        }
    }
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == 0)
        {
            var objs = (object[])photonEvent.CustomData;
            ChangeMap();
            if (!PhotonNetwork.IsMasterClient)
                Timer.Instance.SetTimer();
        }
    }
}
