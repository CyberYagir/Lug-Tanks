using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon3Audio : MonoBehaviour
{
    public Animator animator;
    public AudioSource audio;

    private void Update()
    {
        if (animator.GetBool("IsShoot"))
        {
            audio.volume += Time.deltaTime * 2f;
        }
        else
        {
            audio.volume -= Time.deltaTime * 2f;
        }
        audio.volume = Mathf.Clamp(audio.volume, 0, 0.5f);
    }
}
