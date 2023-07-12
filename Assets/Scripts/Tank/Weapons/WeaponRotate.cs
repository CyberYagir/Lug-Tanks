using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Game;
using UnityEngine;

public class WeaponRotate : MonoBehaviour
{
    public Tank.Controller.Tank tank;
    public Transform shootCamera;
    public static Camera shootCam;
    public float rotateSpeed;

    public static bool IsVisible(GameObject gm)
    {
        try
        {
            return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(shootCam), gm.GetComponentInChildren<Collider>().bounds);

        }
        catch (System.Exception)
        {
            return false;
        }
    }
    private void Start()
    {
        if (GetComponentInParent<Player>().photonView.IsMine)
        {
            shootCam = shootCamera.GetComponent<Camera>();
        }
    }
    private void Update()
    {
        transform.position = tank.corpuses[tank.tankOptions.corpus].weaponPoint.transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, tank.cameraLook.transform.rotation, rotateSpeed * Time.deltaTime);
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        shootCamera.transform.rotation = transform.rotation;
    }

}
