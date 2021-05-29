using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankIcon : MonoBehaviour
{
    public Sprite[] sprites;
    public int currRank;
    public int startMaxExp = 200;
    private void Update()
    {
        if (WebData.playerData != null)
        {
            currRank = 0;
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
}
