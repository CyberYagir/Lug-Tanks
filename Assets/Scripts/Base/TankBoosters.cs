using System.Collections;
using System.Collections.Generic;
using Base.Controller;
using UnityEngine;

namespace Base.Modifyers
{
    [System.Serializable]
    public class PlayerBonus {
        public int type;
        public float fireRateIncrease = 1;
        public float speedIncrease = 1;
        public float heathAdd = 0;
        public float defenceIncrease = 1;
        public float time = 10;
        public float fulltime = 10;
    }


    public class TankBoosters : MonoBehaviour
    {
        
        [SerializeField] private float defenceIncrease = 1;
        [SerializeField] private float fireRateIncrease = 1;
        [SerializeField] private float speedIncrease = 1;
    
        [SerializeField] private List<PlayerBonus> playerBonus;
        
        private Tank tank;


        public float SpeedIncrease => speedIncrease;
        public float FireRateIncrease => fireRateIncrease;
        public float DefenceIncrease => defenceIncrease;


        public List<PlayerBonus> ActiveBoosters => playerBonus;

        public void Init(Tank tank)
        {
            this.tank = tank;
        }
        
        
        public void AddBonus(PlayerBonus plBonus)
        {
            if (playerBonus.Find(x => x.type == plBonus.type) != null)
            {
                playerBonus.Find(x => x.type == plBonus.type).time += plBonus.time;
                return;
            }
            else
            {
                plBonus.fulltime = plBonus.time;
                playerBonus.Add(plBonus);
            }
        }

        private void Update()
        {
            defenceIncrease = 1;
            fireRateIncrease = 1;
            speedIncrease = 1;
            var bonuses = new List<int>();
            for (int i = 0; i < playerBonus.Count; i++)
            {
                bonuses.Add(playerBonus[i].type);
                speedIncrease *= playerBonus[i].speedIncrease;
                fireRateIncrease *= playerBonus[i].fireRateIncrease;
                defenceIncrease *= playerBonus[i].defenceIncrease;

                tank.tankOptions.hp += playerBonus[i].heathAdd * Time.deltaTime;
                if (tank.tankOptions.hp > tank.corpuses[tank.tankOptions.corpus].hp)
                {
                    tank.tankOptions.hp = tank.corpuses[tank.tankOptions.corpus].hp;
                    playerBonus.RemoveAt(i);
                    return;
                }

                if (playerBonus[i].time > playerBonus[i].fulltime)
                {
                    playerBonus[i].time = playerBonus[i].fulltime;
                }
                playerBonus[i].time -= Time.deltaTime;
                if (playerBonus[i].time <= 0)
                {
                    playerBonus.RemoveAt(i);
                    return;
                }
            }
            tank.bonuses = bonuses;
        }
    }
}