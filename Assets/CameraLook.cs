﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public float sence;
    Transform parent;
    Tank tank;
    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent;
        foreach (var item in FindObjectsOfType<CameraLook>())
        {
            if (item != this) Destroy(item.gameObject);
        }
        if (parent)
        {
            tank = parent.GetComponentInParent<Tank>();
        }
        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (parent)
            transform.localEulerAngles += new Vector3(0, Input.GetAxis("Mouse X") * sence, 0);
    }

    private void FixedUpdate()
    {
        if (parent != null)
            transform.position = new Vector3(tank.corpuses[tank.tankOptions.corpus].weaponPoint.transform.position.x, parent.transform.position.y, tank.corpuses[tank.tankOptions.corpus].weaponPoint.transform.position.z);
    }
}
