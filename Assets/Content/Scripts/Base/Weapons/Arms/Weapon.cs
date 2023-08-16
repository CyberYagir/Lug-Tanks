using System;
using System.Collections.Generic;
using System.Linq;
using Base.Modifyers;
using Photon.Game;
using Photon.Pun;
using UnityEngine;
using Range = DG.DemiLib.Range;

namespace Base.Weapons.Arms
{
    public class Weapon : MonoBehaviour
    {
        [System.Serializable]
        public class DamageScaler
        {
            public enum DamageType
            {
                FullDamage,
                MiddleDamage,
                LowDamage,
                OutDamage
            }
            
            [SerializeField] private bool haveScaler;
            [SerializeField] private float minDistance;
            [SerializeField] private float middleDistance;
            [SerializeField] private float highDistance;


            public float GetDamage(float damage, float distance, out DamageType type)
            {
                if (!haveScaler)
                {
                    type = DamageType.FullDamage;
                    return damage;
                }


                if (distance <= minDistance)
                {
                    type = DamageType.FullDamage;
                    return damage;
                }

                if (distance <= middleDistance)
                {
                    type = DamageType.MiddleDamage;
                    return damage * 0.5f;
                }

                if (distance <= highDistance)
                {
                    type = DamageType.LowDamage;
                    return damage * 0.25f;
                }
                
                if (distance > highDistance)
                {
                    type = DamageType.OutDamage;
                    return damage * 0.1f;
                }
                
                type = DamageType.FullDamage;
                return damage;
            }
        }

        [System.Serializable]
        public class WeaponData
        {
            public enum InputType
            {
                Click, Hold
            }
            
            [SerializeField] private float energy = 100;
            [SerializeField] private float shotEnergy;
            [SerializeField] private float addEnergy;
            [Space]
            [SerializeField] private float cooldown;
            [SerializeField] private float damage;
            [SerializeField] private float rotSpeed;
            [SerializeField] private float maxDist;
            [SerializeField] private float upForce;

            [Space] 
            [SerializeField] private InputType inputType;
            [SerializeField] private bool addTime = true;
            [SerializeField] private bool waitToFull;
            [SerializeField] private bool multiplyDeltaTime;

            public bool MultiplyDeltaTime => multiplyDeltaTime;
            public bool WaitToFull => waitToFull;
            public bool AddTime => addTime;
            public float UpForce => upForce;
            public float MaxDist => maxDist;
            public float RotSpeed => rotSpeed;
            public float Damage => damage;
            public float Cooldown => cooldown;
            public float AddEnergy => addEnergy;
            public float ShotEnergy => shotEnergy;
            public float Energy => energy;


            public void AddToEnergy(float val) => energy += val;

            public void SetToEnergy(float val) => energy = val;


            public bool IsShoot()
            {
                switch (inputType)
                {
                    case InputType.Click:
                        return Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space);
                        break;
                    case InputType.Hold:
                        return Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Space);
                }

