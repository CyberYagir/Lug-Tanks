﻿using System;
using System.Linq;
using Base.Controller;
using Base.Modifyers;
using Base.Weapons;
using Base.Weapons.Arms;
using Photon.Game.UI;
using Photon.Pun;
using Scriptable;
using UI;
using UnityEngine;
using Web;
using Random = UnityEngine.Random;

namespace Photon.Game
{
    public class Player : MonoBehaviourPun, IPunObservable
    {

        [SerializeField] private GameDataObject gameData;
        [Space] [SerializeField] private TankUIManager canvas;
        [Space] [SerializeField] private Tank tank;
        [SerializeField] private TankBoosters tankBoosters;
        [SerializeField] private Move tankMove;
        [SerializeField] private WeaponRotate weaponRotate;
        [SerializeField] private CameraLook cameraLook;


        private float timeToDestroy;

        public GameDataObject GameData => gameData;
        public Tank Tank => tank;
        public TankBoosters Boosters => tankBoosters;

        public CameraLook CameraLook => cameraLook;

        public float GetTime() => timeToDestroy;

        private void Awake()
        {
            GameManager.IsOnPause = false;

            tank.Init(this);

            if (IsInMenu()) return;
            
            transform.name = photonView.Owner.NickName;

            canvas.gameObject.SetActive(photonView.IsMine);
            
            if (!photonView.IsMine)
            {
                CameraLook.gameObject.SetActive(false);


                Destroy(tankMove);
                Destroy(tankBoosters);
                Destroy(weaponRotate);



                foreach (var t in tank.weapons)
                {
                    if (t.transform.GetComponent<WeaponAnimate>())
                    {
                        t.transform.GetComponent<WeaponAnimate>().enabled = false;
                    }

                    t.enabled = false;
                }

                foreach (var t in tank.corpuses)
                {
                    t.obj.layer = LayerMask.NameToLayer("Default");
                }
            }
            else
            {
                tankMove.Init(this);
                canvas.Init(this);
                tankBoosters.Init(tank);
                
                
                tank.tankOptions.weapon = WebDataService.tankData.weapon;
                tank.tankOptions.corpus = WebDataService.tankData.corpus;
                tank.tankOptions.team = (int) PhotonNetwork.LocalPlayer.CustomProperties["Team"];
                tank.tankOptions.hp = tank.corpuses[tank.tankOptions.corpus].hp;
            }
        }

        private bool IsInMenu()
        {
            if (!PhotonNetwork.InRoom)
            {
                enabled = false;
                Destroy(tankMove);
                Destroy(tankBoosters);
                Destroy(weaponRotate);
                Destroy(cameraLook.gameObject);

                return true;
            }

            return false;
        }

        private void Start()
        {
            if (!photonView.IsMine)
            {
                if (Weapon.IsEnemyTeam(photonView))
                {
                    tank.ChangeTankLocalTeam(Tank.TankTeam.Enemy);
                }
            }
        }

        private void Update()
        {
            if (!photonView.IsMine)
            {
                if (tank.tankOptions.weapon != -1)
                    tank.weapons[tank.tankOptions.weapon].transform.rotation = Quaternion.Lerp(tank.weapons[tank.tankOptions.weapon].transform.rotation, tank.tankOptions.turretRotation, tank.weapons[tank.tankOptions.weapon].GetRotSpeed() * 2f * Time.deltaTime);
            }
            else
            {
                SuicideUpdate();
                FallUpdate();
                
                tank.tankOptions.turretRotation = tank.weapons[tank.tankOptions.weapon].transform.rotation;
                
                CheckDeath();
            }
        }

        private void FallUpdate()
        {
            if (transform.position.y < -20)
            {
                tank.tankOptions.hp = 0;
            }
        }

        private void SuicideUpdate()
        {
            if (Input.GetKey(KeyCode.Delete))
            {
                timeToDestroy += Time.deltaTime;
                if (timeToDestroy > 2f)
                {
                    tank.tankOptions.hp = 0;
                }
            }
            else
            {
                timeToDestroy = 0;
            }
        }

