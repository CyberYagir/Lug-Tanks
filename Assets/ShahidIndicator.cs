using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShahidIndicator : MonoBehaviour
{
    public Image main;
    public Image fill;
    private void Update()
    {
        if (GetComponentInParent<Player>().timetosuicide == 0)
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
            fill.fillAmount = Mathf.Clamp(GetComponentInParent<Player>().timetosuicide, 0, 2f) / 2f;
        }
    }
}
