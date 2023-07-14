using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using Photon.Game;
using Photon.UI;
using TMPro;
using UnityEngine.UI;

public class Scores : MonoBehaviour
{
    [SerializeField] private GameObject item, holder;
    [SerializeField] private List<Color> teamColors;
    [SerializeField] private GameObject manageButton;
    [SerializeField] private Sprite[] ranks;
    
    private float time = 1f;
    public void Disconnect()
    {
        GameManager.Instance.Disconnect();
    }
    public void OpenClose(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.active);
    }

    private void OnDisable()
    {
        time = 1f;
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

                for (int i = 0; i < players.Count; i++)
                {
                    var scoresItem = Instantiate(item.gameObject, holder.transform).GetComponent<ScoresItem>();
                    
                    
                    var k = (int)players[i].CustomProperties["k"];
                    var d = (int)players[i].CustomProperties["d"];

                    
                    scoresItem.Init(
                        teamColors[(int)players[i].CustomProperties["Team"]],
                        players[i].NickName,
                        k.ToString(),
                        d.ToString(),
                        (d == 0 ? k/1f : k / (float)d).ToString("F2"),
                        ranks[RankIcon.GetRank((int)players[i].CustomProperties["Exp"], ranks)]
                    );
                    
                    scoresItem.gameObject.SetActive(true);
                }
            }
        }
    }


}

