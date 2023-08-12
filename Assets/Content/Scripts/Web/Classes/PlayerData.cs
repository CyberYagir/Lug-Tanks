using Content.Scripts.Anticheat;
using UnityEngine;

namespace Content.Scripts.Web.Classes
{
    [System.Serializable]
    public class PlayerData
    {
        [SerializeField] private int id = -1;
        [SerializeField] private string name;

        public PlayerData(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public int ID => id.ObfUn();
        public string Name => name.ObfUn();


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