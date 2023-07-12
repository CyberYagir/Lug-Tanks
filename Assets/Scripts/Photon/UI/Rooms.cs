using UnityEngine;

namespace Photon.UI
{
    public class Rooms : MonoBehaviour
    {
        [SerializeField] private Transform holder;
        [SerializeField] private Transform item;
        [SerializeField] private GameObject emptyText;
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
                    k.Init(r[i].Name.Split('_')[0], r[i].Name, r[i].PlayerCount + "/" + r[i].MaxPlayers);
                    n.SetActive(true);
                }
            }

            emptyText.SetActive(r.Count == 0);
        }
    }
}
