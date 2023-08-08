using System;
using System.Collections.Generic;
using Misc;
using UnityEngine;

namespace Base.Controller
{
    public class Track : MonoBehaviour
    {
        [SerializeField] private List<GameObject> objects;
        private ParticleSystem particles;
        private ParticleSystem.MinMaxGradient particlesStartColor;
        private bool particlesDecreased = false;
        
        private void Awake()
        {
            particles = GetComponentInChildren<ParticleSystem>();
            particlesStartColor = particles.main.startColor;
            DisableParticles();
        }

        public void DecreaseParticlesCount()
        {
            if (!particlesDecreased)
            {
                if (particles == null)
                {
                    particles = GetComponentInChildren<ParticleSystem>();
                }

                var em = particles.emission;
                em.rateOverDistanceMultiplier /= 3f;

                particlesDecreased = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var isGround = other.transform.CompareTag("Ground");
            if (isGround || other.transform.CompareTag("Enemy"))
            {
                objects.Add(other.gameObject);

                UpdateParticlesColor();
            }
        }

        private void UpdateParticlesColor()
        {
            var emission = particles.emission;
            emission.enabled = true;
            var main = particles.main;
            var rn = objects[objects.Count - 1].GetComponent<GroundColor>();
            if (rn)
            {
                var cl1 = rn.Color;
                cl1.a = 0.5f;
                var cl2 = cl1 * 0.75f;
                cl2.a = 0.5f;

                var color = main.startColor;
                color.colorMax = cl1;
                color.colorMin = cl2;

                main.startColor = color;
            }
            else
            {
                main.startColor = particlesStartColor;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (objects.Contains(other.gameObject))
            {
                objects.Remove(other.gameObject);
            }

            if (GetCount() == 0)
            {
                DisableParticles();
            }
            else
            {
                UpdateParticlesColor();
            }
        }

        private void DisableParticles()
        {
            var emission = particles.emission;
            emission.enabled = false;
        }

        public int GetCount() => objects.Count;
    }
}
