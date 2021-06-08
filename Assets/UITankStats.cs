using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITankStats : MonoBehaviour
{
    public Tank tank;
    public RectTransform hp, energy;
    public GameObject[] bonuses;
    public GameObject bonusesHolder;
    public Vector2 pos;
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
