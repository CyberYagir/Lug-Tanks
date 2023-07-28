using System.Collections.Generic;
using UnityEngine;

namespace Base.Controller
{
    public class TankPart : MonoBehaviour
    {
        private static readonly int MainTex = Shader.PropertyToID("_BaseMap");
        
        [SerializeField] private List<Renderer> renderers;

        public void SetTexture(Texture2D texture)
        {
            foreach (var rn in renderers)
            {
                rn.material.SetTexture(MainTex,texture);
            }
        }
        
    }
}