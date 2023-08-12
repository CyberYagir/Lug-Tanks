using Content.Scripts.Anticheat;
using CrazyGames;
using DG.Tweening;
using Photon.Pun;
using Scriptable;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Web;

namespace Photon.Game.UI
{
    public class RankIcon : TankUIElement
    {
        [SerializeField] private GameDataObject gameData;
        [SerializeField] private TMP_Text playerNameT, xpT;
        [SerializeField] private Image rankIcon;
        [SerializeField] private RectTransform expLine;
        [Space] 
        [SerializeField] private int currRank;

        private int oldRank = -1;
        private int oldExp = 0;

        private GameDataObject.LevelProgress levelProgress => gameData.PlayerLevelsData;
        private const float LEVEL_PROGRESS_CONSTANT = 1.25f;

        public override void Init(Player player)
        {
            base.Init(player);
            
            if (WebDataService.TankData != null)
            {
                InitRank();
            }
            else
            {
                WebDataService.Instance.OnLogin.AddListener(InitRank);
            }
        }


        
        private void InitRank()
        {

            currRank = GetRank(WebDataService.TankData.Exp, gameData);
            rankIcon.sprite = levelProgress.GetIcon(currRank);
            
            playerNameT.text = WebDataService.UserData.PlayerData.Name;
            
            
            CurrentXp(out var currentXp, out var nextXp);
            UpdateVisual(currentXp, nextXp, false);
            
            
            oldExp = WebDataService.TankData.Exp;
            oldRank = currRank;
        }

        public override void UpdateElement()
        {
            if (WebDataService.TankData != null)
            {
                if (WebDataService.TankData.Exp != oldExp)
                {
                    currRank = GetRank(WebDataService.TankData.Exp, gameData);
                    rankIcon.sprite = levelProgress.GetIcon(currRank);
                    CurrentXp(out var currentXp, out var nextXp);
                    UpdateVisual(currentXp, nextXp, true);
                    
                    if (oldRank != currRank)
                    {
                        CrazyEvents.Instance.HappyTime();
                    }

                    oldExp = WebDataService.TankData.Exp;
                }
            }
        }

        private void CurrentXp(out float currentXp, out float nextXp)
        {
            currentXp = CaluclateXpLevel(currRank);
            nextXp = CaluclateXpLevel(currRank + 1);
        }



        private void UpdateVisual(float currentLevelXp, float nextLevelXp, bool animate)
        {
            var nextLevelXP = nextLevelXp - currentLevelXp;
            var currentXP = WebDataService.TankData.Exp - currentLevelXp;
            
            var scaleX = currentXP / nextLevelXP;

            if (animate)
            {
                expLine.DOScaleX(scaleX, 0.2f);
            }
            else
            {
                expLine.localScale = new Vector3(scaleX, 1, 1);
            }

            xpT.text = (int)currentXP + "/" + (int)nextLevelXP;
        }

        public static int GetRank(int exp, GameDataObject gameData)
        {
            int currRank = 0;
            for (int i = 0; i < gameData.PlayerLevelsData.LevelsCount; i++)
            {
                if (CaluclateXpLevel(i, gameData) <= exp)
                {
                    currRank = i;
                }
            }
            return currRank;
        }
        public static float CaluclateXpLevel(int rank, GameDataObject gameData)
        {
            return (gameData.PlayerLevelsData.StartMaxExp * ((rank + 1) * (LEVEL_PROGRESS_CONSTANT * rank)));
        }
        
        public float CaluclateXpLevel(int rank)
        {
            return CaluclateXpLevel(rank, gameData);
        }
    }
}
