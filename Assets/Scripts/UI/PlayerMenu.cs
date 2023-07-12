using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Web;

public class PlayerMenu : MonoBehaviour
{
    public float time;
    public Animator animator;
    bool open = false;

    public void Continue(){
        ChangeMode();
    }

    public void Suicide(){
        GameManager.Instance.LocalPlayer.GetComponent<Tank.Controller.Tank>().tankOptions.hp = 0;
    }

    public void Disconnect(){
        WebDataService.SaveStart();
        GameManager.Instance.Disconnect();
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Escape) && time > 0.5f)
        {
            ChangeMode();
        }
    }

    public void ChangeMode()
    {
        if (open)
        {
            animator.Play("HideMenu");
        }
        else
        {
            animator.Play("OpenMenu");
        }

        open = !open;
        GameManager.pause = open;
        time = 0;
    }
}
