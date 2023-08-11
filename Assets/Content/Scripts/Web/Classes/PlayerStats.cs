using Content.Scripts.Anticheat;
using Web.Classes;

namespace Content.Scripts.Web.Classes
{
    [System.Serializable]
    public class PlayerStats : ChildTable
    {
        public float lastSessionTime;
        public string lastIP;
        public string lastEnter;

        public PlayerStats Obfuscate()
        {
            return new PlayerStats()
            {
                id = id.Obf(),
                userid = userid.Obf(),
                lastSessionTime = lastSessionTime.Obf(),
                lastIP = lastIP.Obf(),
                lastEnter = lastEnter.Obf()
            };
        }

        public PlayerStats UnObfuscate()
        {
            return new PlayerStats()
            {
                id = id.ObfUn(),
                userid = userid.ObfUn(),
                lastSessionTime = lastSessionTime.ObfUn(),
                lastIP = lastIP.ObfUn(),
                lastEnter = lastEnter.ObfUn()
            };
        }
    }
}