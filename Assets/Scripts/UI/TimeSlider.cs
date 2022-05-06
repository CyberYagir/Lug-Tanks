using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TimeSlider : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] Slider slider;


    void Update()
    {
        if (!slider.wholeNumbers)
        {
            text.text = slider.value.ToString("F2") + "/" + slider.maxValue.ToString("F2");
        }
        else
        {
            text.text = slider.value + "/" + slider.maxValue;
        }
    }
}
