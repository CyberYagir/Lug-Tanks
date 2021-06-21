using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankIcon : MonoBehaviour
{
    public TMP_Text pName, exp_text;
    public Sprite[] sprites;
    public int currRank;
    public int startMaxExp = 200;

    public RectTransform expLine;
    private void Update()
    {
        if (WebData.playerData != null)
        {
            var rankIcon = GetComponent<RectTransform>();

            pName.text = WebData.playerData.name;
            var currentXp = (startMaxExp * (((currRank) + 1f) * (1.25f * (currRank))));
            var nextXp = (startMaxExp * (((currRank + 1) + 1f) * (1.25f * (currRank + 1))));
            expLine.localScale = Vector3.Lerp(expLine.localScale, new Vector3((WebData.playerData.exp - currentXp) / (nextXp - currentXp), 1, 1), 6f * Time.deltaTime);
            currRank = 0;
            exp_text.text = (int)(WebData.playerData.exp - currentXp) + "/" + (int)(nextXp - currentXp);
            for (int i = 0; i < sprites.Length; i++)
            {
                if (startMaxExp * ((i+1f) * (1.25f * i)) < WebData.playerData.exp)
                {
                    currRank = i;
                }
            }
            GetComponent<Image>().sprite = sprites[currRank];
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
