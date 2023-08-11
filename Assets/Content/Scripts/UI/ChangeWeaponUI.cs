using Content.Scripts.Anticheat;
using UnityEngine;
using UnityEngine.UI;
using Web;

namespace UI
{
    public class ChangeWeaponUI : ChangeButton
    {

        public override void UpdateBtn()
        {
            base.UpdateBtn();
            if (tank.tankOptions.Weapon == id)
            {
                image.color = selected;
            }
            else
            {
                image.color = standard;
            }
        }
        

        public override void Click()
        {
            base.Click();
            WebDataService.data.tank.weapon = id.Obf();
        }
    }
}
