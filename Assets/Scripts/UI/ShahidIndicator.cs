using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShahidIndicator : MonoBehaviour
{
    [SerializeField] private Image main;
    [SerializeField] private Image fill;


    private Player player;

    private void Start()
    {
        player = GetComponentInParent<Player>();
    }

    private void Update()
    {
        if (player.GetTime() == 0)
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
            fill.fillAmount = Mathf.Clamp(player.GetTime(), 0, 2f) / 2f;
        }
    }
}
