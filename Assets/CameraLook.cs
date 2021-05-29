using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public float sence;
    Transform parent;
    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent;
        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localEulerAngles += new Vector3(0, Input.GetAxis("Mouse X") * sence, 0);
    }

    private void FixedUpdate()
    {
        transform.position = parent.transform.position;
    }
}
