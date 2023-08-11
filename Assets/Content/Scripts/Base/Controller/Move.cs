using Base.Modifyers;
using Content.Scripts.Anticheat;
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
            var crp = tank.corpuses[tank.tankOptions.Corpus];
            bool isFly = true;
            for (int i = 0; i < crp.Tracks.Count; i++)
            {
                if (crp.Tracks[i].GetCount() != 0)
                {
                    canRot = true;
                    isFly = false;
                    rb.AddForce((crp.Tracks[i].transform.forward * boosters.SpeedIncrease * ((crp.Speed + (angle * 0.5f)) / crp.Tracks.Count) * Input.GetAxisRaw("Vertical")) + (new Vector3(0, 0.02f * Input.GetAxis("Vertical"))) * Time.fixedDeltaTime, ForceMode.Acceleration);
                }
            }

            rb.drag = isFly ? 0.1f : 1.5f;

            if (canRot)
            {
                rb.MoveRotation(Quaternion.Euler(transform.localEulerAngles + (new Vector3(0, Input.GetAxis("Horizontal"), 0) * crp.RotSpeed) * boosters.SpeedIncrease * Time.fixedDeltaTime));
            }
        }
    }
}
