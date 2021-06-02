using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRotate : MonoBehaviour
{
    public Tank tank;
    public Transform shootCamera;
    public static Camera shootCam;
    public float rotateSpeed;

    public static bool IsVisible(GameObject gm)
    {
        return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(shootCam), gm.GetComponentInChildren<Collider>().bounds);
    }
    private void Start()
    {
        if (GetComponentInParent<Player>().photonView.IsMine)
            shootCam = shootCamera.GetComponent<Camera>();
    }
    private void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, tank.cameraLook.transform.rotation, rotateSpeed * Time.deltaTime);
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }
    private void FixedUpdate()
    {
        shootCamera.transform.rotation = transform.rotation;
    }

}
