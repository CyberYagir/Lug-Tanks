using UnityEngine;
using UnityEngine.UI;

namespace Photon.Game.UI
{
    public class ShahidIndicator : TankUIElement
    {
        [SerializeField] private Image main;
        [SerializeField] private Image fill;


        public override void UpdateElement()
        {
            if (Player.GetTime() == 0)
            {
                main.enabled = false;
                fill.fillAmount -= Time.deltaTime * 4f;
                if (fill.fillAmount < 0)
                {
                    fill.fillAmount = 0;
                }
            }
            else
            {
                main.enabled = true;
                fill.fillAmount = Mathf.Clamp(Player.GetTime(), 0, 2f) / 2f;
            }
        }
    }
}
