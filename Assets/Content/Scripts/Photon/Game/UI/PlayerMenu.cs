using Content.Scripts.Anticheat;
using Photon.Pun;
using TMPro;
using UnityEngine;
using Web;

namespace Photon.Game.UI
{
    public class PlayerMenu : TankUIElement
    {
        [SerializeField] private Animator animator;
        [SerializeField] private TMP_Text roomGUID;
        private bool open;
        private float time;


        public override void Init(Player player)
        {
            base.Init(player);

            roomGUID.text = PhotonNetwork.CurrentRoom.Name;
        }

        public override void UpdateElement()
        {
            base.UpdateElement();
            
            time += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Escape) && time > 0.5f)
            {
                ChangeMode();
            }
        }

        public void Continue(){
            ChangeMode();
        }

        public void Suicide(){
            GameManager.Instance.GetPlayerTank().tankOptions.hp = 0.Obf();
        }

        public void Disconnect(){
            WebDataService.SaveStart();
            GameManager.Instance.Disconnect();
        }

        public void CopyGUID()
        {
            GUIUtility.systemCopyBuffer = PhotonNetwork.CurrentRoom.Name;
        }

        public void ChangeMode()
        {
            if (open)
            {
                animator.Play("HideMenu");
            }
            else
            {
                animator.Play("OpenMenu");
            }

            open = !open;
            GameManager.IsOnPause = open;
            time = 0;
        }
    }
}
