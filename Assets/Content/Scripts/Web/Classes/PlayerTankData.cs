using Content.Scripts.Anticheat;
using Web.Classes;

namespace Content.Scripts.Web.Classes
{
    [System.Serializable]
    public class PlayerTankData : ChildTable
    {
        public int weapon, corpus, exp, level;
    
        public PlayerTankData()
        {

        }

        public PlayerTankData Obfuscate()
        {
            return new PlayerTankData()
            {
                id = id.Obf(),
                userid = userid.Obf(),
                weapon = weapon.Obf(),
                corpus = corpus.Obf(),
                exp = exp.Obf(),
                level = level.Obf(),
            };
        }

        public PlayerTankData UnObfuscate()
        {
            return new PlayerTankData()
            {
                id = id.ObfUn(),
                userid = userid.ObfUn(),
                weapon = weapon.ObfUn(),
                corpus = corpus.ObfUn(),
                exp = exp.ObfUn(),
                level = level.ObfUn(),
            };
        }
    }
}