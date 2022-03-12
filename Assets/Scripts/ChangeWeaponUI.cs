using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeWeaponUI : MonoBehaviour
{
    public int weaponid;
    public Color standard, selected;
    Tank tank;

    private void Start()
    {
        tank = FindObjectOfType<Tank>();
    }
    private void Update()
    {
        if (tank.tankOptions.weapon == weaponid)
        {
            GetComponent<Image>().color = selected;
        }
        else
        {
            GetComponent<Image>().color = standard;
        }
    }

    public void Click()
    {
        tank.tankOptions.weapon = weaponid;
        WebData.playerData.weapon = weaponid;
        WebData.SaveStart();
    }
}
