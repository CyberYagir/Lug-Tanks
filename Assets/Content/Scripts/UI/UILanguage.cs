using System;
using System.Collections.Generic;
using Localization;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UILanguage : MonoBehaviour
    {
        private void Awake()
        {
            var dropdown = GetComponent<TMP_Dropdown>();

            var optionsData = new List<TMP_Dropdown.OptionData>();

            foreach (var lang in LocalizationService.LanguagesList)
            {
                optionsData.Add(new TMP_Dropdown.OptionData(lang));
            }


            dropdown.options = optionsData;
            dropdown.value = LocalizationService.CurrentLanguage;
            dropdown.onValueChanged.AddListener(OnChangeLanguage);
        }

        private void OnChangeLanguage(int value)
        {
            LocalizationService.ChangeLanguage(value);
        }
    }
}
