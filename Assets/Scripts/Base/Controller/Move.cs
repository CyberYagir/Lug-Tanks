using Base.Modifyers;
using Photon.Game;
using UnityEngine;

namespace Base.Controller
{
    public class Move : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float angle;
        private Player player;

        public void Init(Player player)
        {
            this.player = player;
        }
        
        private void FixedUpdate()
        {
            if (GameManager.IsOnPause) return;
        
        
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit))
            {
                angle = Mathf.Abs(Vector3.Angle(hit.normal, Vector3.down) - 180f);
            }

            var tank = player.Tank;
            var boosters = player.Boosters;
            
            bool canRot = false;
            var crp = tank.corpuses[tank.tankOptions.corpus];
            bool isFly = true;
            for (int i = 0; i < crp.tracks.Count; i++)
            {
                if (crp.tracks[i].GetCount() != 0)
                {
                    canRot = true;
                    isFly = false;
                    rb.AddForce((crp.tracks[i].transform.forward * boosters.SpeedIncrease * ((tank.corpuses[tank.tankOptions.corpus].speed + (angle * 0.5f)) / crp.tracks.Count) * Input.GetAxisRaw("Vertical")) + (new Vector3(0, 0.02f * Input.GetAxis("Vertical"))) * Time.fixedDeltaTime, ForceMode.Acceleration);
                }
            }
            rb.drag = isFly ? 0.1f : 1.5f; 

            if (canRot)
                rb.MoveRotation(Quaternion.Euler(transform.localEulerAngles + (new Vector3(0, Input.GetAxisRaw("Horizontal"), 0) * tank.corpuses[tank.tankOptions.corpus].rotSpeed) * boosters.SpeedIncrease * Time.fixedDeltaTime));
        }
    }
}
