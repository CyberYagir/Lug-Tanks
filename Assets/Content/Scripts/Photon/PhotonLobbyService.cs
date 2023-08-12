using System;
using System.Collections.Generic;
using Content.Scripts.Anticheat;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Photon.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Web;
using Random = UnityEngine.Random;

namespace Photon
{
    public class PhotonLobbyService : MonoBehaviourPunCallbacks
    {
        public static PhotonLobbyService Instance;
        
        
        [SerializeField] private GameObject mainUI;
        [SerializeField] private TMP_Text errorText, lobbyText;
        [SerializeField] private UIRooms rooms;
        [SerializeField] private int mapsCount = 1;

        private string autoRegion;

        public string AutoRegion => autoRegion;


        public UnityEvent OnConnectToMaster = new UnityEvent();

        public void Init()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            errorText.text = "";
        
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
        

        public void InitPUN()
        { 
            if (PhotonNetwork.IsConnected == false)
            {
                PhotonNetwork.GameVersion = Application.version;
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.NickName = WebDataService.UserData.PlayerData.Name;

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
            autoRegion = PhotonNetwork.NetworkingClient.CloudRegion;
            
            
            OnConnectToMaster.Invoke();
        }
        public override void OnJoinedLobby()
        {
            lobbyText.text = PhotonNetwork.CurrentLobby.Name;
        }
        public void ToBattle()
        {
            PhotonNetwork.JoinRandomRoom();
        }
        public void CreateRoom()
        {
            int map = Random.Range(0, mapsCount);
            string name = "Room [" + (PhotonNetwork.CountOfRooms + 1) + "]";
            
            var h = CreateBaseHashtable(name, 500, map, "FFA");
            var roomOptions = CreateRoomOptions(h);
            
            
            PhotonNetwork.CreateRoom(GenerateUniqName(), roomOptions);
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            base.OnRoomListUpdate(roomList);
            rooms.UpdateRooms(roomList);
        }

        public void CreateRoom(string name, bool visible, byte players, int time, int map = 0, string mode = "FFA")
        {
            if (name.Replace(" ", "") == "")
            {
                name = $"Room [{PhotonNetwork.CountOfRooms + 1}]";
            }

            var h = CreateBaseHashtable(name, time, map, mode);

            if (mode == "TDM")
            {
                h.Add("RedKills", 0);
                h.Add("BlueKills", 0);
            }

            var roomOptions = CreateRoomOptions(h);

            roomOptions.IsVisible = visible;
            roomOptions.MaxPlayers = players;


            PhotonNetwork.CreateRoom(GenerateUniqName(), roomOptions);
        }

        public string GenerateUniqName()
        {
            var guid = Guid.NewGuid().ToString();
            Debug.LogError(guid);
            return guid;
        }
        
        private RoomOptions CreateRoomOptions(Hashtable h)
        {
            RoomOptions roomOptions = new RoomOptions()
            {
                IsVisible = true,
                IsOpen = true,
                MaxPlayers = 16,
                CustomRoomProperties = h,
                CleanupCacheOnLeave = false
            };
            return roomOptions;
        }
        private Hashtable CreateBaseHashtable(string name, int time, int map, string mode)
        {
            Hashtable h = new Hashtable();
            h.Add("RoomName", name);
            h.Add("Map", map);
            h.Add("Time", time);
            h.Add("Mode", mode);
            h.Add("BonusDropTime", 0f);
            return h;
        }

        public void JoinRoom(string nm)
        {
            PhotonNetwork.JoinRoom(nm.Replace("\t", "").Replace("\n", "").Replace("\r", ""));
        }
        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            base.OnJoinRoomFailed(returnCode, message);
            print(message);
        }
        public override void OnJoinedRoom()
        {
           
            Hashtable h = new Hashtable();
            h.Add("k", 0);
            h.Add("d", 0);
            h.Add("Exp", WebDataService.TankData.Exp);
            h.Add("Team", 0);
            PhotonNetwork.LocalPlayer.SetCustomProperties(h);
            PhotonNetwork.LoadLevel("BaseGame");
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            errorText.text = "Join room Error";
            CreateRoom();
        }
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            errorText.text = "Failed to create room";
        }
    
        private void OnApplicationQuit()
        {
            ClearErrorPrefs();
        }
    }
}
