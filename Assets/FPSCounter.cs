using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    void Update()
    {
        GetComponent<TMP_Text>().text = "" + (int)(1f / Time.unscaledDeltaTime);
    }
}
