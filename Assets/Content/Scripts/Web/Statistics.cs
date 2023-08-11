using System;
using System.Collections;
using System.Collections.Generic;
using Content.Scripts.Web.Classes;
using UnityEngine;
using Web;

public class Statistics : MonoBehaviour
{
    [SerializeField] private string URL;
    [SerializeField] private static float sessionTime;
    [SerializeField] private string userIP;
    [SerializeField] private static DateTime lastEnter;
    private void Start()
    {
        StartCoroutine(GetIP());
        lastEnter = DateTime.Now;
    }

    public IEnumerator GetIP()
    {
        var conenect = new WWW(URL);
        yield return conenect;
        if (conenect.error == null)
        {
            userIP = conenect.text;
        }
    }
    
    
    
    void Update()
    {
        if (WebDataService.data != null)
        {
            sessionTime += Time.deltaTime;
        }
    }

    public PlayerStats GetStats()
    {
        var lastStats = WebDataService.data.statistics.UnObfuscate();

        lastStats.lastEnter = lastEnter.ToString("yyyy-MM-dd HH:mm:ss");
        lastStats.lastIP = userIP;
        lastStats.lastSessionTime = sessionTime;

        return lastStats.Obfuscate();
    }
}
