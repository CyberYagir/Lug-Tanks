using System;
using System.Collections;
using System.Collections.Generic;
using Base.Weapons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    [SerializeField] private Image hp;
    [SerializeField] private GameObject indicator;
    [SerializeField] private TMP_Text pname;
    [SerializeField] private List<GameObject> bonuses;
    [SerializeField] private GameObject bonus;


    private Camera camera;

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        if (Base.Controller.Tank.lastPlayer != null && Base.Controller.Tank.lastPlayerClearTime < 10 && WeaponRotate.IsVisible(Base.Controller.Tank.lastPlayer.gameObject))
        {
            indicator.gameObject.SetActive(true);
            hp.transform.localScale = new Vector3(Base.Controller.Tank.lastPlayer.GetComponent<Base.Controller.Tank>().tankOptions.hp / GetComponentInParent<Base.Controller.Tank>().corpuses[Base.Controller.Tank.lastPlayer.GetComponent<Base.Controller.Tank>().tankOptions.corpus].hp, 1, 1);
            pname.text = Base.Controller.Tank.lastPlayer.name;
            indicator.transform.position = Vector3.Lerp(indicator.transform.position, camera.WorldToScreenPoint(Base.Controller.Tank.lastPlayer.GetComponent<Base.Controller.Tank>().damageDisplayPoint.position, Camera.MonoOrStereoscopicEye.Mono), 10f * Time.deltaTime);
            bonus.SetActive(Base.Controller.Tank.lastPlayer.GetComponent<Base.Controller.Tank>().bonuses.Count != 0);
            
            for (int i = 0; i < bonuses.Count; i++)
            {
                bonuses[i].SetActive(Base.Controller.Tank.lastPlayer.GetComponent<Base.Controller.Tank>().bonuses.Contains(i));
            }
        }
        else
        {
            indicator.gameObject.SetActive(false);
        }
    }
}
