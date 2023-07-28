using Base.Controller;
using Photon.Game;
using UnityEngine;

namespace Base.Weapons
{
    public class WeaponRotate : MonoBehaviour
    {
        public static Camera CameraInstance;
        
        public Transform shootCamera;
        public float rotateSpeed;

        private Tank tank;
        private Player player;
        
        public static bool IsVisible(GameObject gm)
        {
            try
            {
                return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(CameraInstance), gm.GetComponentInChildren<Collider>().bounds);

            }
            catch (System.Exception)
            {
                return false;
            }
        }
        private void Start()
        {
            player = GetComponentInParent<Player>();
            tank = player.Tank;
            
            if (player.photonView != null && player.photonView.IsMine)
            {
                CameraInstance = shootCamera.GetComponent<Camera>();
            }
        }
        private void Update()
        {
            transform.position = tank.corpuses[tank.tankOptions.corpus].weaponPoint.transform.position;
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
            shootCamera.transform.position = tank.weapons[tank.tankOptions.weapon].shootPoint.position;

            if (player.CameraLook != null)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, player.CameraLook.transform.rotation, rotateSpeed * Time.deltaTime);
            }

        }

    }
}
