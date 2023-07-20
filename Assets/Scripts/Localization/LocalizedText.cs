using System;
using TMPro;
using UnityEngine;

namespace Localization
{
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private string key;
#if UNITY_EDITOR
        private void OnValidate()
        {
            text = GetComponent<TMP_Text>();
        }
#endif

        private void Awake()
        {
            text.fontStyle |= FontStyles.UpperCase;
            LocalizationService.OnChangeLanguage += OnChangeLanguage;
            OnChangeLanguage(-1);
        }

        private void OnChangeLanguage(int newLanguageID)
        {
            text.text = LocalizationService.GetWorld(key);
        }

        private void OnDestroy()
        {
            LocalizationService.OnChangeLanguage -= OnChangeLanguage;
        }
    }
}
