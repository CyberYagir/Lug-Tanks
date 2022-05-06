using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairUI : MonoBehaviour
{
    [SerializeField] private GameObject crosshair;
    [SerializeField] private Tank tank;
    [SerializeField] private LayerMask layerMask;

    private Camera camera;

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        var enemies = tank.weapons[tank.tankOptions.weapon].Enemies(tank.weapons[tank.tankOptions.weapon].shootPoint);

        if (enemies.Count == 0)
        {
            crosshair.SetActive(false);
        }
        else
        {
            RaycastHit hit;

            if (Physics.Raycast(tank.weapons[tank.tankOptions.weapon].shootPoint.position, enemies[0].enemy.transform.position - tank.weapons[tank.tankOptions.weapon].shootPoint.position, out hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Collide))
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
