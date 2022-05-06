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
        if (Tank.lastPlayer != null && Tank.lastPlayerClearTime < 10 && WeaponRotate.IsVisible(Tank.lastPlayer.gameObject))
        {
            indicator.gameObject.SetActive(true);
            hp.transform.localScale = new Vector3(Tank.lastPlayer.GetComponent<Tank>().tankOptions.hp / GetComponentInParent<Tank>().corpuses[Tank.lastPlayer.GetComponent<Tank>().tankOptions.corpus].hp, 1, 1);
            pname.text = Tank.lastPlayer.name;
            indicator.transform.position = Vector3.Lerp(indicator.transform.position, camera.WorldToScreenPoint(Tank.lastPlayer.GetComponent<Tank>().damageDisplayPoint.position, Camera.MonoOrStereoscopicEye.Mono), 10f * Time.deltaTime);
            bonus.SetActive(Tank.lastPlayer.GetComponent<Tank>().bonuses.Count != 0);
            
            for (int i = 0; i < bonuses.Count; i++)
            {
                bonuses[i].SetActive(Tank.lastPlayer.GetComponent<Tank>().bonuses.Contains(i));
            }
        }
        else
        {
            indicator.gameObject.SetActive(false);
        }
    }
}
