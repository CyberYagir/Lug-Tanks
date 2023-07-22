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
            while (energy > 0.01f)
            {
                energy -= Time.deltaTime * 100f;
                yield return null;
            }

            energy = 0;
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
            if (energy < 100)
            {
                if (canShoot)
                    AddEnergy();
            }
            else
                energy = 100;
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
                    var tank = GetComponentInParent<Base.Controller.Tank>();
                    if (tank.Team == Tank.TankTeam.Enemy)
                        hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, (float) damage, (string) PhotonNetwork.NickName, tank.tankOptions.weapon);
                    CreateLine(shootPoint.transform.position, hit.point);
                }
                else
                {
                    CreateLine(shootPoint.transform.position, shootPoint.forward * 1000f);
                }
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(shootPoint.transform.position, targets[0].enemy.gameObject.transform.position - shootPoint.transform.position, out hit))
                {
                    targets[0].enemy.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, (float) damage, (string) PhotonNetwork.NickName, GetComponentInParent<Base.Controller.Tank>().tankOptions.weapon);
                    Base.Controller.Tank.SetLastPlayer(targets[0].enemy.gameObject);
                    CreateLine(shootPoint.transform.position, targets[0].enemy.transform.position);
                }
            }
        }

        public void CreateLine(Vector3 pos1, Vector3 pos2)
        {
            var l = PhotonNetwork.Instantiate(line.gameObject.name, Vector2.zero, Quaternion.identity).GetPhotonView();
            l.TransferOwnership(PhotonNetwork.CurrentRoom.MasterClientId);
            l.RPC("SetLinePoses", RpcTarget.All, pos1, pos2);
            l.GetComponent<LineScript>().Start_Destroy();
        }
    }
}
