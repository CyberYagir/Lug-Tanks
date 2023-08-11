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
        private float lastAngle;
        
        
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
        private void FixedUpdate()
        {
            var crp = tank.corpuses[tank.tankOptions.Corpus];
            var angle = crp.RotatorData.CorpusRotator.GetTagetAngle();

            lastAngle = Mathf.Lerp(lastAngle, angle, 10 * Time.fixedDeltaTime);
            
            transform.position = crp.WeaponPoint.transform.position;
            shootCamera.transform.position = tank.weapons[tank.tankOptions.Weapon].shootPoint.position;

            if (player.CameraLook != null)
            {
                transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0) + transform.InverseTransformDirection(tank.transform.right * lastAngle);
                transform.rotation = Quaternion.Lerp(transform.rotation, player.CameraLook.transform.rotation, rotateSpeed * Time.fixedDeltaTime);
            }

            shootCamera.localEulerAngles = new Vector3(lastAngle * 2f, 0, 0);
        }
    }
}
