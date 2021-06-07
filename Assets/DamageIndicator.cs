using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    public Image hp;
    public GameObject indicator;
    public TMP_Text pname;
    private void Update()
    {
        if (Tank.lastPlayer != null && Tank.lastPlayerClearTime < 10)
        {
            indicator.gameObject.SetActive(true);
            hp.transform.localScale = new Vector3(Tank.lastPlayer.GetComponent<Tank>().tankOptions.hp / GetComponentInParent<Tank>().corpuses[Tank.lastPlayer.GetComponent<Tank>().tankOptions.corpus].hp, 1, 1);
            pname.text = Tank.lastPlayer.name;
            indicator.transform.position = Camera.main.WorldToScreenPoint(Tank.lastPlayer.GetComponent<Tank>().damageDisplayPoint.position, Camera.MonoOrStereoscopicEye.Mono);

        }
        else { indicator.gameObject.SetActive(false); }
    }
}
