using Content.Scripts.Anticheat;

namespace Content.Scripts.Web.Classes
{
    [System.Serializable]
    public class PlayerData
    {
        public int id = -1;
        public string name;
    
        public PlayerData()
        {

        }

        public PlayerData Obfuscate()
        {
            return new PlayerData()
            {
                id = id.Obf(),
                name = name.Obf()
            };
        }

        public PlayerData UnObfuscate()
        {
            return new PlayerData()
            {
                id = id.ObfUn(),
                name = name.ObfUn()
            };
        }
    }
}