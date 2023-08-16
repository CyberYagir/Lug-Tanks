using System;
using System.Collections.Generic;
using UnityEngine;
using Base.Controller;
using Base.Weapons.Arms;
using EPOOutline;

namespace Photon.Game.UI
{
    public class CrosshairUI : TankUIElement
    {
        // [SerializeField] private GameObject crosshair;
        [SerializeField] private LayerMask layerMask;

        private Tank tank;
        private Camera camera;
        private Tank targetTank;

        public bool activeOutline = false;

        
        private List<Outlinable> outlinables = new List<Outlinable>(5);
        
        public override void Init(Player player)
        {
            base.Init(player);

            // crosshair.gameObject.SetActive(false);
            camera = Player.CameraLook.GetCamera();
            tank = player.Tank;


            ConfigurePlayerOutlineMask();
        }

        private void ConfigurePlayerOutlineMask()
        {
            var weapon = tank.weapons[tank.tankOptions.Weapon];
            var corpus = tank.corpuses[tank.tankOptions.Corpus];

            var weaponOutline = weapon.GetComponent<Outlinable>();
            var corpusOutline = corpus.GetComponent<Outlinable>();

            weaponOutline.DrawingMode = OutlinableDrawingMode.Normal | OutlinableDrawingMode.Obstacle;
            corpusOutline.DrawingMode = weaponOutline.DrawingMode;

            weaponOutline.OutlineParameters.DilateShift = 0;
            corpusOutline.OutlineParameters.DilateShift = 0;

            corpusOutline.enabled = true;
            weaponOutline.enabled = true;
        }


        public override void UpdateElement()
        {
            var weapon = tank.weapons[tank.tankOptions.Weapon];
            var enemies = weapon.Enemies(weapon.shootPoint);

            if (enemies.Count == 0)
            {
                if (targetTank != null)
                {
                    SetOutline(false);
                    targetTank = null;
                }
            }
            else
            {
                RaycastHit hit;

                if (Physics.Raycast(weapon.shootPoint.position, enemies[0].point.position - weapon.shootPoint.position, out hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Collide))
                {
                    if (hit.transform == enemies[0].enemy.transform)
                    {
                        var enemyTank = enemies[0].enemy.GetComponent<Tank>();
                        if (targetTank != enemyTank)
                        {
                            targetTank = enemyTank;
                            SetOutline(true);
                        }
                        else
                        {
                            Weapon.DamageScaler.DamageType type;
                            Color color;
                            tank.weapons[tank.tankOptions.Weapon].GetDamage(hit.distance, out type);

                            switch (type)
                            {
                                case Weapon.DamageScaler.DamageType.FullDamage:
                                    color = Color.red;
                                    break;
                                case Weapon.DamageScaler.DamageType.MiddleDamage:
                                    color = new Color32(252, 126, 0, 255);
                                    break;
                                case Weapon.DamageScaler.DamageType.LowDamage:
                                    color = new Color32(69, 159, 30, 255);
                                    break;
                                default:
                                    color = new Color32(109, 108, 105, 255);
                                    break;
                            }

                            for (int i = 0; i < outlinables.Count; i++)
                            {
                                outlinables[i].OutlineParameters.Color = color;
                            }
                        }
                        return;

                    }
                }

                if (targetTank != null)
                {
                    SetOutline(false);
                }
            }
        }

        private void OnDestroy()
        {
            if (targetTank != null)
            {
                SetOutline(false);
            }
        }

        public void SetOutline(bool state)
        {
            if (activeOutline != state)
            {
                var weaponOut = targetTank.weapons[targetTank.tankOptions.Weapon].GetComponent<Outlinable>();
                var corpusOut = targetTank.corpuses[targetTank.tankOptions.Corpus].GetComponent<Outlinable>();
                weaponOut.enabled = state;
                corpusOut.enabled = state;

                outlinables.Clear();
                if (state)
                {
                    outlinables.Add(weaponOut);
                    outlinables.Add(corpusOut);
                }
                
                activeOutline = state;
            }
        }
    }
}
