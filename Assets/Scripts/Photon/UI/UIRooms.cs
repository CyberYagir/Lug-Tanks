using Scriptable;
using UnityEngine;

namespace Photon.UI
{
    public class UIRooms : MonoBehaviour
    {
        [SerializeField] private Transform holder;
        [SerializeField] private Transform item;
        [SerializeField] private GameObject emptyText;
        [SerializeField] private GameDataObject gameData;
        
        public void UpdateRooms()
        {
            var r = PhotonLobbyService.Instance.rooms;
            foreach (Transform item in holder)
            {
                Destroy(item.gameObject);
            }

            for (int i = 0; i < r.Count; i++)
            {
                if (r[i].PlayerCount != r[i].MaxPlayers)
                {
                    var n = Instantiate(item.gameObject, holder);
                    var k = n.GetComponent<RoomItem>();

                    var map = 0;
                    if (r[i].CustomProperties["Map"] != null)
                    {
                        map = (int) r[i].CustomProperties["Map"];
                    }

                    k.Init(
                        r[i].Name.Split('_')[0],
                        r[i].Name,
                        r[i].PlayerCount + "/" + r[i].MaxPlayers,
                        gameData.MapsData.GetMapSprite(map),
                        gameData.GameModesData.StringToMode((string) r[i].CustomProperties["Mode"]).Sprite
                    );


                    n.SetActive(true);
                }
            }

            emptyText.SetActive(r.Count == 0);
        }
    }
}
