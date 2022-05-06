using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCorpusUI : MonoBehaviour
{
    [SerializeField] private int corpusid;
    [SerializeField] private Color standard, selected;
    private Image image;
    private Tank tank;

    
    

    private void Start()
    {
        tank = FindObjectOfType<Tank>();
        image = GetComponent<Image>();
        tank = FindObjectOfType<Tank>();
    }
    private void Update()
    {
        if (tank.tankOptions.corpus == corpusid)
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
        tank.tankOptions.corpus = corpusid;
        WebData.tankData.corpus = corpusid;
        WebData.SaveStart();
    }
}
