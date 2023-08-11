using System.Collections.Generic;
using Base.Weapons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Base.Controller;
namespace Photon.Game.UI
{
    public class DamageIndicator : TankUIElement
    {
        [SerializeField] private Image hp;
        [SerializeField] private GameObject indicator;
        [SerializeField] private TMP_Text pname;
        [SerializeField] private List<GameObject> bonuses;
        [SerializeField] private GameObject bonus;


        private Camera camera;

        public override void Init(Player player)
        {
            base.Init(player);

            camera = player.CameraLook.GetCamera();
        }
        

        public override void UpdateElement()
        {
            if (Tank.lastPlayer != null && Tank.lastPlayerClearTime < 10 && WeaponRotate.IsVisible(Tank.lastPlayer.gameObject))
            {
                var tank = Tank.lastPlayer.GetComponent<Tank>();
                indicator.gameObject.SetActive(true);
                hp.transform.localScale = new Vector3(tank.tankOptions.Hp / tank.corpuses[tank.tankOptions.Corpus].Hp, 1, 1);
                pname.text = Tank.lastPlayer.name;
                indicator.transform.position = Vector3.Lerp(indicator.transform.position, camera.WorldToScreenPoint(tank.damageDisplayPoint.position, Camera.MonoOrStereoscopicEye.Mono), 10f * Time.deltaTime);
                bonus.SetActive(tank.bonuses.Count != 0);

                for (int i = 0; i < bonuses.Count; i++)
                {
                    bonuses[i].SetActive(tank.bonuses.Contains(i));
                }
            }
            else
            {
                indicator.gameObject.SetActive(false);
            }
        }
    }
}
