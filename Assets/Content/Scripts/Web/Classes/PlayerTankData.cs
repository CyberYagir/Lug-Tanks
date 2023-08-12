using Content.Scripts.Anticheat;
using UnityEngine;
using Web.Classes;

namespace Content.Scripts.Web.Classes
{
    [System.Serializable]
    public class PlayerTankData : ChildTable
    {
        [SerializeField] private int weapon, corpus, exp, level;



        public int Level => level.ObfUn();

        public int Exp => exp.ObfUn();

        public int Corpus => corpus.ObfUn();

        public int Weapon => weapon.ObfUn();

        
        public int GetWeaponId() => weapon;
        public int GetCorpusId() => corpus;

        public PlayerTankData()
        {

        }
        
        public PlayerTankData(int id, int userid, int weapon, int corpus, int exp, int level) : base(id, userid)
        {
            this.weapon = weapon;
            this.corpus = corpus;
            this.exp = exp;
            this.level = level;
        }

        public PlayerTankData Obfuscate()
        {
            return new PlayerTankData(id.Obf(), userid.Obf(), weapon.Obf(), corpus.Obf(), exp.Obf(), level.Obf());
        }

        public PlayerTankData UnObfuscate()
        {
            return new PlayerTankData(ID, Userid, Weapon, Corpus, Exp, Level);
        }

        public void SetCorpus(int id) => corpus = id.Obf();
        public void SetWeapon(int id) => weapon = id.Obf();

        public void AddXp(int xp) => exp.ObfAdd(xp);
    }
}