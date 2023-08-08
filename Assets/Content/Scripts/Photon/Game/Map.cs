using System.Collections.Generic;
using UnityEngine;

namespace Photon.Game
{
    [System.Serializable]
    public class Map {
        [SerializeField] private GameObject map;
        [SerializeField] private BonusSpawns bonusSpawns;
        [SerializeField] private List<TeamSpawn> teamSpawns;

        public BonusSpawns BonusSpawns => bonusSpawns;


        public void SetMapState(bool state)
        {
            map.SetActive(state);
        }

        public TeamSpawn GetTeamSpawn(int id)
        {
            return teamSpawns[id];
        }
    }
}