using System;
using System.Collections.Generic;
using Base.Weapons.Arms;
using Content.Scripts.Anticheat;
using Photon.Game;
using Photon.Pun;
using Scriptable;
using UnityEngine;
using UnityEngine.Events;

namespace Base.Controller
{
    public class Tank : MonoBehaviour
    {
        [System.Serializable]
        public class TankOptions {
            public float hp;
            public int corpus;
            public int weapon;
            public int team = 0;
            public Quaternion turretRotation;
            
            [NonSerialized]
            public UnityEvent OnChangeTank = new UnityEvent();

            public int Team => team.ObfUn();

            public int Weapon => weapon.ObfUn();

            public int Corpus => corpus.ObfUn();

            public float Hp => hp.ObfUn();

            public void ChangeCorpus(int corpus)
            {
                this.corpus = corpus.Obf();
                OnChangeTank.Invoke();
            }
            
            public void ChangeWeapon(int weapon)
            {
                this.weapon = weapon.Obf();
                OnChangeTank.Invoke();
            }



            public TankOptions Obfuscate()
            {
                return new TankOptions()
                {
                    corpus = corpus.Obf(),
                    weapon = weapon.Obf(),
                    hp = hp.Obf(),
                    team = team.Obf(),
                    OnChangeTank = OnChangeTank,
                    turretRotation = turretRotation
                };
            }
            
            public TankOptions UnObfuscate()
            {
                return new TankOptions()
                {
                    corpus = corpus.ObfUn(),
                    weapon = weapon.ObfUn(),
                    hp = hp.ObfUn(),
                    team = team.ObfUn(),
                    OnChangeTank = OnChangeTank,
                    turretRotation = turretRotation
                };
            }
        }

        public enum TankTeam
        {
            Player, Enemy   
        }
        
        public static GameObject lastPlayer;
        public static float lastPlayerClearTime;
    
        public TankOptions tankOptions;
        public List<Corpus> corpuses;
        public List<Weapon> weapons;
        public List<int> bonuses;
        public Rigidbody rb;
        public Transform damageDisplayPoint;
        

        [SerializeField] private TankTeam tankTeam = TankTeam.Player;
        private Player player;

        private static readonly int MainTex = Shader.PropertyToID("_BaseMap");

        public TankTeam Team => tankTeam;

        
        public void Init(Player player)
        {
            this.player = player;
            
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Ignore Raycast"), LayerMask.NameToLayer("NoCollisions"));
            
            SetTeamsColors();

            if (GameManager.Instance != null)
            {
                transform.parent = GameManager.Instance.PlayersHolder;
            }
        }

        private void SetTeamsColors()
        {
            if (PhotonNetwork.InRoom)
            {
                var team = (int) gameObject.GetPhotonView().Owner.CustomProperties["Team"];
                var teamsData = player.GameData.TeamsData;
                
                for (int i = 0; i < corpuses.Count; i++)
                {
                    corpuses[i].SetTexture(teamsData.GetTeam(team).Texture);
                }

                for (int i = 0; i < weapons.Count; i++)
                {
                    weapons[i].GetComponent<Renderer>().material.SetTexture(MainTex, teamsData.GetTeam(team).Texture);
                }
            }
        }
        
        public void ChangeTankLocalTeam(TankTeam team) => tankTeam = team;

        public static void SetLastPlayer(GameObject obj)
        {
            lastPlayer = obj;
            lastPlayerClearTime = 0;
        }
    

        void SetEquip()
        {
            var playerCorpus = tankOptions.Corpus;
            var playerWeapon = tankOptions.Weapon;
            for (int i = 0; i < corpuses.Count; i++)
            {
                corpuses[i].ActiveCorpus(i == playerCorpus, player.photonView == null || player.photonView.IsMine);
                if (i == playerCorpus)
                {
                    rb.centerOfMass = corpuses[i].CenterOfMass;
                }
            }
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].gameObject.SetActive(i == playerWeapon);
                if (i == playerWeapon)
                {
                    weapons[i].transform.position = corpuses[playerCorpus].WeaponPoint.transform.position;
                }
            }
        }
        private void Update()
        {
            DisableUI();

            if (gameObject.GetPhotonView() == null || gameObject.GetPhotonView().IsMine)
            {
                lastPlayerClearTime += Time.deltaTime;
                SetEquip();
            }
            else
            {
                if (tankOptions.Corpus != -1)
                {
                    SetEquip();
                    weapons[tankOptions.Weapon].transform.position = corpuses[tankOptions.Corpus].WeaponPoint.transform.position;
                }
            }
        }

        private void DisableUI()
        {
            if (Input.GetKey(KeyCode.F1))
            {
                foreach (var n in FindObjectsOfType<Canvas>()) n.gameObject.SetActive(false);
            }
        }

        private void OnDrawGizmos()
        {
            for (int i = 0; i < corpuses.Count; i++)
            {
                if (corpuses[i].IsActive)
                {
                    if (corpuses[i].WeaponPoint != null)
                    {
                        Gizmos.DrawWireSphere(corpuses[i].WeaponPoint.position, .2f);
                    }
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireSphere(transform.TransformPoint(corpuses[i].CenterOfMass), .2f);
                }
            }
        }

        private void OnDisable()
        {
            lastPlayer = null;
        }
    }
}
