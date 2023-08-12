using System;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Controller
{
    [System.Serializable]
    public class Corpus : TankPart
    {
        [System.Serializable]
        public class Rotator
        {
            [SerializeField] private Transform rotator;
            [SerializeField] private float angle;
            [SerializeField] private float springSpeed;
            [SerializeField] private CorpusRotator corpusRotator;
            public float SpringSpeed => springSpeed;
            public float Angle => angle;
            public Transform RotateTransform => rotator;

            public CorpusRotator CorpusRotator => corpusRotator;
        }
        
        [SerializeField] private float hp;
        [SerializeField] private float speed;
        [SerializeField] private float rotSpeed;
        [SerializeField] private Transform weaponPoint;
        [SerializeField] private Vector3 centerOfMass;
        [SerializeField] private List<Transform> hitPoints;
        [SerializeField] private List<Track> tracks;
        [SerializeField] private List<Collider> colliders;
        [SerializeField] private Rotator rotator;

        public Vector3 CenterOfMass => centerOfMass;
        public Transform WeaponPoint => weaponPoint;
        public bool IsActive => gameObject.activeSelf;
        public List<Transform> HitPoints => hitPoints;
        public float Hp => hp;

        public List<Track> Tracks => tracks;

        public float RotSpeed => rotSpeed;

        public float Speed => speed;

        public Rotator RotatorData => rotator;

        public void ActiveCorpus(bool state, bool isMine)
        {
            gameObject.SetActive(state);

            if (!isMine)
            {
                foreach (var tr in tracks)
                {
                    tr.DecreaseParticlesCount();
                }
            }
        }


        public void SetLayer(int layer)
        {
            foreach (var col in colliders)
            {
                col.gameObject.layer = layer;
            }
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < hitPoints.Count; i++)
            {
                for (int j = 0; j < hitPoints.Count; j++)
                {
                    Gizmos.DrawLine(hitPoints[i].position, hitPoints[j].position);
                }
            }
        }
#endif
    }
}