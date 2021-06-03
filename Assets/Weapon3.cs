using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon3 : Weapon
{
    public Animator animator;
    
    private void Start()
    {
        shootAction += () =>
        {
            animator.SetBool("IsShoot", energy > shot_energy);
            if (energy < shot_energy)
            {
                animator.SetBool("IsShoot", false);
            }
            else
            {
                var targets = Enemies(shootPoint);
                if (targets.Count != 0)
                {
                    targets[0].enemy.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, (float)damage*Time.deltaTime, (string)PhotonNetwork.NickName, GetComponentInParent<Tank>().tankOptions.weapon);
                }
            }
        };
        notShootAction += () =>
        {
            animator.SetBool("IsShoot", false);
        };
    }
}
