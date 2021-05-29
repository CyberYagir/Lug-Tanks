using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon3 : Weapon
{
    public Animator animator;
    public bool waitToFull;

    private void Start()
    {
        shootAction += () =>
        {
            animator.SetBool("IsShoot", energy > shot_energy);
            if (energy < shot_energy)
            {
                animator.SetBool("IsShoot", false);
                waitToFull = true;
            }
        };
        notShootAction += () =>
        {
            animator.SetBool("IsShoot", false);
        };
    }
}
