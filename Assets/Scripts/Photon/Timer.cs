using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance;
    public bool end;
    
    
    private bool startTimer = false;
    private double timerIncrementValue;
    private double startTime;
    
    [SerializeField] double timer = 20;
    
    ExitGames.Client.Photon.Hashtable CustomeValue;
    [SerializeField] private TMP_Text text;

    void Start()
    {
        SetTimer();
    }

    public void SetTimer()
    {
        Instance = this;
        end = false;
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
