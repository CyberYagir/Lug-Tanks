using Base.Controller;
using Photon.Pun;
using UnityEngine;

namespace Base.Weapons.Arms
{
    public class Weapon1 : Weapon
    {
        [SerializeField] private GameObject particles;
        private void Start()
        {
        
            shootAction += () =>
            {
                var targets = Enemies(shootPoint);
                if (targets.Count == 0)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(shootPoint.transform.position, shootPoint.forward, out hit))
                    {
                        var tank = GetComponentInParent<Base.Controller.Tank>();
                        if (tank.Team == Tank.TankTeam.Enemy)
                        {
                            hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, (float)damage, (string)PhotonNetwork.NickName, tank.tankOptions.Weapon);
                        }
                        PhotonNetwork.Instantiate(particles.name, hit.point, Quaternion.identity).GetComponent<ParticleDestroy>().StartEnum();
                    }
                }
                else
                {
                    RaycastHit hit;
                    if (Physics.Raycast(shootPoint.transform.position, targets[0].enemy.gameObject.transform.position - shootPoint.transform.position, out hit))
                    {
                        Base.Controller.Tank.SetLastPlayer(targets[0].enemy.gameObject);
                        targets[0].enemy.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, (float)damage, (string)PhotonNetwork.NickName, GetComponentInParent<Base.Controller.Tank>().tankOptions.Weapon);
                        PhotonNetwork.Instantiate(particles.name, targets[0].enemy.gameObject.transform.position, Quaternion.identity).GetComponent<ParticleDestroy>().StartEnum();
                    }
                }
            };
        }

        public GameObject GetParticles()
        {
            return particles;
        }
    }
}