                return false;
            }
        }

        [System.Serializable]
        public class CameraViewData
        {
            [SerializeField] private float fov;
            [SerializeField] private Rect cameraRect = new Rect(0, 0, 1, 1);

            public Rect CameraRect => cameraRect;
            public float FieldOfView => fov;
        }

        [SerializeField] private WeaponData weaponData;
        [SerializeField] private CameraViewData cameraViewData;
        [SerializeField] private DamageScaler damageScaler;
        
        
        public Transform minPoint;
        public Transform shootPoint;



        protected float time = 999;
        private bool wait;
        private WeaponRotate rotate;
        private Rigidbody rb;
        private Player player;
        
        
        public Action shootAction;
        public Action updateAction;
        public Action notShootAction;

        public WeaponData WeaponValues => weaponData;


        public float GetRotSpeed() => weaponData.RotSpeed;
        public float GetCooldown() => weaponData.Cooldown;
        public float GetEnergy() => weaponData.Energy;
        public float GetTime() => time;
        public float GetShotEnergy() => weaponData.ShotEnergy;

        public float GetDamage(float distance) => damageScaler.GetDamage(weaponData.Damage, distance, out var type);        
        public float GetDamage(float distance, out DamageScaler.DamageType type) => damageScaler.GetDamage(weaponData.Damage, distance, out type);

        
        
        private void Awake()
        {
            rotate = transform.parent.GetComponent<WeaponRotate>();
            rb = transform.GetComponentInParent<Rigidbody>();
            player = GetComponentInParent<Player>();
        }

        public void Update()
        {
            rotate.rotateSpeed = GetRotSpeed();
            ConfigurateCamera();
            ClampMaxEnergy();
        
        
            if (weaponData.AddTime)
                time += Time.deltaTime * player.Boosters.FireRateIncrease;


            if (GameManager.IsOnPause) return;
        
        
            if (!weaponData.WaitToFull)
            {
                if (weaponData.IsShoot())
                {
                    if (time >= GetCooldown())
                    {
                        if (GetEnergy() >= GetShotEnergy())
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
                    if (GetEnergy() == 100)
                    {
                        wait = false;
                    }
                }
                if (weaponData.IsShoot() && !wait)
                {
                    if (time >= GetCooldown())
                    {
                        if (GetEnergy() >= GetShotEnergy() && !wait)
                        {
                            AddPhysics();
                            
                            shootAction.Invoke();
                            time = 0;
                            weaponData.AddToEnergy(-GetShotEnergy() * (weaponData.MultiplyDeltaTime ?  Time.deltaTime : 1));
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

        private void ConfigurateCamera()
        {
            var cam = WeaponRotate.CameraInstance;
            if (cam != null)
            {
                cam.fieldOfView = cameraViewData.FieldOfView;
                cam.rect = cameraViewData.CameraRect;
            }
        }

        protected void AddPhysics()
        {
            rb.AddTorque(-rotate.transform.right * weaponData.UpForce);
            rb.AddForce(-rotate.transform.forward * weaponData.UpForce * 2);
        }

        protected virtual void ShootProcess()
        {
            AddPhysics();
            shootAction.Invoke();
            time = 0;
            weaponData.AddToEnergy(-GetShotEnergy());
        }

        protected virtual void ClampMaxEnergy()
        {
            if (GetEnergy() < 100)
            {
                AddUpdateEnergy();
            }
            else
                weaponData.SetToEnergy(100);
        }

        protected void AddUpdateEnergy()
        {
            weaponData.AddToEnergy(weaponData.AddEnergy * Time.deltaTime * player.Boosters.FireRateIncrease);
        }

        [System.Serializable]
        public class Target {
            public GameObject enemy;
            public Transform point;
            public float angle;
        }

        private List<GameObject> viewedEnemies = new List<GameObject>(20);
        private List<Transform> viewedEnemiesPoints = new List<Transform>(20);
        private List<Target> targets = new List<Target>(20);
        public List<Target> Enemies(Transform shootPoint)
        {
            viewedEnemies.Clear();
            viewedEnemiesPoints.Clear();
            var enemies = GameManager.Instance.GetEnemies();
            if (WeaponRotate.CameraInstance == null) return new List<Target>();
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] == null) continue;
                if (WeaponRotate.IsVisible(enemies[i].gameObject))
                {
                    RaycastHit hit;
                    var t = enemies[i];
                
                    if (t.tankOptions.Team != 0 && (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"] == t.tankOptions.Team) continue;
                    bool finded = false;
                    for (int u = 0; u < t.corpuses[t.tankOptions.Corpus].HitPoints.Count; u++)
                    {
                        if (Physics.Raycast(shootPoint.position, t.corpuses[t.tankOptions.Corpus].HitPoints[u].position - shootPoint.position, out hit))
                        {
                            if (hit.collider != null)
                            {
                                if (hit.transform == enemies[i].transform)
                                {
                                    finded = true;
                                    viewedEnemies.Add(enemies[i].gameObject);
                                    viewedEnemiesPoints.Add(t.corpuses[t.tankOptions.Corpus].HitPoints[u]);
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
                                    viewedEnemies.Add(enemies[i].gameObject);
                                }
                            }
                        }
                    }
                }
            }
            targets.Clear();
            for (int i = 0; i < viewedEnemies.Count; i++)
            {
                bool add = true;
                if (weaponData.MaxDist != 0)
                {
                    if (Vector3.Distance(shootPoint.transform.position, viewedEnemies[i].gameObject.transform.position) > weaponData.MaxDist)
                    {
                        add = false;
                    }
                }
                if (add)
                    targets.Add(new Target() { angle = Vector3.Angle(shootPoint.forward, viewedEnemies[i].transform.position - shootPoint.position), enemy = viewedEnemies[i], point = viewedEnemiesPoints[i]});
            }
            return targets.OrderBy(x => x.angle).ToList();
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