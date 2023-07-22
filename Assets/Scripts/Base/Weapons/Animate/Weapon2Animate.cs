using System;
using Base.Weapons.Arms;
using UnityEngine;

namespace Base.Weapons.Animate
{
    public class Weapon2Animate : WeaponAnimate
    {
        public Weapon2 weapon;
        public Animator animator;
        public GameObject sphere;
        public Transform spherePoint;
        public AudioSource audioSource;
        private static readonly int IsShoot = Animator.StringToHash("IsShoot");
        private static readonly int IsCharge = Animator.StringToHash("IsCharge");

        private void Awake()
        {
        }

        private void Update()
        {
            animator.SetBool(IsCharge, !weapon.canShoot && weapon.GetEnergy() < 100);
            animator.SetBool(IsShoot, weapon.playAnimation);
        }

        public void SpawnSphere()
        {
            audioSource.Play();
            Destroy(Instantiate(sphere.gameObject, spherePoint.transform.position, Quaternion.identity), 2f);
        }
    }
}
