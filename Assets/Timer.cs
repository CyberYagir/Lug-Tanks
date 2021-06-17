using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Test {
    public Vector3 pos;

}



public class Timer : MonoBehaviour
{
    public static Timer timer_;
    bool startTimer = false;
    double timerIncrementValue;
    double startTime;
    [SerializeField] double timer = 20;
    ExitGames.Client.Photon.Hashtable CustomeValue;
    public TMP_Text text;
    public bool end;

    void Start()
    {
        timer_ = this;
        timer = (int)PhotonNetwork.CurrentRoom.CustomProperties["Time"];
        if (PhotonNetwork.IsMasterClient)
        {
            CustomeValue = new ExitGames.Client.Photon.Hashtable();
            startTime = PhotonNetwork.Time;
            startTimer = true;
            CustomeValue.Add("StartTime", startTime);
            PhotonNetwork.CurrentRoom.SetCustomProperties(CustomeValue);
        }
        else
        {
            startTime = double.Parse(PhotonNetwork.CurrentRoom.CustomProperties["StartTime"].ToString());
            startTimer = true;
        }
    }

    void Update()
    {
        if (!startTimer) return;
        timerIncrementValue = PhotonNetwork.Time - startTime;
        text.text = timerIncrementValue.ToString("F0") + "/" + timer ;
        if ((int)timerIncrementValue >= timer)
        {
            end = true;
        }
    }
}
