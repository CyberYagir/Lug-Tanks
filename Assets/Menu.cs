using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public RankIcon rankIcon;
    public RectTransform rectTransform;
    public TMP_Text pName;
    public Image icon;
    public RectTransform expLine;
    public PHPMenuManager menuManager;

    float initX;
    private void Start()
    {
        initX = rectTransform.position.x;
    }
    private void Update()
    {
        if (WebData.playerData != null && PhotonNetwork.IsConnected)
        {
            menuManager.gameObject.SetActive(false);
            pName.text = WebData.playerData.name;
            rectTransform.position = Vector3.Lerp(rectTransform.position, new Vector3(0, rectTransform.position.y, 0), 4f * Time.deltaTime);
            var currentXp = (rankIcon.startMaxExp * (((rankIcon.currRank) + 1f) * (1.25f * (rankIcon.currRank))));
            var nextXp = (rankIcon.startMaxExp * (((rankIcon.currRank + 1) + 1f) * (1.25f * (rankIcon.currRank + 1))));
            expLine.localScale = Vector3.Lerp(expLine.localScale, new Vector3((WebData.playerData.exp - currentXp) / (nextXp - currentXp), 1, 1), 6f * Time.deltaTime);
        }
        else
        {
            menuManager.gameObject.SetActive(true);
            rectTransform.position = Vector3.Lerp(rectTransform.position, new Vector3(initX, rectTransform.position.y, 0), 4f * Time.deltaTime);
        }
    }
}
