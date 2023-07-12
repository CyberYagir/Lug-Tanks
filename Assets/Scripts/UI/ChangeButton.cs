using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ChangeButton : MonoBehaviour
    {
        [SerializeField] protected int id;
        [SerializeField] protected Color standard;
        [SerializeField] protected Color selected;
        
        
        protected Image image;
        protected Tank.Controller.Tank tank;


        public virtual void Init(Tank.Controller.Tank tank)
        {
            image = GetComponent<Image>();
            this.tank = tank;
            
            tank.tankOptions.OnChangeTank.AddListener(UpdateBtn);

            UpdateBtn();
        }

        public virtual void UpdateBtn()
        {
            
        }

        public virtual void Click()
        {
            UpdateBtn();
        }
    }
}