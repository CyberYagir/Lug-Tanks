using Photon;
using UnityEngine;

namespace Content.Scripts.UI
{
    public class TutorialWindow : MonoBehaviour
    {
        [SerializeField] private PhotonLobbyService photonLobbyService;
        public void Decine()
        {
            PlayerPrefs.GetString("Tutorial", "No");
            gameObject.SetActive(false);
        }

        public void GoToTutorial()
        {
            photonLobbyService.CreateAndJoinInTutorialRoom();
        }
    }
}