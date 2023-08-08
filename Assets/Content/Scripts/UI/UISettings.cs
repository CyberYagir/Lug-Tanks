using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace UI
{
    public class UISettings : MonoBehaviour
    {
        [SerializeField] private Slider shadowsS, senseS, audioS;
        [SerializeField] private Toggle toggleAmbient;
        [SerializeField] private Toggle toggleGrass;
        [SerializeField] private RenderPipelineAsset normal, ambient;

        public UnityEvent<bool> OnChangeGrassState = new UnityEvent<bool>();
        
        private void Start()
        {
            shadowsS.value = PlayerPrefs.GetInt("Shadows", 90);
            ChangeShadow(shadowsS);

            toggleAmbient.isOn = PlayerPrefs.GetInt("Ambient", 1) == 1;
            AmbientChange(toggleAmbient);

            toggleGrass.isOn = PlayerPrefs.GetInt("Grass", 1) == 1;
            GrassChange(toggleGrass);
            
            audioS.value = PlayerPrefs.GetFloat("Vol", 1);
            AudioChange(audioS);

            senseS.value = PlayerPrefs.GetFloat("Sens", 1);
        }


        public void SensChange(Slider slider)
        {
            PlayerPrefs.SetFloat("Sens", slider.value);
            senseS.value = slider.value;
        }
        public void AudioChange(Slider slider)
        {
            PlayerPrefs.SetFloat("Vol", slider.value);
            AudioListener.volume = slider.value;
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
        
        public void GrassChange(Toggle toggle)
        {
            PlayerPrefs.SetInt("Grass", toggleGrass.isOn ? 1 : 0);
            OnChangeGrassState.Invoke(toggleGrass.isOn);
        }
    }
}
