using System;
using Base.Controller;
using Photon.Pun;
using UnityEngine;

namespace Base.Weapons.Arms
{
    public class Weapon3 : Weapon
    {
        public Animator animator;
        private static readonly int IsShoot = Animator.StringToHash("IsShoot");

        private void Start()
        {
            shootAction += () =>
            {
                animator.SetBool(IsShoot, energy > shot_energy);
                if (energy < shot_energy)
                {
                    animator.SetBool(IsShoot, false);
                }
                else
                {
                    var targets = Enemies(shootPoint);
                    if (targets.Count != 0)
                    {
                        var tank = targets[0].enemy.GetComponent<Tank>();
                        if (tank.Team == Tank.TankTeam.Enemy)
                        {
                            targets[0].enemy.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, (float) damage * Time.deltaTime, (string) PhotonNetwork.NickName, GetComponentInParent<Tank>().tankOptions.weapon);
                            Tank.SetLastPlayer(targets[0].enemy.gameObject);
                        }
                    }
                }
            };
            notShootAction += () => { animator.SetBool(IsShoot, false); };
        }
    }
}