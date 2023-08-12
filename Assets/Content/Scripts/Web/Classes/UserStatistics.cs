using Content.Scripts.Anticheat;
using UnityEngine;
using Web.Classes;

namespace Content.Scripts.Web.Classes
{
    [System.Serializable]
    public class UserStatistics : ChildTable
    {
        [SerializeField] private int kills;
        [SerializeField] private int deaths;
        [SerializeField] private string registerDate;



        public int Deaths => deaths;
        public int Kills => kills;
        public string RegisterDate => registerDate;

        public UserStatistics()
        {

        }

        public UserStatistics(int id, int userid, int kills, int deaths, string registerDate) : base(id, userid)
        {
            this.kills = kills;
            this.deaths = deaths;
            this.registerDate = registerDate;
        }

        public UserStatistics Obfuscate()
        {
            return new UserStatistics(id.Obf(), userid.Obf(), kills.Obf(), deaths.Obf(), registerDate.Obf());
        }

        public UserStatistics UnObfuscate()
        {
            return new UserStatistics(ID, Userid, Kills, Deaths, RegisterDate);
        }

        public void AddDeath()
        {
            deaths.ObfAdd(1);
        }

        public void AddKill()
        {
            kills.ObfAdd(1);
        }
    }
}