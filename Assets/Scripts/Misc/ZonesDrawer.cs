using System;
using UnityEngine;

namespace Misc
{
    [ExecuteInEditMode]
    public class ZonesDrawer : MonoBehaviour
    {
        [SerializeField] private Color color;
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
            Gizmos.color = color;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }

#endif
    }
}
