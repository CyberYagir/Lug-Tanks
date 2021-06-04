using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using TMPro;

public class Scores : MonoBehaviour
{
    public GameObject item, holder;
    float time;


    private void Update()
    {
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
                players = players.OrderBy(x => x.CustomProperties["k"]).ToList();
                players.Reverse();
                foreach (Transform item in holder.transform)
                {
                    Destroy(item.gameObject);
                }
                holder.GetComponent<RectTransform>().sizeDelta = new Vector2(holder.GetComponent<RectTransform>().sizeDelta.x, players.Count * item.GetComponent<RectTransform>().sizeDelta.y);
                for (int i = 0; i < players.Count; i++)
                {
                    var n = Instantiate(item.gameObject, holder.transform);
                    var rt = n.GetComponent<RectTransform>();
                    rt.anchoredPosition = new Vector2(0, i * -rt.sizeDelta.y);
                    rt.offsetMin = new Vector2(0, rt.offsetMin.y);
                    rt.offsetMax = new Vector2(-0, rt.offsetMax.y);
                    n.transform.GetChild(0).GetComponent<TMP_Text>().text = players[i].NickName;
                    var k = (int)players[i].CustomProperties["k"];
                    var d = (int)players[i].CustomProperties["d"];
                    n.transform.GetChild(1).GetComponent<TMP_Text>().text = (k).ToString();
                    n.transform.GetChild(2).GetComponent<TMP_Text>().text = (d).ToString();
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

