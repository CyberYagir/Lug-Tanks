using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look : MonoBehaviour
{
    SpriteRenderer renderer;
    float transp;
    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (Camera.main != null)
        {
            renderer.color = Color.Lerp(renderer.color, new Color(1, 1, 1, transp), 5f * Time.deltaTime);
            transform.LookAt(new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z));
            if (Vector3.Distance(transform.position, Camera.main.transform.parent.position) < 4f)
            {
                transp = 0.5f;
            }
            else
            {
                transp = 1f;
            }
        }
    }
}
