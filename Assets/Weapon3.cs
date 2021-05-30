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
                print(energy < shot_energy);
                animator.SetBool("IsShoot", false);
            }
        };
        notShootAction += () =>
        {
            animator.SetBool("IsShoot", false);
        };
    }
}
