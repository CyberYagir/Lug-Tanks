using System.Collections.Generic;
using UnityEngine;

namespace Scriptable
{
    [CreateAssetMenu(menuName = "GameData", fileName = "GameDataObject", order = 0)]
    public class GameDataObject : ScriptableObject
    {
        [System.Serializable]
        public class Teams
        {
            [System.Serializable]
            public class Team
            {
                [SerializeField] private int id;
                [SerializeField] private Texture2D texture;

                public Texture2D Texture => texture;

                public int ID => id;
            }

            [SerializeField] private List<Team> teams;

            public Team GetTeam(int id)
            {
                return teams.Find(x => x.ID == id);
            }
        }
        [System.Serializable]
        public class Maps
        {
            [System.Serializable]
            public class Map
            {
                [SerializeField] private string mapName;
                [SerializeField] private Sprite mapIcon;

                public Sprite MapIcon => mapIcon;
                public string MapName => mapName;
            }
            [SerializeField] private List<Map> maps;
            public int Count => maps.Count;

            public Sprite GetMapSprite(int map)
            {
                return maps[Mathf.Clamp(map, 0, maps.Count - 1)].MapIcon;
            }
        }
        public enum GameMode
        {
            FFA = 0, TDM = 1
        }
        [System.Serializable]
        public class GameModes
        {

            [System.Serializable]
            public class Mode
            {
                [SerializeField] private GameMode gameMode;
                [SerializeField] private Sprite sprite;

                public Sprite Sprite => sprite;

                public GameMode GameMode => gameMode;
            }

            [SerializeField] private List<Mode> modes;


            public Sprite GetModeSprite(GameMode mode)
            {
                return modes.Find(x => x.GameMode == mode).Sprite;
            }

            public Mode StringToMode(string mode)
            {
                return modes.Find(x => x.GameMode.ToString().ToLower() == mode.ToLower());
            }
        }
        
        [System.Serializable]
        public class LevelProgress
        {
            [SerializeField] private Sprite[] sprites;
            [SerializeField] private int startMaxExp = 50;

            public int StartMaxExp => startMaxExp;
            public int LevelsCount => sprites.Length;

            public Sprite GetIcon(int id) => sprites[id];
        }
        
        
        [SerializeField] private Teams teamsData;
        [SerializeField] private Maps maps;
        [SerializeField] private GameModes gameModes;
        [SerializeField] private LevelProgress levelProgress;
        public Teams TeamsData => teamsData;
        public Maps MapsData => maps;
        public GameModes GameModesData => gameModes;

        public LevelProgress PlayerLevelsData => levelProgress;
    }
}
