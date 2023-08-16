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
                animator.SetBool(IsShoot, GetEnergy() > GetShotEnergy());
                if (GetEnergy() < GetShotEnergy())
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
                            targets[0].enemy.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, GetDamage(0) * Time.deltaTime, (string) PhotonNetwork.NickName, GetComponentInParent<Tank>().tankOptions.Weapon);
                            Tank.SetLastPlayer(targets[0].enemy.gameObject);
                        }
                    }
                }
            };
            notShootAction += () => { animator.SetBool(IsShoot, false); };
        }
    }
}