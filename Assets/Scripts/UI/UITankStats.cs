using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITankStats : MonoBehaviour
{
    [SerializeField] private Tank.Controller.Tank tank;
    [SerializeField] private RectTransform hp, energy;
    [SerializeField] private GameObject[] bonuses;
    [SerializeField] private GameObject bonusesHolder;
    [SerializeField] private Vector2 pos;

    private Camera camera;

    private void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        bonusesHolder.SetActive(tank.bonuses.Count != 0);
        for (int i = 0; i < bonuses.Length; i++)
        {
            bonuses[i].SetActive(tank.bonuses.Contains(i));
            if (tank.bonuses.Contains(i))
            {
                var b = TankModificators.modificators.playerBonus.Find(x => x.type == i);
                if (b != null)
                {
                    bonuses[i].transform.GetChild(0).GetComponent<Image>().fillAmount = b.time / b.fulltime;
                }
            }
        }
        transform.position = Vector3.Lerp(transform.position, camera.WorldToScreenPoint(tank.transform.position, Camera.MonoOrStereoscopicEye.Mono) + (Vector3)pos, 5 * Time.deltaTime);
        hp.localScale = new Vector3((float)tank.tankOptions.hp / tank.corpuses[tank.tankOptions.corpus].hp, 1, 1);
        if (!tank.weapons[tank.tankOptions.weapon].waitTofull)
        {
            energy.localScale = new Vector3(tank.weapons[tank.tankOptions.weapon].GetEnergy() / 100f, 1, 1);
        }
        else
        {
            energy.localScale = new Vector3((tank.weapons[tank.tankOptions.weapon].GetEnergy() - tank.weapons[tank.tankOptions.weapon].GetShotEnergy()) / (100f-tank.weapons[tank.tankOptions.weapon].GetShotEnergy()), 1, 1);
        }
    }
}
