using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Scriptable;
using TMPro;
using UnityEngine;

namespace Photon.UI
{
    public class UIRooms : MonoBehaviour
    {
        [SerializeField] private Transform holder;
        [SerializeField] private Transform item;
        [SerializeField] private GameObject emptyText;
        [SerializeField] private GameDataObject gameData;

        public void UpdateRooms(List<RoomInfo> infos)
        {
            var r = infos;
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
                        (string)r[i].CustomProperties["RoomName"],
                        r[i].Name,
                        r[i].PlayerCount + "/" + r[i].MaxPlayers,
                        gameData.MapsData.GetMapSprite(map),
                        gameData.GameModesData.StringToMode((string) r[i].CustomProperties["Mode"]).Sprite
                    );


                    n.SetActive(true);
                }
            }

            emptyText.SetActive(r.FindAll(x => x.IsOpen && x.IsVisible).Count == 0);
        }

        public void JoinByGUID(TMP_InputField text)
        {
            Debug.LogError(text.text);
            PhotonLobbyService.Instance.JoinRoom(text.text);
        }
        
    }
}
