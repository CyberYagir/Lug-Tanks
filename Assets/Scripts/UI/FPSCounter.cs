using System;
using Photon.Game;
using TMPro;
using UnityEngine;

namespace UI
{
    public class FPSCounter : MonoBehaviour
    {
        private TMP_Text text;

        private void Awake()
        {
            text = GetComponent<TMP_Text>();
        }

        void Update()
        {
            text.text = "" + (int)(1f / Time.unscaledDeltaTime) + "/";
        }
    }
}
