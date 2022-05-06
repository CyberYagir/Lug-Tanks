using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    private TMP_Text text;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
        
    }

    void Update()
    {
        text.text = "" + (int)(1f / Time.unscaledDeltaTime) + "/";

        if (GameManager.Instance.LocalPlayer != null)
        {
            text.text += GameManager.Instance.LocalPlayer.photonView.Owner.IsMasterClient;
        }
    }
}
