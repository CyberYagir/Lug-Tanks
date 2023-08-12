using System;
using UnityEngine;

namespace Content.Scripts.Misc
{
    public class SpawnsDrawer : MonoBehaviour
    {

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            foreach (Transform child in transform)
            {
                var color = Color.green;
                if(Physics.Raycast(child.position, Vector3.down, out RaycastHit hit))
                {
                    if (hit.distance < 1f)
                    {
                        color = Color.red;
                    }
                }
                else
                {
                    color = Color.red;
                }
                Gizmos.color = color;
                Gizmos.DrawWireSphere(child.position, 0.2f);
                Gizmos.DrawLine(child.position, child.position + child.forward * 2f);
                
                
                
                Gizmos.DrawLine(child.position, child.position + -child.up);
            }
        }
#endif
    }
        
}
