using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIServersButton : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private TMP_Text text;
        [SerializeField] private Image pingBar;
        [SerializeField] private TMP_Text ping;

        private const float maxPing = 120;
        private const float minPing = 30;
        
        private void Awake()
        {
            canvas.enabled = false;
            StartCoroutine(Loop());
        }

        IEnumerator Loop()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f / 10f);
                
                canvas.enabled = PhotonNetwork.IsConnectedAndReady;
            
                if (PhotonNetwork.IsConnectedAndReady)
                {
                    text.text = PhotonNetwork.NetworkingClient.CloudRegion;
                    var pingValue = PhotonNetwork.GetPing();
                    pingBar.fillAmount = Mathf.Clamp(1f - (((pingValue-minPing) / maxPing)), 0.1f, 1f);
                    pingBar.color = Color.Lerp(Color.red, Color.green, pingBar.fillAmount);

                    ping.text = "ping: " + pingValue;
                }
            }
        }
    }
}
