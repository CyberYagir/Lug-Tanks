using UnityEngine;

namespace Base.Weapons.Animate
{
    public class Weapon2Charger : MonoBehaviour
    {
        [SerializeField] private Animator animate;
        [SerializeField] private AudioSource source;

        void Update()
        {
            var isChange = animate.GetCurrentAnimatorStateInfo(0).IsTag("Charge");

            if (isChange)
            {
                if (!source.isPlaying)
                {
                    source.Play();
                }
            }
            else
            {
                if (source.isPlaying)
                {
                    source.Stop();
                }
            }
        }
    }
}
