using System;
using Content.Scripts.Anticheat;
using Photon;
using UnityEngine;
using Web;

namespace Menu
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private Base.Controller.Tank tankService;
        [SerializeField] private UI.MenuUIService menuService;
        [SerializeField] private PHPMenuService phpMenuService;
        [SerializeField] private PhotonLobbyService lobbyService;
        [SerializeField] private WebDataService webDataService;
        private void Awake()
        {
            ObfuseExtensions.Init();
            tankService.tankOptions = tankService.tankOptions.Obfuscate();
            lobbyService.Init();
            webDataService.Init(); 
            menuService.Init(tankService, lobbyService);
            phpMenuService.Init(tankService);
        }

        private void Start()
        {
            Application.targetFrameRate = 120;
        }
    }
}
