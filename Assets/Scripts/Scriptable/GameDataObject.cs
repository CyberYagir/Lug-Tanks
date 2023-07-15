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

        [SerializeField] private Teams teamsData;

        public Teams TeamsData => teamsData;
    }
}
