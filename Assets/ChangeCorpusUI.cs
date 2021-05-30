using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCorpusUI : MonoBehaviour
{
    public int corpusid;
    public Color standard, selected;
    Tank tank;

    private void Start()
    {
        tank = FindObjectOfType<Tank>();
    }
    private void Update()
    {
        if (tank.tankOptions.corpus == corpusid)
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
        FindObjectOfType<Tank>().tankOptions.corpus = corpusid;
    }
}
