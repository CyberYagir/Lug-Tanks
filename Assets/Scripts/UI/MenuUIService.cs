﻿using Photon;
using Photon.Game.UI;
using Photon.Pun;
using UnityEngine;
using Web;

namespace UI
{
    public class MenuUIService : MonoBehaviour
    {
        [System.Serializable]
        public class Window
        {
            public Vector2 posHide, posOpen;
            public bool isOpen;
            public RectTransform rectTransform;

        }
        
        
        [SerializeField] private Window[] windows;
        [SerializeField] private ChangeButton[] buttons;
        [SerializeField] private PHPMenuService menuManager;
        [SerializeField] private RankIcon rankIcon;
        
        
        private Base.Controller.Tank tank;
    
        public void Init(Base.Controller.Tank tank)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            this.tank = tank;
            
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Init(tank);
            }
            
            rankIcon.Init(null);
        }
        private void Update()
        {
            AnimateWindows();
            
            rankIcon.UpdateElement();
            
            if (WebDataService.tankData != null && PhotonNetwork.IsConnected)
            {
                menuManager.gameObject.SetActive(false);

                ChangeCorpusWeapon();

                if (PhotonNetwork.InLobby)
                {
                    windows[0].isOpen = true;
                }
            }
            else
            {
                windows[0].rectTransform.anchoredPosition = windows[0].posHide;
                windows[0].isOpen = false;
                menuManager.gameObject.SetActive(true);
            }
        }

        private void ChangeCorpusWeapon()
        {
            bool isChanged = false;
            if (WebDataService.tankData.corpus != tank.tankOptions.corpus)
            {
                tank.tankOptions.ChangeCorpus(WebDataService.tankData.corpus);
                isChanged = true;
            }

            if (WebDataService.tankData.weapon != tank.tankOptions.weapon)
            {
                tank.tankOptions.ChangeWeapon(WebDataService.tankData.weapon);
                isChanged = true;
            }

            if (isChanged)
            {
                WebDataService.SaveStart();
            }
        }
        
        private void AnimateWindows()
        {
            for (int i = 0; i < windows.Length; i++)
            {
                if (windows[i].isOpen)
                {
                    windows[i].rectTransform.anchoredPosition = Vector2.Lerp(windows[i].rectTransform.anchoredPosition, windows[i].posOpen, 5f * Time.deltaTime);
                }
                else
                {
                    windows[i].rectTransform.anchoredPosition = Vector2.Lerp(windows[i].rectTransform.anchoredPosition, windows[i].posHide, 5f * Time.deltaTime);
                }
            }
        }

        public void OpenWindow(int n)
        {
            for (int i = 1; i < windows.Length; i++)
            {
                if (i == n)
                {
                    windows[i].isOpen = !windows[i].isOpen;
                }
                else
                {
                    windows[i].isOpen = false;
                }
            }
        }
    }
}
