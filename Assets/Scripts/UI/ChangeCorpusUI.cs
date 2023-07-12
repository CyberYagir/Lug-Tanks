using Web;

namespace UI
{
    public class ChangeCorpusUI : ChangeButton
    {
        public override void UpdateBtn()
        {
            base.UpdateBtn();
            if (tank.tankOptions.corpus == id)
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
            WebDataService.data.tank.corpus = id;
        }
    }
}
