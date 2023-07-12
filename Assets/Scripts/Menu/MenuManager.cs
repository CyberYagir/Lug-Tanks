using System;
using Photon;
using UnityEngine;
using Web;

namespace Menu
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private Tank.Controller.Tank tankService;
        [SerializeField] private UI.MenuUIService menuService;
        [SerializeField] private PHPMenuService phpMenuService;
        [SerializeField] private PhotonLobbyService lobbyService;
        [SerializeField] private WebDataService webDataService;
        private void Awake()
        {
            lobbyService.Init();
            webDataService.Init();
            menuService.Init(tankService);
            phpMenuService.Init(tankService);
        }

        private void Start()
        {
            Application.targetFrameRate = 120;
        }
    }
}
