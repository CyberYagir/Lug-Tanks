using System;
using System.Linq;
using Base.Controller;
using Base.Modifyers;
using Base.Weapons;
using Base.Weapons.Arms;
using Content.Scripts.Anticheat;
using CrazyGames;
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
        private bool getFirstSyncPacket = false;
        private float timeToDestroy;
        private CorpusRotator corpusRotator;
        
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
                    t.SetLayer(LayerMask.NameToLayer("Default"));
                }
            }
            else
            {
                tankMove.Init(this);
                tankBoosters.Init(tank);
                try
                {
                    CrazyEvents.Instance.GameplayStart();
                }
                catch (Exception e)
                {
                    // ignored
                }

                tank.tankOptions.weapon = WebDataService.TankData.GetWeaponId();
                tank.tankOptions.corpus = WebDataService.TankData.GetCorpusId();
                tank.tankOptions.team = ((int) PhotonNetwork.LocalPlayer.CustomProperties["Team"]).Obf();
                tank.tankOptions.hp = tank.corpuses[tank.tankOptions.Corpus].Hp.Obf();
                
                
                canvas.Init(this);
            }
        }

        private bool IsInMenu()
        {
            if (!PhotonNetwork.InRoom)
            {
                enabled = false;
                Destroy(tankMove);
                Destroy(tankBoosters);
                // Destroy(weaponRotate);
                if (cameraLook != null)
                {
                    Destroy(cameraLook.gameObject);
                }

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
                if (getFirstSyncPacket)
                {
                    if (tank.tankOptions.Weapon != -1)
                        tank.weapons[tank.tankOptions.Weapon].transform.rotation = Quaternion.Lerp(tank.weapons[tank.tankOptions.Weapon].transform.rotation, tank.tankOptions.turretRotation, tank.weapons[tank.tankOptions.Weapon].GetRotSpeed() * 2f * Time.deltaTime);
                }
            }
            else
            {
                SuicideUpdate();
                FallUpdate();
                
                tank.tankOptions.turretRotation = tank.weapons[tank.tankOptions.Weapon].transform.rotation;
                
                CheckDeath();
            }
        }

        private void FallUpdate()
        {
            if (transform.position.y < -20)
            {
                tank.tankOptions.hp = 0.Obf();
            }
        }

        private void SuicideUpdate()
        {
            if (Input.GetKey(KeyCode.Delete))
            {
                timeToDestroy += Time.deltaTime;
                if (timeToDestroy > 2f)
                {
                    tank.tankOptions.hp = 0.Obf();
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
                if (tank.tankOptions.Hp > 0)
                {
                    tank.tankOptions.hp.ObfAdd(-(damage / Boosters.DefenceIncrease));

                    if (tank.tankOptions.Hp <= 0)
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
            
            
            try
            {
                CrazyEvents.Instance.GameplayStop();
            }
            catch (Exception)
            {
                // ignored
            }

            WebDataService.UserData.UserStatistics.AddDeath();
        }

        [PunRPC]
        public void KillRPC(string playerKiller, string playerKilled, int weapon)
        {
            if (PhotonNetwork.NickName == playerKiller)
            {
                WebDataService.TankData.AddXp(15);
                WebDataService.UserData.UserStatistics.AddKill();


                PhotonNetwork.LocalPlayer.CustomProperties["Exp"] = WebDataService.TankData.Exp;


                WebDataService.SaveStart();
            }

            KillsList.Instance.Create(playerKiller, playerKilled, weapon);
        }

        public void CheckDeath()
        {
            if (photonView.IsMine)
            {
                if (tank.tankOptions.Hp <= 0)
                {
                    AddDeath();
                    if (tank.tankOptions.Weapon == -1 || tank.tankOptions.Corpus == -1) return;
                    
                    var dead = PhotonNetwork.Instantiate("TankDead", transform.position, transform.rotation);
                    var rb = GetComponent<Rigidbody>();
                    dead.GetPhotonView().RPC("Set", RpcTarget.All, tank.tankOptions.Weapon, tank.tankOptions.Corpus, tank.tankOptions.turretRotation, rb.velocity, rb.mass, rb.drag, new Vector3(Random.Range(-5, 5), Random.Range(5, 20), Random.Range(-5, 10)), new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100)), true);
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
                var spawn = team.spawns[Random.Range(0, team.spawns.Length)];
                var pos = spawn.position;
                var rot = spawn.rotation;
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
                stream.SendNext(JsonUtility.ToJson(tank.tankOptions.UnObfuscate(), true));
                stream.SendNext(tank.bonuses.ToArray());
                stream.SendNext(tank.corpuses[tank.tankOptions.Corpus].RotatorData.CorpusRotator.GetTagetAngle());
            }
            else
            {
                tank.tankOptions = JsonUtility.FromJson<Tank.TankOptions>((string) stream.ReceiveNext()).Obfuscate();
                tank.bonuses = ((int[]) stream.ReceiveNext()).ToList();
                tank.corpuses[tank.tankOptions.Corpus].RotatorData.CorpusRotator.RotateCorpus((float)stream.ReceiveNext());
                getFirstSyncPacket = true;
            }
        }
    }
}
