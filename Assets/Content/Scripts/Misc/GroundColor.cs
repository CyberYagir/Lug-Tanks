using UnityEngine;

namespace Misc
{
    public class GroundColor : MonoBehaviour
    {
        [SerializeField] private Color color = Color.gray;

        public Color Color => color;
    }
}
