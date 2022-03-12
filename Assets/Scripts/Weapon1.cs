﻿using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using UnityEngine;

public class Weapon : MonoBehaviour {

    [SerializeField]
    protected float energy = 100, shot_energy, energy_add, cooldown, damage, rotSpeed, time, fov, maxDist, upForce;
    public Rect cameraRect = new Rect(0, 0, 1, 1);
    public bool addTime = true, waitTofull;
    public bool multiplyDeltaTime;
    bool wait;
    public Transform minPoint;
    public Transform shootPoint;
    public Action shootAction;
    public Action updateAction;
    public Action notShootAction;

    public float getRotSpeed()
    {
        return rotSpeed;
    }
    public float getCooldown()
    {
        return cooldown;
    }
    public float getEnergy()
    {
        return energy;
    }
    public float getTime()
    {
        return time;
    }
    public float getShotEnergy()
    {
        return shot_energy;
    }
    public void Update()
    {
        transform.parent.GetComponent<WeaponRotate>().rotateSpeed = rotSpeed;
        if (WeaponRotate.shootCam != null)
        WeaponRotate.shootCam.fieldOfView = fov;
        WeaponRotate.shootCam.rect = cameraRect;
        if (energy < 100)
        { energy += energy_add * Time.deltaTime * TankModificators.fireRateIncrease; }
        else energy = 100;
        if (addTime)
        time += Time.deltaTime * TankModificators.fireRateIncrease;


        if (GameManager.pause) return;
        if (!waitTofull)
        {
            if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Space))
            {
                if (time >= cooldown)
                {
                    if (energy >= shot_energy)
                    {
                        shootAction.Invoke();
                        transform.GetComponentInParent<Rigidbody>().AddTorque(-FindObjectOfType<WeaponRotate>().transform.right * upForce);
                        transform.GetComponentInParent<Rigidbody>().AddForce(-FindObjectOfType<WeaponRotate>().transform.forward * upForce * 2);
                        time = 0;
                        energy -= shot_energy;
                    }
                }
            }
            else
            {
                if (notShootAction != null)
                {
                    notShootAction.Invoke();
                }
            }
        }
        else
        {
            if (wait)
            {
                if (energy == 100)
                {
                    wait = false;
                }
            }
            if (Input.GetKey(KeyCode.Mouse0) && !wait)
            {
                if (time >= cooldown)
                {
                    if (energy >= shot_energy && !wait)
                    {
                        transform.GetComponentInParent<Rigidbody>().AddTorque(-FindObjectOfType<WeaponRotate>().transform.right * upForce);
                        transform.GetComponentInParent<Rigidbody>().AddForce(-FindObjectOfType<WeaponRotate>().transform.forward * upForce * 2);
                        shootAction.Invoke();
                        time = 0;
                        energy -= shot_energy * (multiplyDeltaTime ?  Time.deltaTime : 1);
                    }
                    else
                    {
                        wait = true;
                    }
                }
            }
            else
            {
                if (notShootAction != null)
                {
                    notShootAction.Invoke();
                }
            }
        }
        if (updateAction != null)
            updateAction.Invoke();
    }
    [System.Serializable]
    public class Target {
        public GameObject enemy;
        public float angle;
    }
    
    public List<Target> Enemies(Transform shootPoint)
    {
        var ret = new List<GameObject>();
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (WeaponRotate.shootCam == null) return new List<Target>();
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] == null) continue;
            if (WeaponRotate.IsVisible(enemies[i]))
            {
                RaycastHit hit;
                var t = enemies[i].GetComponent<Tank>();
                if (t.tankOptions.team != 0 && (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"] == t.tankOptions.team) continue;
                bool finded = false;
                for (int u = 0; u < t.corpuses[t.tankOptions.corpus].hitPoints.Length; u++)
                {
                    if (Physics.Raycast(shootPoint.position, t.corpuses[t.tankOptions.corpus].hitPoints[u].position - shootPoint.position, out hit))
                    {
                        if (hit.collider != null)
                        {
                            if (hit.transform == enemies[i].transform)
                            {
                                finded = true;
                                ret.Add(enemies[i]);
                                break;
                            }
                        }
                    }
                }                
                if (!finded){
                    if (Physics.Raycast(shootPoint.transform.position, enemies[i].transform.position - shootPoint.position, out hit))
                    {
                        if (hit.collider != null)
                        {
                            if (hit.transform == enemies[i].transform)
                            {
                                ret.Add(enemies[i]);
                            }
                        }
                    }
                }
            }
        }
        var trgs = new List<Target>();
        for (int i = 0; i < ret.Count; i++)
        {
            bool add = true;
            if (maxDist != 0)
            {
                if (Vector3.Distance(shootPoint.transform.position, ret[i].gameObject.transform.position) > maxDist)
                {
                    add = false;
                }
            }
            if (add)
                trgs.Add(new Target() { angle = Vector3.Angle(shootPoint.forward, ret[i].transform.position - shootPoint.position), enemy = ret[i] });
        }
        return trgs.OrderBy(x => x.angle).ToList();
    }
    public bool TeamCheck(GameObject hit)
    {
        return (int)hit.GetPhotonView().Owner.CustomProperties["Team"] == 0 || (int)hit.GetPhotonView().Owner.CustomProperties["Team"] != (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
    }
}


public class Weapon1 : Weapon
{
    public GameObject particles;
    //public List<Target> targets;
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
                    if (hit.transform.tag == "Enemy" && TeamCheck(hit.transform.gameObject))
                    {
                        hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, (float)damage, (string)PhotonNetwork.NickName, GetComponentInParent<Tank>().tankOptions.weapon);
                    }
                    PhotonNetwork.Instantiate(particles.name, hit.point, Quaternion.identity).GetComponent<ParticleDestroy>().StartEnum();
                }
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(shootPoint.transform.position, targets[0].enemy.gameObject.transform.position - shootPoint.transform.position, out hit))
                {
                    Tank.SetLastPlayer(targets[0].enemy.gameObject);
                    targets[0].enemy.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, (float)damage, (string)PhotonNetwork.NickName, GetComponentInParent<Tank>().tankOptions.weapon);
                    PhotonNetwork.Instantiate(particles.name, targets[0].enemy.gameObject.transform.position, Quaternion.identity).GetComponent<ParticleDestroy>().StartEnum();
                }
            }
        };
    }
}