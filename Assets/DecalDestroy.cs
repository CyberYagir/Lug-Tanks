using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalDestroy : MonoBehaviour
{
    public float fadeSpeed;
    float time;
    void Update()
    {
        time += Time.deltaTime;
        if (time > 2)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, fadeSpeed * Time.deltaTime);
            if (transform.localScale.x < 0.01f) Destroy(gameObject);
        }
    }
}
