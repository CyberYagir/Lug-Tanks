﻿using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Photon
{
    public class DeadTank : MonoBehaviour
    {
        [SerializeField] private GameObject[] weapons, corpuses, shootPoints;
        [SerializeField] private Quaternion rot;
        [SerializeField] private GameObject particles;
        [SerializeField] private List<Renderer> renderers;
        [SerializeField] private Material material;
        [SerializeField] private Material clonedMat;


        private float emission = 1;
        private static readonly int ProgressHash = Shader.PropertyToID("_Progress");

        public int weapon { get; private set; }
        public int corpus { get; private set; }
    
    
        public Quaternion GetRot() => rot;

        private void Awake()
        {
            clonedMat = Instantiate(material);

            foreach (var rn in renderers)
            {
                var mats = rn.materials;
                for (int i = 0; i < mats.Length; i++)
                {
                    mats[i] = clonedMat;
                }
                rn.materials = mats;
            }
        }

        private void Update()
        {
            emission -= Time.deltaTime/2f;
            clonedMat.SetFloat(ProgressHash, Mathf.Clamp01(emission));

        }

        public void StartDestroy()
        {
            StartCoroutine(wait());
        }
        IEnumerator wait()
        {
            yield return new WaitForSeconds(5);
            PhotonNetwork.Destroy(gameObject);
        }

        [PunRPC]
        public void Set(int w, int c, Quaternion r, Vector3 vel, float mass, float drag, Vector3 force, Vector3 torque, bool spawnp = true)
        {
            weapon = w;
            corpus = c;
            rot = r;
            if (spawnp)
            {
                particles.transform.parent = null;
                Destroy(particles, 2);
            }
            else
            {
                Destroy(particles);
            }

            weapons[w].SetActive(true);
            weapons[w].transform.position = shootPoints[c].transform.position;
            weapons[w].transform.rotation = r;
            corpuses[c].SetActive(true);
            var wRb = weapons[w].AddComponent<Rigidbody>();
            
            var rb = GetComponent<Rigidbody>();
            rb.mass = mass;
            rb.drag = drag;
            rb.velocity = vel;
            rb.AddRelativeForce(force, ForceMode.Impulse);
            rb.AddRelativeTorque(torque);

            wRb.velocity = rb.velocity * 2f;
            wRb.angularVelocity = Random.insideUnitSphere * 100f;
            wRb.drag = rb.drag;
        }
    }
}
