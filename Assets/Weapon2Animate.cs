﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon2Animate : WeaponAnimate
{
    public Weapon weapon;
    public Animator animator;
    public GameObject sphere;
    public Transform spherePoint;
    public AudioSource audioSource;
    private void Update()
    {
        animator.SetBool("IsShoot", weapon.getTime() < weapon.getCooldown());
    }

    public void SpawnSphere()
    {
        audioSource.Play();
        Destroy(Instantiate(sphere.gameObject, spherePoint.transform.position, Quaternion.identity), 2f);
    }
}
