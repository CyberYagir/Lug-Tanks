using Content.Scripts.Web.Classes;

namespace Web.Classes
{
    [System.Serializable]
    public class Responce
    {
        public PlayerData playerData;
        public PlayerTankData tank;
        public PlayerStats statistics;
        public UserStatistics userStatistics;



        public Responce Obfuscate()
        {
            return new Responce()
            {
                playerData = playerData.Obfuscate(),
                tank = tank.Obfuscate(),
                userStatistics = userStatistics.Obfuscate(),
                statistics = statistics.Obfuscate(),
            };
        }
        
        
        public Responce UnObfuscate()
        {
            return new Responce()
            {
                playerData = playerData.UnObfuscate(),
                tank = tank.UnObfuscate(),
                userStatistics = userStatistics.UnObfuscate(),
                statistics = statistics.UnObfuscate(),
            };
        }
    }
}