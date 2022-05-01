using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KillsList : MonoBehaviour
{
    public static KillsList killsList;
    public GameObject holder;
    public GameObject item;
    public Sprite[] icons;

    private void Start()
    {
        killsList = this;
    }

    public void Create(string playerKiller, string playerKilled, int weapon)
    {
        var it = Instantiate(item.gameObject, holder.transform);
        it.transform.GetChild(0).GetComponent<TMP_Text>().text = playerKiller;
        it.transform.GetChild(1).GetComponent<Image>().sprite = icons[weapon];
        it.transform.GetChild(2).GetComponent<TMP_Text>().text = playerKilled;
        it.SetActive(true);
        Destroy(it.gameObject, 3);
    }
}
