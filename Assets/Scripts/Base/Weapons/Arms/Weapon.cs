using System;
using System.Collections.Generic;
using System.Linq;
using Base.Modifyers;
using Photon.Game;
using Photon.Pun;
using UnityEngine;

namespace Base.Weapons.Arms
{
    public class Weapon : MonoBehaviour {

        [SerializeField]
        protected float energy = 100, shot_energy, energy_add, cooldown, damage, rotSpeed, time, fov, maxDist, upForce;
        public Rect cameraRect = new Rect(0, 0, 1, 1);
        public bool addTime = true, waitTofull;
        public bool multiplyDeltaTime;
        public Transform minPoint;
        public Transform shootPoint;
        public Action shootAction;
        public Action updateAction;
        public Action notShootAction;


        private bool wait;
        private WeaponRotate rotate;
        private Rigidbody rb;
        private Player player;
        
        public float GetRotSpeed() => rotSpeed;
        public float GetCooldown() => cooldown;
        public float GetEnergy() => energy;
        public float GetTime() => time;
        public float GetShotEnergy() => shot_energy;
        
        
        
        private void Awake()
        {
            rotate = transform.parent.GetComponent<WeaponRotate>();
            rb = transform.GetComponentInParent<Rigidbody>();
            player = GetComponentInParent<Player>();
        }

        public void Update()
        {
            rotate.rotateSpeed = rotSpeed;
            var cam = WeaponRotate.CameraInstance;
            if (cam != null)
            {
                cam.fieldOfView = fov;
                cam.rect = cameraRect;
            }

            ClampMaxEnergy();
        
        
            if (addTime)
                time += Time.deltaTime * player.Boosters.FireRateIncrease;


            if (GameManager.IsOnPause) return;
        
        
            if (!waitTofull)
            {
                if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Space))
                {
                    if (time >= cooldown)
                    {
                        if (energy >= shot_energy)
                        {
                            ShootProcess();
                        }
                    }
                }
                else
                {
                    if (notShootAction != null)
                    {
                        notShootAction.Invoke();
                    }
                }
            }
            else
            {
                if (wait)
                {
                    if (energy == 100)
                    {
                        wait = false;
                    }
                }
                if (Input.GetKey(KeyCode.Mouse0) && !wait)
                {
                    if (time >= cooldown)
                    {
                        if (energy >= shot_energy && !wait)
                        {
                            AddPhysics();
                            
                            shootAction.Invoke();
                            time = 0;
                            energy -= shot_energy * (multiplyDeltaTime ?  Time.deltaTime : 1);
                        }
                        else
                        {
                            wait = true;
                        }
                    }
                }
                else
                {
                    if (notShootAction != null)
                    {
                        notShootAction.Invoke();
                    }
                }
            }
            if (updateAction != null)
                updateAction.Invoke();
        }

        protected void AddPhysics()
        {
            rb.AddTorque(-rotate.transform.right * upForce);
            rb.AddForce(-rotate.transform.forward * upForce * 2);
        }

        protected virtual void ShootProcess()
        {
            AddPhysics();
            shootAction.Invoke();
            time = 0;
            energy -= shot_energy;
        }

        protected virtual void ClampMaxEnergy()
        {
            if (energy < 100)
            {
                AddEnergy();
            }
            else
                energy = 100;
        }

        protected void AddEnergy()
        {
            energy += energy_add * Time.deltaTime * player.Boosters.FireRateIncrease;
        }

        [System.Serializable]
        public class Target {
            public GameObject enemy;
            public float angle;
        }
    
        List<GameObject> ret = new List<GameObject>(20);
        public List<Target> Enemies(Transform shootPoint)
        {
            ret.Clear();
            var enemies = GameManager.Instance.GetEnemies();
            if (WeaponRotate.CameraInstance == null) return new List<Target>();
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] == null) continue;
                if (WeaponRotate.IsVisible(enemies[i].gameObject))
                {
                    RaycastHit hit;
                    var t = enemies[i];
                
                    if (t.tankOptions.team != 0 && (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"] == t.tankOptions.team) continue;
                    bool finded = false;
                    for (int u = 0; u < t.corpuses[t.tankOptions.corpus].hitPoints.Length; u++)
                    {
                        if (Physics.Raycast(shootPoint.position, t.corpuses[t.tankOptions.corpus].hitPoints[u].position - shootPoint.position, out hit))
                        {
                            if (hit.collider != null)
                            {
                                if (hit.transform == enemies[i].transform)
                                {
                                    finded = true;
                                    ret.Add(enemies[i].gameObject);
                                    break;
                                }
                            }
                        }
                    }                
                    if (!finded){
                        if (Physics.Raycast(shootPoint.transform.position, enemies[i].transform.position - shootPoint.position, out hit))
                        {
                            if (hit.collider != null)
                            {
                                if (hit.transform == enemies[i].transform)
                                {
                                    ret.Add(enemies[i].gameObject);
                                }
                            }
                        }
                    }
                }
            }
            var trgs = new List<Target>();
            for (int i = 0; i < ret.Count; i++)
            {
                bool add = true;
                if (maxDist != 0)
                {
                    if (Vector3.Distance(shootPoint.transform.position, ret[i].gameObject.transform.position) > maxDist)
                    {
                        add = false;
                    }
                }
                if (add)
                    trgs.Add(new Target() { angle = Vector3.Angle(shootPoint.forward, ret[i].transform.position - shootPoint.position), enemy = ret[i] });
            }
            return trgs.OrderBy(x => x.angle).ToList();
        }

        public static bool IsEnemyTeam(GameObject hit)
        {
            return IsEnemyTeam(hit.GetPhotonView());
        }
        public static bool IsEnemyTeam(PhotonView view)
        {
            var pw = view;
            if (pw != null)
            {
                return (int) pw.Owner.CustomProperties["Team"] == 0 || (int) pw.Owner.CustomProperties["Team"] != (int) PhotonNetwork.LocalPlayer.CustomProperties["Team"];
            }

            return false;
        }
    }
}