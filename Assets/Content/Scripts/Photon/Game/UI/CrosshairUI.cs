using UnityEngine;
using Base.Controller;
namespace Photon.Game.UI
{
    public class CrosshairUI : TankUIElement
    {
        [SerializeField] private GameObject crosshair;
        [SerializeField] private LayerMask layerMask;

        private Tank tank;
        private Camera camera;

        public override void Init(Player player)
        {
            base.Init(player);
            
            crosshair.gameObject.SetActive(false);
            camera = Player.CameraLook.GetCamera();
            tank = player.Tank;
        }


        public override void UpdateElement()
        {
            var enemies = tank.weapons[tank.tankOptions.Weapon].Enemies(tank.weapons[tank.tankOptions.Weapon].shootPoint);

            if (enemies.Count == 0)
            {
                crosshair.SetActive(false);
            }
            else
            {
                RaycastHit hit;

                if (Physics.Raycast(tank.weapons[tank.tankOptions.Weapon].shootPoint.position, enemies[0].enemy.transform.position - tank.weapons[tank.tankOptions.Weapon].shootPoint.position, out hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Collide))
                {
                    if (hit.transform == enemies[0].enemy.transform)
                    {
                        if (Tank.lastPlayer == null || Tank.lastPlayerClearTime > 5f)
                        {
                            crosshair.SetActive(true);
                            crosshair.transform.position = camera.WorldToScreenPoint(enemies[0].enemy.transform.position, Camera.MonoOrStereoscopicEye.Mono);
                            return;
                        }
                    }
                }

                crosshair.SetActive(false);
            }
        }
    }
}
