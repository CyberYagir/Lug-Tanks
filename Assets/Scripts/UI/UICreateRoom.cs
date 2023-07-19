using Photon;
using Scriptable;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UICreateRoom : MonoBehaviour
    {
        [SerializeField] private GameDataObject gameData;
        [SerializeField] private Image preview;
        [SerializeField] private Slider players, time;
        [SerializeField] private Toggle isPublic;
        [SerializeField] private TMP_InputField mapName;
        [SerializeField] private GameDataObject.GameMode mode;
        
        private int selectedMap;
        
        
        public void SetMode(int newMode)
        {
            mode = (GameDataObject.GameMode)newMode;
        }
        public void ChangeMap()
        {
            selectedMap++;
            if (selectedMap >= gameData.MapsData.Count)
            {
                selectedMap = 0;
            }

            preview.sprite = gameData.MapsData.GetMapSprite(selectedMap);
        }

        public void Create()
        {
            PhotonLobbyService.Instance.CreateRoom(mapName.text, isPublic.isOn, (byte)players.value, (int)time.value, selectedMap, mode.ToString());
        }
    }
}
