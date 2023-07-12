using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Web;

public class RankIcon : MonoBehaviour
{
    [SerializeField] TMP_Text pName, exp_text;
    [SerializeField] Sprite[] sprites;
    [SerializeField] int currRank;
    [SerializeField] int startMaxExp = 200;

    [SerializeField] private RectTransform expLine;

    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if (WebDataService.tankData != null)
        {
            pName.text = WebDataService.data.playerData.name;
            var currentXp = (startMaxExp * (((currRank) + 1f) * (1.25f * (currRank))));
            var nextXp = (startMaxExp * (((currRank + 1) + 1f) * (1.25f * (currRank + 1))));
            expLine.localScale = Vector3.Lerp(expLine.localScale, new Vector3((WebDataService.tankData.exp - currentXp) / (nextXp - currentXp), 1, 1), 6f * Time.deltaTime);
            currRank = 0;
            exp_text.text = (int)(WebDataService.tankData.exp - currentXp) + "/" + (int)(nextXp - currentXp);
            for (int i = 0; i < sprites.Length; i++)
            {
                if (startMaxExp * ((i+1f) * (1.25f * i)) < WebDataService.tankData.exp)
                {
                    currRank = i;
                }
            }
            image.sprite = sprites[currRank];
        }
    }

    public static int GetRank(int exp, Sprite[] sprites)
    {
        int currRank = 0;
        for (int i = 0; i < sprites.Length; i++)
        {
            if (100f * ((i + 1f) * (1.25f * i)) < exp)
            {
                currRank = i;
            }
        }
        return currRank;
    }
}
