using Content.Scripts.Web.Classes;
using UnityEngine;

namespace Web.Classes
{
    [System.Serializable]
    public class Responce
    {
        [SerializeField] private PlayerData playerData;
        [SerializeField] private PlayerTankData tank;
        [SerializeField] private PlayerStats statistics;
        [SerializeField] private UserStatistics userStatistics;


        public Responce(PlayerData playerData, PlayerTankData tank, PlayerStats statistics, UserStatistics userStatistics)
        {
            this.playerData = playerData;
            this.tank = tank;
            this.statistics = statistics;
            this.userStatistics = userStatistics;
        }

        public UserStatistics UserStatistics => userStatistics;

        public PlayerStats Statistics => statistics;

        public PlayerTankData Tank => tank;

        public PlayerData PlayerData => playerData;

        public Responce Obfuscate()
        {
            return new Responce(
                playerData:     PlayerData.Obfuscate(),
                tank:           Tank.Obfuscate(),
                userStatistics: UserStatistics.Obfuscate(),
                statistics:     Statistics.Obfuscate()
            );
        }


        public Responce UnObfuscate()
        {
            return new Responce(
                playerData: PlayerData.UnObfuscate(),
                tank: Tank.UnObfuscate(),
                userStatistics: UserStatistics.UnObfuscate(),
                statistics: Statistics.UnObfuscate()
            );
        }

        public void SetStatistics(PlayerStats stats)
        {
            statistics = stats;
        }
    }
}