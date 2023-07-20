using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SetResolution : MonoBehaviour
    {
        public Sprite minScr, fullscr;
        public void Full()
        {
            if (!Screen.fullScreen)
            {
                Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, true, 60);
                transform.GetChild(0).GetComponent<Image>().sprite = minScr;
            }
            else
            {
                Screen.SetResolution(Screen.width, Screen.height, false, 60);
                transform.GetChild(0).GetComponent<Image>().sprite = fullscr;
            }
        }
    
        public static void SetName(string newname)
        {
        }
    }
}
