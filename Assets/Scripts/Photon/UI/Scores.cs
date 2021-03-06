using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class Scores : MonoBehaviour
{
    [SerializeField] private GameObject item, holder;
    [SerializeField] private List<Color> teamColors;
    [SerializeField] private GameObject manageButton;
    [SerializeField] private Sprite[] ranks;
    
    private float time;
    public void Disconnect()
    {
        GameManager.Instance.Disconnect();
    }
    public void OpenClose(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.active);
    }
    private void Update()
    {
        manageButton.SetActive(Timer.Instance.end && PhotonNetwork.LocalPlayer.IsMasterClient);
        time += Time.deltaTime;
        if (time > 0.5f)
        {
            time = 0;
            if (PhotonNetwork.InRoom)
            {
                List<Photon.Realtime.Player> players = new List<Photon.Realtime.Player>();
                foreach (var pl in PhotonNetwork.CurrentRoom.Players)
                {
                    players.Add(pl.Value);
                }
                players = players.OrderBy(x => (int)x.CustomProperties["Team"]).ThenBy(x=>(int)x.CustomProperties["k"]).ToList();
                players.Reverse();
                foreach (Transform item in holder.transform)
                {
                    Destroy(item.gameObject);
                }
                holder.GetComponent<RectTransform>().sizeDelta = new Vector2(holder.GetComponent<RectTransform>().sizeDelta.x, players.Count * item.GetComponent<RectTransform>().sizeDelta.y);
                for (int i = 0; i < players.Count; i++)
                {
                    var n = Instantiate(item.gameObject, holder.transform);
                    n.GetComponent<Image>().color = teamColors[(int)players[i].CustomProperties["Team"]];
                    var rt = n.GetComponent<RectTransform>();
                    rt.anchoredPosition = new Vector2(0, i * -rt.sizeDelta.y);
                    rt.offsetMin = new Vector2(0, rt.offsetMin.y);
                    rt.offsetMax = new Vector2(-0, rt.offsetMax.y);



                    n.transform.GetChild(0).GetComponent<TMP_Text>().text = players[i].NickName;
                    var k = (int)players[i].CustomProperties["k"];
                    var d = (int)players[i].CustomProperties["d"];
                    n.transform.GetChild(1).GetComponent<TMP_Text>().text = (k).ToString();
                    n.transform.GetChild(2).GetComponent<TMP_Text>().text = (d).ToString();
                    n.transform.GetChild(n.transform.childCount - 1).GetComponent<Image>().sprite = ranks[RankIcon.GetRank((int)players[i].CustomProperties["Exp"], ranks)];
                    if (d == 0)
                        n.transform.GetChild(3).GetComponent<TMP_Text>().text = ((float)k / (float)1f).ToString("F2");
                    else
                        n.transform.GetChild(3).GetComponent<TMP_Text>().text = ((float)k / (float)d).ToString("F2");
                    n.SetActive(true);
                }
            }
        }
    }


}

