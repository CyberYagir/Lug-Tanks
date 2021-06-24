using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponAnimate: MonoBehaviour
{

}
public class Weapon1Animate : WeaponAnimate
{
    public Weapon weapon;
    public Animator animator;
    public Transform particlesPoint;
    public Transform decalPoint;
    public GameObject decal;
    public AudioSource audioSource;
    private void Update()
    {
        animator.SetBool("IsShoot", weapon.getTime() < weapon.getCooldown());
    }
    public void SpawnParticles()
    {
        var p = Instantiate(GetComponent<Weapon1>().particles, particlesPoint.transform.position, Quaternion.identity);
        p.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        audioSource.Play();
        Destroy(p.gameObject, 1.5f);
    }
    public void SpawnDecal()
    {
        Instantiate(decal, decalPoint.transform.position, decalPoint.rotation).GetComponent<Rigidbody>().AddRelativeForce(Vector3.right * 100f);
    }
}
