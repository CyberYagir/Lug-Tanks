using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Localization
{
    public class LocalizationService : MonoBehaviour
    {
        private static LocalizationService Instance;
        
        [SerializeField] private TextAsset text;
        [SerializeField] private List<string> langs;
        [SerializeField] private Dictionary<string, CSVParser.WorldHolder> worldHolders;

        private int currentLanguage = 0;
        private CSVParser parser = new CSVParser();

        private const string LANGUAGE_PREF = "Language";
        
        public void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            
            worldHolders = parser.ParseTable(text).ToDictionary(x=>x.Key);
            langs = parser.GetLanguagesNamesList();

            if (PlayerPrefs.HasKey(LANGUAGE_PREF))
            {
                currentLanguage = Mathf.Clamp(PlayerPrefs.GetInt(LANGUAGE_PREF, 0), 0, langs.Count - 1);
            }
            else
            {
                if (Application.systemLanguage == SystemLanguage.Russian ||
                    Application.systemLanguage == SystemLanguage.Belarusian)
                {
                    currentLanguage = langs.FindIndex(x => x == "ru");
                }
                else if (Application.systemLanguage == SystemLanguage.Ukrainian)
                {
                    currentLanguage = langs.FindIndex(x => x == "ua");
                }
                else
                {
                    currentLanguage = langs.FindIndex(x => x == "en");
                }
                PlayerPrefs.SetInt(LANGUAGE_PREF, currentLanguage);
                
            }
        }

        public static void ChangeLanguage(int val)
        {
            Instance.currentLanguage = val;
            OnChangeLanguage?.Invoke(val);
            PlayerPrefs.SetInt(LANGUAGE_PREF, Instance.currentLanguage);
        }


        public static event Action<int> OnChangeLanguage;
        public static int CurrentLanguage => Instance.currentLanguage;
        public static List<string> LanguagesList => Instance.langs;

        public static string GetWorld(string key)
        {
            if (Instance.worldHolders.ContainsKey(key))
            {
                return Instance.worldHolders[key].GetWord(Instance.currentLanguage);
            }
            return String.Empty;
        }
    }
}
