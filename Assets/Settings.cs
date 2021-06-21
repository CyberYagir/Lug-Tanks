using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class Settings : MonoBehaviour
{
    public Slider shadowsS, senseS;
    public Toggle toggleAmbient;
    public RenderPipelineAsset normal, ambient;
    private void Start()
    {
        shadowsS.value = PlayerPrefs.GetInt("Shadows", 90);
        ChangeShadow(shadowsS);

        toggleAmbient.isOn = PlayerPrefs.GetInt("Ambient", 1) == 1 ? true : false;
        AmbientChange(toggleAmbient);

        senseS.value = PlayerPrefs.GetFloat("Sens", 1);
    }


    public void SensChange(Slider slider)
    {
        PlayerPrefs.SetFloat("Sens", slider.value);
        senseS.value = slider.value;
    }
    public void ChangeShadow(Slider slider)
    {
        PlayerPrefs.SetInt("Shadows", (int)slider.value);
        var n = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
        n.shadowDistance = slider.value;
    }

    public void AmbientChange(Toggle toggle)
    {
        GraphicsSettings.renderPipelineAsset = toggle.isOn ? ambient : normal;
        PlayerPrefs.SetInt("Ambient", toggleAmbient.isOn ? 1 : 0);
        ChangeShadow(shadowsS);
    }

}
