using Content.Scripts.Anticheat;
using UnityEngine;

namespace Web.Classes
{
    [System.Serializable]
    public class ChildTable
    {
        [SerializeField] protected int id, userid;



        public int ID => id.ObfUn();
        public int Userid => userid.ObfUn();
        
        public ChildTable()
        {

        }
        
        public ChildTable(int id, int userid)
        {
            this.id = id;
            this.userid = userid;
        }
    }
}