using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Photon.Chat;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;

public class PhotonLobby : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    public static PhotonLobby lobby;
    [SerializeField] private GameObject mainUI;
    [SerializeField] private TMP_Text errorText, lobbyText;
    [SerializeField] private int mapsCount = 1;

    public List<RoomInfo> rooms { get; private set; }


    private void OnApplicationQuit()
    {
        ClearErrorPrefs();
    }
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        errorText.text = "";
        
        if (lobby != null)
        {
            Destroy(lobby.gameObject);
        }

        lobby = this;
        DontDestroyOnLoad(gameObject);

    }
    public void InitPUN()
    { 
        if (PhotonNetwork.IsConnected == false)
        {
            PhotonNetwork.GameVersion = Application.version;
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.NickName = WebData.data.playerData.name;
        }
        else
        {
            mainUI.SetActive(true);
            lobbyText.text = PhotonNetwork.CurrentLobby.Name;
        }
        if (PlayerPrefs.HasKey("Disconnect"))
        {
            errorText.text = PlayerPrefs.GetString("Disconnect");
        }
    }
    public static void ClearErrorPrefs()
    {
        PlayerPrefs.DeleteKey("Disconnect");
    }
    public void ClearError()
    {
        ClearErrorPrefs();
    }
    public void Exit()
    {
        Application.Quit();
    }
    private void Update()
    {
        if (PhotonNetwork.InLobby)
        {
            lobbyText.text = PhotonNetwork.CurrentLobby.Name + " " + PhotonNetwork.CountOfRooms;
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(new TypedLobby("DEFAULT", LobbyType.Default));
        mainUI.SetActive(true);
        base.OnConnectedToMaster();
    }


    public override void OnJoinedLobby()
    {
        lobbyText.text = PhotonNetwork.CurrentLobby.Name;
        //PhotonNetwork.GetCustomRoomList(PhotonNetwork.CurrentLobby, "C0");
        base.OnJoinedLobby();
    }

    public void ToBattle()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    public void CreateRoom()
    {
        int map = Random.Range(0, mapsCount);
        string name = "Room [" + (PhotonNetwork.CountOfRooms + 1) +  "]_"+ map;
        ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
        h.Add("Map", map);
        h.Add("Time", 500);
        RoomOptions roomOptions = new RoomOptions() {IsVisible = true, IsOpen = true, MaxPlayers = 16, CustomRoomProperties = h};
        PhotonNetwork.CreateRoom(name, roomOptions, PhotonNetwork.CurrentLobby);
    }
    public void CreateRoom(string name, bool visible, byte players, int time, int map = 0, string mode = "FFA")
    {
        if (name.Replace(" ", "") == "")
        {
            name = "Room [" + (PhotonNetwork.CountOfRooms + 1) +  "]_"+ map;
        }
        else
        {
            name = name + "_" + map;
        }

        ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
        h.Add("Map", map);
        h.Add("Time", time);
        h.Add("Mode", mode);
        if (mode == "TDM")
        {
            h.Add("RedKills", 0);
            h.Add("BlueKills", 0);
        }
        RoomOptions roomOptions = new RoomOptions() { IsVisible = visible, IsOpen = true, MaxPlayers = players, CustomRoomProperties = h };
        PhotonNetwork.CreateRoom(name, roomOptions);
    }
    public void JoinRoom(string nm)
    {
        PhotonNetwork.JoinRoom(nm);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
    }
    public override void OnJoinedRoom()
    {
        WebData.tankData.corpus = FindObjectOfType<Tank>().tankOptions.corpus;
        WebData.tankData.weapon = FindObjectOfType<Tank>().tankOptions.weapon;
        ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
        h.Add("k", 0);
        h.Add("d", 0);
        h.Add("Exp", WebData.tankData.exp);
        h.Add("Team", 0);
        PhotonNetwork.LocalPlayer.SetCustomProperties(h);
        PhotonNetwork.LoadLevel(1);
        base.OnJoinedRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        errorText.text = "Join room Error";
        CreateRoom();
        base.OnJoinRandomFailed(returnCode, message);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        rooms = roomList;
        FindObjectOfType<Rooms>().UpdateRooms();
//        print("ROOMS COUNT: " + rooms.Count);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Failed to create room";
        base.OnCreateRoomFailed(returnCode, message);
    }
}
