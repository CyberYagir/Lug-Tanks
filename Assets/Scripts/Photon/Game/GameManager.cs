using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Photon.Game
{
    public class GameManager : MonoBehaviourPunCallbacks, IInRoomCallbacks, IOnEventCallback
    {
        public static GameManager Instance;
        public static bool pause;



        [SerializeField] private Player localPlayer;


        [SerializeField] private Player playerPrefab;
        [SerializeField] private float time;
        [SerializeField] private GameObject tabMenu;
        [SerializeField] private List<Map> maps;

        private int map = 0;        
        
        public List<Map> Maps => maps;
        public Map ActiveMap => Maps[map];

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
            SwitchModes();
        }

        private void Update()
        {
            tabMenu.SetActive(Input.GetKey(KeyCode.Tab) || Timer.Instance.end);
            if (localPlayer == null)
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
                    PhotonNetwork.Destroy(localPlayer.gameObject);
                }
            }

            if (pause || Timer.Instance.end || tabMenu.active)
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

        
        
        private void ChangeMap()
        {
            map = (int)PhotonNetwork.CurrentRoom.CustomProperties["Map"];
            
            for (int i = 0; i < Maps.Count; i++)
            {
                Maps[i].SetMapState(i == map);
            }
        }

        private void SwitchModes()
        {
            switch ((string) PhotonNetwork.CurrentRoom.CustomProperties["Mode"])
            {
                case "TDM":
                    ConfigurePlayerTDM();
                    Debug.Log("TDM Mode");
                    break;
                default:
                    Debug.Log("Default Mode");
                    break;
            }
        }

        private void ConfigurePlayerTDM()
        {
                var newC = new ExitGames.Client.Photon.Hashtable();
                newC.Add("k", 0);
                newC.Add("d", 0);

                int blue = 0;
                int red = 0;

                
                foreach (var pl in PhotonNetwork.CurrentRoom.Players)
                {
                    var player = pl.Value;
                    if (player.CustomProperties["Team"] != null)
                    {
                        var team = (int)player.CustomProperties["Team"];
                        
                        if (team == 1)
                        {
                            red++;
                        }else
                        if (team == 2)
                        {
                            blue++;
                        }
                    }
                }
                
                
                newC.Add("Team", blue > red ? 1 : 2);

                PhotonNetwork.LocalPlayer.SetCustomProperties(newC);
        }
        
        
        
        

        public void RespawnPlayer()
        {
            if (!Timer.Instance.end)
                Player.RefreshInstance(ref localPlayer, playerPrefab, true);
        }

        public void Disconnect()
        {
            if (localPlayer != null)
            {
                PhotonNetwork.Destroy(localPlayer.gameObject);
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
                // RespawnAll(otherPlayer);

                if (PhotonNetwork.IsMasterClient)
                {
                    var tanks = FindObjectsByType<Tank.Controller.Tank>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
                    foreach (var tank in tanks)
                    {
                        if (tank.gameObject.GetPhotonView().Owner.ActorNumber == otherPlayer.ActorNumber)
                        {
                            PhotonNetwork.Destroy(tank.gameObject);
                            break;
                        }
                    }
                }
            }
            base.OnPlayerLeftRoom(otherPlayer);
        }
        void IInRoomCallbacks.OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
        {
            // RespawnAll(PhotonNetwork.CurrentRoom.GetPlayer(PhotonNetwork.CurrentRoom.masterClientId));
            // foreach (var item in FindObjectsOfType<Bonus>())
            // {
            //     var n = PhotonNetwork.Instantiate("Bonus", item.transform.position, item.transform.rotation);
            //     n.GetPhotonView().RPC("SetParent", RpcTarget.AllBuffered, item.bonus_id, item.box_type);
            // }
        }


        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code == 0)
            {
                var objs = (object[])photonEvent.CustomData;
                
                ChangeMap();
                SwitchModes();
                
                if (!PhotonNetwork.IsMasterClient)
                    Timer.Instance.SetTimer();
            }
        }
        
        
        public Tank.Controller.Tank GetPlayerTank() => localPlayer.Tank;
        public override void OnDisable() => PhotonNetwork.RemoveCallbackTarget(this);
        public override void OnEnable() => PhotonNetwork.AddCallbackTarget(this);


    }
}