using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{

    void Update()
    {
        var manager = FindObjectOfType<GameManager>();
        GetComponent<TMP_Text>().text = "" + (int)(1f / Time.unscaledDeltaTime) + "/";

        if (manager.LocalPlayer != null)
        {
            GetComponent<TMP_Text>().text += manager.LocalPlayer.photonView.Owner.IsMasterClient;
        }
    }
}
