using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TeamsCounters : MonoBehaviour
{
    public TMP_Text redT, blueT;

    private void Start()
    {
        if ((string)PhotonNetwork.CurrentRoom.CustomProperties["Mode"] != "TDM") gameObject.SetActive(false);
    }
    private void Update()
    {
        redT.text = PhotonNetwork.CurrentRoom.CustomProperties["RedKills"].ToString();
        blueT.text = PhotonNetwork.CurrentRoom.CustomProperties["BlueKills"].ToString();
    }
}
