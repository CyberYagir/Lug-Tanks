using System;
using System.Collections;
using System.Collections.Generic;
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
        if (Tank.Controller.Tank.lastPlayer != null && Tank.Controller.Tank.lastPlayerClearTime < 10 && WeaponRotate.IsVisible(Tank.Controller.Tank.lastPlayer.gameObject))
        {
            indicator.gameObject.SetActive(true);
            hp.transform.localScale = new Vector3(Tank.Controller.Tank.lastPlayer.GetComponent<Tank.Controller.Tank>().tankOptions.hp / GetComponentInParent<Tank.Controller.Tank>().corpuses[Tank.Controller.Tank.lastPlayer.GetComponent<Tank.Controller.Tank>().tankOptions.corpus].hp, 1, 1);
            pname.text = Tank.Controller.Tank.lastPlayer.name;
            indicator.transform.position = Vector3.Lerp(indicator.transform.position, camera.WorldToScreenPoint(Tank.Controller.Tank.lastPlayer.GetComponent<Tank.Controller.Tank>().damageDisplayPoint.position, Camera.MonoOrStereoscopicEye.Mono), 10f * Time.deltaTime);
            bonus.SetActive(Tank.Controller.Tank.lastPlayer.GetComponent<Tank.Controller.Tank>().bonuses.Count != 0);
            
            for (int i = 0; i < bonuses.Count; i++)
            {
                bonuses[i].SetActive(Tank.Controller.Tank.lastPlayer.GetComponent<Tank.Controller.Tank>().bonuses.Contains(i));
            }
        }
        else
        {
            indicator.gameObject.SetActive(false);
        }
    }
}
