using System;
using System.Collections;
using Base.Controller;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace Base.Weapons.Arms
{
    public class Weapon2 : Weapon
    {
        public GameObject line;
        public bool playAnimation;
        public bool canShoot;
        
        private void Start()
        {
            canShoot = true;
            shootAction += () =>
            {
                canShoot = false;
                StartCoroutine(ShootDelay());
            };
        }

        IEnumerator ShootDelay()
        {
            while (GetEnergy() > 0.01f)
            {
                WeaponValues.AddToEnergy(-Time.deltaTime * 100f);
                yield return null;
            }

            WeaponValues.SetToEnergy(0);
            playAnimation = true;
            AddPhysics();
            Shoot();
            canShoot = true;
            
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            
            playAnimation = false;
        }

        protected override void ClampMaxEnergy()
        {
            if (GetEnergy() < 100)
            {
                if (canShoot)
                    AddUpdateEnergy();
            }
            else
                WeaponValues.SetToEnergy(100);
        }

        protected override void ShootProcess()
        {
            shootAction.Invoke();
            time = 0;
        }


        private void Shoot()
        {
            var targets = Enemies(shootPoint);
            if (targets.Count == 0)
            {
                RaycastHit hit;
                if (Physics.Raycast(shootPoint.transform.position, shootPoint.forward, out hit))
                {
                    var tank = GetComponentInParent<Tank>();
                    if (tank.Team == Tank.TankTeam.Enemy)
                        hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, (float) GetDamage(hit.distance), (string) PhotonNetwork.NickName, tank.tankOptions.Weapon);
                    CreateLine(shootPoint.transform.position, hit.point);
                    
                }
                else
                {
                    CreateLine(shootPoint.transform.position, shootPoint.transform.position + shootPoint.forward * 1000f);
                }
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(shootPoint.transform.position, targets[0].point.position - shootPoint.transform.position, out hit))
                {
                    targets[0].enemy.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, (float) GetDamage(hit.distance), (string) PhotonNetwork.NickName, GetComponentInParent<Tank>().tankOptions.Weapon);
                    Tank.SetLastPlayer(targets[0].enemy.gameObject);
                    CreateLine(shootPoint.transform.position, hit.point);
                }
            }
        }

        public void CreateLine(Vector3 pos1, Vector3 pos2)
        {
            Debug.DrawLine(shootPoint.transform.position, shootPoint.transform.position + shootPoint.forward * 1000, Color.magenta, Single.MaxValue);
            var l = PhotonNetwork.Instantiate(line.gameObject.name, Vector2.zero, Quaternion.identity).GetPhotonView();
            l.TransferOwnership(PhotonNetwork.CurrentRoom.MasterClientId);
            l.RPC("SetLinePoses", RpcTarget.All, pos1, pos2);
            l.GetComponent<LineScript>().Start_Destroy();
        }
    }
}
