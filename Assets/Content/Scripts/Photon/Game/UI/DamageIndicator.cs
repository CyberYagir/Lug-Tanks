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
        private Vector3 baseIndicatorScale;

        public override void Init(Player player)
        {
            base.Init(player);

            camera = player.CameraLook.GetCamera();

            indicator.gameObject.SetActive(true);
            baseIndicatorScale = indicator.transform.localScale;
            indicator.transform.localScale = Vector3.zero;
        }


        public override void UpdateElement()
        {
            bool showIndicator = false;
            if (Tank.lastPlayer != null && Tank.lastPlayerClearTime < 10 && WeaponRotate.IsVisible(Tank.lastPlayer.gameObject))
            {
                var tank = Tank.lastPlayer.GetComponent<Tank>();
                foreach (var point in tank.corpuses[tank.tankOptions.Corpus].HitPoints)
                {
                    if (Physics.Raycast(camera.transform.position, point.position - camera.transform.position, out var hit))
                    {
                        if (hit.rigidbody.gameObject == Tank.lastPlayer)
                        {
                            showIndicator = true;
                            pname.text = Tank.lastPlayer.name;
                        
                            hp.transform.localScale = new Vector3(tank.tankOptions.Hp / tank.corpuses[tank.tankOptions.Corpus].Hp, 1, 1);
                            indicator.transform.position = Vector3.Lerp(indicator.transform.position, camera.WorldToScreenPoint(tank.damageDisplayPoint.position, Camera.MonoOrStereoscopicEye.Mono), 10f * Time.deltaTime);
                            bonus.SetActive(tank.bonuses.Count != 0);

                            for (int i = 0; i < bonuses.Count; i++)
                            {
                                bonuses[i].SetActive(tank.bonuses.Contains(i));
                            }
                            
                            break;
                        }
                    }
                }
            }

            indicator.transform.localScale = Vector3.Lerp(indicator.transform.localScale, showIndicator ? baseIndicatorScale : Vector3.zero, 20 * Time.deltaTime);
        }
    }
}
