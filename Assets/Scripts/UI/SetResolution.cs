using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

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


#if PLATFORM_STANDALONE_WIN
    
    //Import the following.
    [DllImport("user32.dll", EntryPoint = "SetWindowText")]
    public static extern bool SetWindowText(System.IntPtr hwnd, System.String lpString);
    [DllImport("user32.dll", EntryPoint = "FindWindow")]
    public static extern System.IntPtr FindWindow(System.String className, System.String windowName);
    
    //Get the window handle.
    public static System.IntPtr windowPtr = FindWindow(null, "Lug-Tanks");
    //Set the title text using the window handle.
#endif
    
    public static void SetName(string newname)
    {
        
#if PLATFORM_STANDALONE_WIN
        SetWindowText(windowPtr, newname);
#endif
    }
}
