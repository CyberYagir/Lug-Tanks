using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Localization
{
    public class CSVParser
    {
        [System.Serializable]
        public class WorldHolder
        {
            [SerializeField] private string key;
            [SerializeField] private List<string> localizations;

            public string Key => key;
            
            public WorldHolder(string key, List<string> localizations)
            {
                this.key = key;
                this.localizations = localizations;
            }

            public string GetWord(int language)
            {
                return localizations[language];
            }
        }

        private List<WorldHolder> wordsHolders = new List<WorldHolder>();
        private List<string> langs = new List<string>();

        public const string SEPARATOR = ",";

        private List<string> lineLocales = new List<string>(3);
        public List<WorldHolder> ParseTable(TextAsset textAsset)
        {
            langs.Clear();
            wordsHolders.Clear();
            
            
            var lines = textAsset.text.Split("\n");
            
            ExtractHeader(lines);

            for (int i = 1; i < lines.Length; i++)
            {
                var words = lines[i].Split(SEPARATOR).ToList();
                
                var key = words[0];
                words.RemoveAt(0);
                
                wordsHolders.Add(new WorldHolder(key, words));
            }

            return new(wordsHolders);
        }

        public List<string> GetLanguagesNamesList()
        {
            return new(langs);
        }


        private void ExtractHeader(string[] lines)
        {
            var header = lines[0].Split(SEPARATOR);
            for (int i = 1; i < header.Length; i++)
            {
                langs.Add(header[i]);
            }
        }
    }
}
