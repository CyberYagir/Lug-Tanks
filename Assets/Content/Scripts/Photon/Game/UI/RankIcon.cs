using CrazyGames;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Web;

namespace Photon.Game.UI
{
    public class RankIcon : TankUIElement
    {
        [SerializeField] private TMP_Text playerNameT, xpT;
        [SerializeField] private Image rankIcon;
        [SerializeField] private RectTransform expLine;
        [Space] 
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private int currRank;
        [SerializeField] private int startMaxExp = 50;

        private int oldRank = -1;


        public override void UpdateElement()
        {
            if (WebDataService.tankData != null)
            {
                playerNameT.text = WebDataService.data.playerData.name;
                var currentXp = (startMaxExp * (((currRank) + 1f) * (1.25f * (currRank))));
                var nextXp = (startMaxExp * (((currRank + 1) + 1f) * (1.25f * (currRank + 1))));
                expLine.localScale = Vector3.Lerp(expLine.localScale, new Vector3((WebDataService.tankData.exp - currentXp) / (nextXp - currentXp), 1, 1), 6f * Time.deltaTime);
                currRank = 0;
                xpT.text = (int)(WebDataService.tankData.exp - currentXp) + "/" + (int)(nextXp - currentXp);
                for (int i = 0; i < sprites.Length; i++)
                {
                    if (startMaxExp * ((i+1f) * (1.25f * i)) < WebDataService.tankData.exp)
                    {
                        currRank = i;
                    }
                }
                rankIcon.sprite = sprites[currRank];

                if (oldRank == -1)
                {
                    oldRank = currRank;
                }else if (oldRank != currRank)
                {
                    CrazyEvents.Instance.HappyTime();
                }
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
}
