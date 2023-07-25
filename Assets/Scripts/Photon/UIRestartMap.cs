using ExitGames.Client.Photon;
using Photon.Game;
using Photon.Pun;
using Scriptable;
using UnityEngine;
using UnityEngine.UI;

namespace Photon
{
    public class UIRestartMap : MonoBehaviour
    {
        [SerializeField] private GameDataObject gameData;
        [SerializeField] private Image preview;
        [SerializeField] private Slider sliderTime;
        
        
        private int selectedMap;
        
        
        public void ChangeMap()
        {
            selectedMap++;
            if (selectedMap >= gameData.MapsData.Count)
            {
                selectedMap = 0;
            }
            preview.sprite = gameData.MapsData.GetMapSprite(selectedMap);
        }

        public void RestartRoom()
        {       
            var rm = new ExitGames.Client.Photon.Hashtable();
            rm.Add("Mode", PhotonNetwork.CurrentRoom.CustomProperties["Mode"]);
            rm.Add("Map", selectedMap);
            rm.Add("Time", (int)sliderTime.value);
            if ((string)PhotonNetwork.CurrentRoom.CustomProperties["Mode"] == "TDM"){
                var redK = (int)PhotonNetwork.CurrentRoom.CustomProperties["RedKills"];
                var blueK = (int)PhotonNetwork.CurrentRoom.CustomProperties["BlueKills"];
                rm.Add("BlueKills", blueK);
                rm.Add("RedKills", redK);
            }
            PhotonNetwork.CurrentRoom.SetCustomProperties(rm);
            GameManager.Instance.SetTime(0f);
            Timer.Instance.SetTimer();
            object[] content = new object[] {};
            PhotonNetwork.RaiseEvent(0, content, new Photon.Realtime.RaiseEventOptions() { Receivers = Photon.Realtime.ReceiverGroup.All }, SendOptions.SendReliable);
        }
    }
}
