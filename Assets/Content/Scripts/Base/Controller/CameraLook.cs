using Content.Scripts.Anticheat;
using Photon.Game;
using Photon.Pun;
using UnityEngine;

namespace Base.Controller
{
    public class CameraLook : MonoBehaviour
    {
        public static CameraLook Instance;
        [SerializeField] private float sence;
        [SerializeField] private Transform maxpoint;
        [SerializeField] private Camera camera;
        private Transform parent;
        private Tank tank;


        void Start()
        {
            parent = transform.parent;
            if (parent)
            {
                tank = parent.GetComponentInParent<Tank>();
            }

            if (!tank.gameObject.GetPhotonView().IsMine)
            {
                Destroy(gameObject);
                return;
            }
        
            Instance = this;
            
            sence = PlayerPrefs.GetFloat("Sens", 1);
            transform.parent = null;
        }
    
        void Update()
        {
            if (tank == null) Destroy(gameObject);
        
            if (parent && !GameManager.IsOnPause)
            {
                transform.localEulerAngles += new Vector3(0, (Input.GetAxis("Mouse X") + Input.GetAxis("Turret")) * sence, 0);
                RaycastHit back, up;
                var weapon = tank.weapons[tank.tankOptions.Weapon];
                
                
                Physics.Raycast(weapon.minPoint.transform.position, -weapon.minPoint.transform.forward, out back, Mathf.Abs(maxpoint.localPosition.z));
                Physics.Raycast(weapon.minPoint.transform.position, weapon.minPoint.transform.up, out up, maxpoint.localPosition.y);

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
            {
                var pos = tank.corpuses[tank.tankOptions.Corpus].WeaponPoint.transform.position;
                transform.position = Vector3.Lerp(transform.position, new Vector3(pos.x, parent.transform.position.y, pos.z), 10f * Time.fixedDeltaTime);
            }
        }

        public Camera GetCamera() => camera;
    }
}
