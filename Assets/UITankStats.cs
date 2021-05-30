using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITankStats : MonoBehaviour
{
    public Tank tank;
    public RectTransform hp, energy;
    public Vector2 pos;
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, Camera.main.WorldToScreenPoint(tank.transform.position, Camera.MonoOrStereoscopicEye.Mono) + (Vector3)pos, 5 * Time.deltaTime);
        hp.localScale = new Vector3((float)tank.tankOptions.hp / tank.corpuses[tank.tankOptions.corpus].hp, 1, 1);
        if (!tank.weapons[tank.tankOptions.weapon].waitTofull)
        {
            energy.localScale = new Vector3(tank.weapons[tank.tankOptions.weapon].getEnergy() / 100f, 1, 1);
        }
        else
        {
            energy.localScale = new Vector3((tank.weapons[tank.tankOptions.weapon].getEnergy() - tank.weapons[tank.tankOptions.weapon].getShotEnergy()) / (100f-tank.weapons[tank.tankOptions.weapon].getShotEnergy()), 1, 1);
        }
    }
}
