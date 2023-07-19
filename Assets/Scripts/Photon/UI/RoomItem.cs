using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Photon.UI
{
    public class RoomItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text roomName, playersCount;
        [SerializeField] private Image icon, modeIcon;
        [SerializeField] private string roomsStringName;
        public void Connect()
        {
            PhotonLobbyService.Instance.JoinRoom(roomsStringName);
        }

        public void Init(string rname, string text, string count, Sprite mapIcon, Sprite modeIcon)
        {
            this.roomsStringName = text;
            this.roomName.text = rname;
            this.playersCount.text = count;
            this.icon.sprite = mapIcon;
            this.modeIcon.sprite = modeIcon;
        }
    }
}
