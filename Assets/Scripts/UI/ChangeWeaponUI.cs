using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeWeaponUI : MonoBehaviour
{
    [SerializeField] private int weaponid;
    [SerializeField] private Color standard, selected;
    private Image image;
    private Tank tank;
    private void Start()
    {
        tank = FindObjectOfType<Tank>();
        image = GetComponent<Image>();
    }
    private void Update()
    {
        if (tank.tankOptions.weapon == weaponid)
        {
            image.color = selected;
        }
        else
        {
            image.color = standard;
        }
    }

    public void Click()
    {
        tank.tankOptions.weapon = weaponid;
        WebData.tankData.weapon = weaponid;
        WebData.SaveStart();
    }
}
