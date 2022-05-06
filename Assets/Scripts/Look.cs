using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look : MonoBehaviour
{
    private SpriteRenderer renderer;
    private float transp;
    private Collider col;

    private Camera camera;
    private void Start()
    {
        col = GetComponent<Collider>();
        renderer = GetComponent<SpriteRenderer>();
        camera = Camera.main;
    }
    void Update()
    {
        if (camera != null)
        {
            renderer.color = Color.Lerp(renderer.color, new Color(1, 1, 1, transp), 5f * Time.deltaTime);
            transform.LookAt(new Vector3(camera.transform.position.x, transform.position.y, camera.transform.position.z));
            if (Vector3.Distance(transform.position, camera.transform.parent.position) < 4f)
            {
                col.enabled = false;
                transp = 0.5f;

            }
            else
            {
                col.enabled = true;
                transp = 1f;
            }
        }
    }
}
