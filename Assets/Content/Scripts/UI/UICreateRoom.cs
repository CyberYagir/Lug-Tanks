using Photon;
using Scriptable;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UICreateRoom : MonoBehaviour
    {
        [SerializeField] private Slider players, time;
        [SerializeField] private Toggle isPublic;
        [SerializeField] private TMP_InputField mapName;
        [SerializeField] private GameDataObject.GameMode mode;
        [SerializeField] private MapsPreview preview;
        
        
        
        
        public void SetMode(int newMode)
        {
            mode = (GameDataObject.GameMode)newMode;
        }

        public void Create()
        {
            PhotonLobbyService.Instance.CreateRoom(mapName.text, isPublic.isOn, (byte)players.value, (int)time.value, preview.SelectedMap, mode.ToString());
        }
    }
}
