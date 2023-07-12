using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public static CameraLook Instance;
    [SerializeField] private float sence;
    [SerializeField] private Transform maxpoint;
    [SerializeField] private Camera camera;
    private Transform parent;
    private Tank.Controller.Tank tank;


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        parent = transform.parent;
        foreach (var item in FindObjectsOfType<CameraLook>())
        {
            if (item != this) Destroy(item.gameObject);
        }
        if (parent)
        {
            tank = parent.GetComponentInParent<Tank.Controller.Tank>();
        }
        sence = PlayerPrefs.GetFloat("Sens", 1);
        transform.parent = null;
    }
    
    void Update()
    {
        if (parent && !GameManager.pause)
        {

            transform.localEulerAngles += new Vector3(0, (Input.GetAxis("Mouse X") + Input.GetAxis("Turret")) * sence, 0);
            RaycastHit back, up;
            Physics.Raycast(tank.weapons[tank.tankOptions.weapon].minPoint.transform.position, -tank.weapons[tank.tankOptions.weapon].minPoint.transform.forward, out back, Mathf.Abs(maxpoint.localPosition.z));
            Physics.Raycast(tank.weapons[tank.tankOptions.weapon].minPoint.transform.position, tank.weapons[tank.tankOptions.weapon].minPoint.transform.up, out up, maxpoint.localPosition.y);

            Vector3 finalPos = maxpoint.localPosition;

            if (back.collider != null)
            {
                if (transform.InverseTransformPoint(back.point).z > finalPos.z)
                {
                    finalPos = new Vector3(0, finalPos.y, transform.InverseTransformPoint(back.point).z);
                }
            }

            if (up.collider != null)
            {
                if (transform.InverseTransformPoint(up.point).y < finalPos.y)
                {
                    finalPos = new Vector3(0, transform.InverseTransformPoint(up.point).y, finalPos.z);
                }
            }
            camera.transform.localPosition = Vector3.Lerp(camera.transform.localPosition, finalPos + new Vector3(0,0,0.2f), 5 * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        if (parent != null)
            transform.position = new Vector3(tank.corpuses[tank.tankOptions.corpus].weaponPoint.transform.position.x, parent.transform.position.y, tank.corpuses[tank.tankOptions.corpus].weaponPoint.transform.position.z);
    }

    public Camera GetCamera()
    {
        return camera;
    }
}
