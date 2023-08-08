using System.Collections.Generic;
using Base.Controller;
using UnityEngine;

namespace Base
{
    public class Look : MonoBehaviour
    {
        [SerializeField] private List<Renderer> renderers;
        [SerializeField] private Transform collider;
        private float transp;
        private Collider col;
        private Material mat;
        private float alpha = 1;
        
        private Camera camera;
        private static readonly int AlphaHash = Shader.PropertyToID("_Alpha");

        private void Start()
        {
            col = collider.GetComponent<Collider>();
            mat = renderers[0].material;

            foreach (var rn in renderers)
            {
                rn.material = mat;
                rn.transform.localEulerAngles += Random.insideUnitSphere * 5f;
            }
        }
        void Update()
        {
            var lookCamera = CameraLook.Instance != null ? CameraLook.Instance.GetCamera() : camera;
        
            if (lookCamera != null)
            {
                alpha = Mathf.Lerp(alpha, transp, 5f * Time.deltaTime);
                
                mat.SetFloat(AlphaHash, alpha);
                
                
                collider.LookAt(new Vector3(lookCamera.transform.position.x, transform.position.y, lookCamera.transform.position.z));
                if (Vector3.Distance(transform.position, lookCamera.transform.parent.position) < 4f)
                {
                    col.enabled = false;
                    transp = 0.5f;

                }
                else
                {
                    col.enabled = true;
                    transp = 1f;
                }
            }
        }
    }
}
