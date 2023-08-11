using Content.Scripts.Anticheat;
using Web.Classes;

namespace Content.Scripts.Web.Classes
{
    [System.Serializable]
    public class UserStatistics : ChildTable
    {
        public int kills;
        public int deaths;
        public string registerDate;

        public UserStatistics Obfuscate()
        {
            return new UserStatistics()
            {
                id = id.Obf(),
                userid = userid.Obf(),
                deaths = deaths.Obf(),
                kills = kills.Obf(),
                registerDate = registerDate.Obf()
            };
        }

        public UserStatistics UnObfuscate()
        {
            return new UserStatistics()
            {
                id = id.ObfUn(),
                userid = userid.ObfUn(),
                deaths = deaths.ObfUn(),
                kills = kills.ObfUn(),
                registerDate = registerDate.ObfUn()
            };
        }
    }
}