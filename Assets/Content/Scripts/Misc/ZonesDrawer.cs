using System;
using UnityEngine;

namespace Misc
{
    [ExecuteInEditMode]
    public class ZonesDrawer : MonoBehaviour
    {
        [SerializeField] private Color color = Color.white;
#if UNITY_EDITOR
        private Renderer[] renderer;
        private Bounds bounds;
        
        private void Update()
        {
            renderer = GetComponentsInChildren<Renderer>();
            if (renderer.Length == 0) return;

            bounds = renderer[0].bounds;
            foreach (var rn in renderer)
            {
                bounds.Encapsulate(rn.bounds);
            }
        }


        private void OnDrawGizmos()
        {
            var col = color;
            col.a *= 0.3f;
            
            Gizmos.color = col;
            Gizmos.DrawCube(bounds.center, bounds.size + Vector3.one * 0.1f);
        }

#endif
    }
}
