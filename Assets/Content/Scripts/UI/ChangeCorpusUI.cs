using Content.Scripts.Anticheat;
using Web;

namespace UI
{
    public class ChangeCorpusUI : ChangeButton
    {
        public override void UpdateBtn()
        {
            base.UpdateBtn();
            if (tank.tankOptions.Corpus == id)
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
            WebDataService.UserData.Tank.SetCorpus(id);
        }
    }
}
