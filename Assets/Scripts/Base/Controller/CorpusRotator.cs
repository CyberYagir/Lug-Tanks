using System;
using Photon.Pun;
using UnityEngine;

namespace Base.Controller
{
    public class CorpusRotator : MonoBehaviour
    {
        [SerializeField] private Corpus corpus;
        [SerializeField] private AnimationCurve animation;
        private Tank tank;

        private float time;
        private float lastAngle;
        private float targetRotate;
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            corpus = GetComponent<Corpus>();
        }
#endif

        private void Awake()
        {
            tank = GetComponentInParent<Tank>();

            if (tank.gameObject.GetPhotonView() != null && !tank.gameObject.GetPhotonView().IsMine)
            {
                enabled = false;
            }
        }

        private void Update()
        {
            if (Input.GetAxisRaw("Vertical") != 0)
            {
                targetRotate = corpus.RotatorData.Angle * Input.GetAxis("Vertical") * (Mathf.Clamp01(tank.rb.velocity.magnitude / 4f));
                lastAngle = targetRotate;
                time = 0;
            }
            else
            {
                time += Time.deltaTime;
                targetRotate = lastAngle * (animation.Evaluate(time)/2f);
            }

            RotateCorpus(targetRotate);
        }

        public void RotateCorpus(float angle)
        {
            var rotator = corpus.RotatorData.RotateTransform;
            rotator.localRotation = Quaternion.Lerp(rotator.localRotation, Quaternion.Euler(angle, 0, 0), 10 * Time.deltaTime);
        }

        public float GetTagetAngle() => targetRotate;
    }
}