        [PunRPC]
        public void TakeDamage(float damage, string actorName, int weapon)
        {
            if (photonView.IsMine)
            {
                if (tank.tankOptions.hp > 0)
                {
                    tank.tankOptions.hp -= damage / Boosters.DefenceIncrease;
                    if (tank.tankOptions.hp <= 0)
                    {
                        photonView.RPC("KillRPC", RpcTarget.All, actorName, photonView.Owner.NickName, weapon);
                        AddDeath();
                        foreach (var item in PhotonNetwork.CurrentRoom.Players)
                        {
                            if (item.Value.NickName == actorName)
                            {
                                var newC = new ExitGames.Client.Photon.Hashtable();
                                newC.Add("k", ((int) item.Value.CustomProperties["k"]) + 1);
                                newC.Add("d", (int) item.Value.CustomProperties["d"]);
                                newC.Add("Team", (int) item.Value.CustomProperties["Team"]);
                                item.Value.SetCustomProperties(newC);


                                if ((string) PhotonNetwork.CurrentRoom.CustomProperties["Mode"] == "TDM")
                                {
                                    var rm = new ExitGames.Client.Photon.Hashtable();
                                    rm.Add("Mode", PhotonNetwork.CurrentRoom.CustomProperties["Mode"]);
                                    rm.Add("Map", (int) PhotonNetwork.CurrentRoom.CustomProperties["Map"]);
                                    rm.Add("Time", (int) PhotonNetwork.CurrentRoom.CustomProperties["Time"]);

                                    var redK = (int) PhotonNetwork.CurrentRoom.CustomProperties["RedKills"];
                                    var blueK = (int) PhotonNetwork.CurrentRoom.CustomProperties["BlueKills"];

                                    if ((int) item.Value.CustomProperties["Team"] == 1)
                                    {
                                        redK++;
                                    }

                                    if ((int) item.Value.CustomProperties["Team"] == 2)
                                    {
                                        blueK++;
                                    }

                                    rm.Add("BlueKills", blueK);
                                    rm.Add("RedKills", redK);
                                    PhotonNetwork.CurrentRoom.SetCustomProperties(rm);
                                }

                                break;
                            }
                        }
                    }
                }
            }
        }

        public void AddDeath()
        {
            var k = (int) photonView.Owner.CustomProperties["k"];
            var d = (int) photonView.Owner.CustomProperties["d"];
            var newC = new ExitGames.Client.Photon.Hashtable();
            newC.Add("k", k);
            newC.Add("d", d + 1);
            newC.Add("Team", (int) photonView.Owner.CustomProperties["Team"]);
            photonView.Owner.SetCustomProperties(newC);


            WebDataService.data.userStatistics.deaths++;
        }

        [PunRPC]
        public void KillRPC(string playerKiller, string playerKilled, int weapon)
        {
            if (PhotonNetwork.NickName == playerKiller)
            {
                WebDataService.tankData.exp += 15;
                WebDataService.data.userStatistics.kills++;
                PhotonNetwork.LocalPlayer.CustomProperties["Exp"] = WebDataService.tankData.exp;
                WebDataService.SaveStart();
            }

            KillsList.Instance.Create(playerKiller, playerKilled, weapon);
        }

        public void CheckDeath()
        {
            if (photonView.IsMine)
            {
                if (tank.tankOptions.hp <= 0)
                {
                    AddDeath();
                    
                    if (tank.tankOptions.weapon == -1 || tank.tankOptions.corpus == -1) return;
                    
                    var dead = PhotonNetwork.Instantiate("TankDead", transform.position, transform.rotation);
                    var rb = GetComponent<Rigidbody>();
                    dead.GetPhotonView().RPC("Set", RpcTarget.All, tank.tankOptions.weapon, tank.tankOptions.corpus, tank.tankOptions.turretRotation, rb.velocity, rb.mass, rb.drag, new Vector3(Random.Range(-5, 5), Random.Range(5, 20), Random.Range(-5, 10)), new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100)), true);
                    dead.GetComponent<DeadTank>().StartDestroy();
                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }

        public static void RefreshInstance(ref Player player, Player playerPrefab, bool withMasterClient = false)
        {
            if (PhotonNetwork.IsMasterClient == false || withMasterClient == true)
            {
                var team = GameManager.Instance.ActiveMap.GetTeamSpawn((int) PhotonNetwork.LocalPlayer.CustomProperties["Team"]);
                var pos = team.spawns[Random.Range(0, team.spawns.Length)].position;
                var rot = Quaternion.identity;
                if (player != null)
                {
                    pos = player.transform.position;
                    rot = player.transform.rotation;
                    PhotonNetwork.Destroy(player.gameObject);
                }

                player = PhotonNetwork.Instantiate(playerPrefab.gameObject.name, pos, rot).GetComponent<Player>();
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(JsonUtility.ToJson(tank.tankOptions, true));
                stream.SendNext(tank.bonuses.ToArray());
            }
            else
            {
                tank.tankOptions = JsonUtility.FromJson<Tank.TankOptions>((string) stream.ReceiveNext());
                tank.bonuses = ((int[]) stream.ReceiveNext()).ToList();
            }
        }
    }
}
