using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenu : MonoBehaviour
{
    public float time;
    public Animator animator;
    bool open = false;
    private void Update()
    {
        time += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Escape) && time > 0.5f)
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
}
