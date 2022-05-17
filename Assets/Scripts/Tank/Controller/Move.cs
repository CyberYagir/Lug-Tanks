using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Tank tank;
    [SerializeField] private float angle;
    private void FixedUpdate()
    {

        if (GameManager.pause) return;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            angle = Mathf.Abs(Vector3.Angle(hit.normal, Vector3.down) - 180f);
        }

        bool canRot = false;
        var crp = tank.corpuses[tank.tankOptions.corpus];
        bool isFly = true;
        for (int i = 0; i < crp.tracks.Count; i++)
        {
            if (crp.tracks[i].GetCount() != 0)
            {
                canRot = true;
                isFly = false;
                rb.AddForce((crp.tracks[i].transform.forward * TankModificators.speedIncrease * ((tank.corpuses[tank.tankOptions.corpus].speed + (angle * 0.5f)) / crp.tracks.Count) * Input.GetAxisRaw("Vertical")) + (new Vector3(0, 0.02f * Input.GetAxis("Vertical"))) * Time.fixedDeltaTime, ForceMode.Acceleration);
            }
        }
        rb.drag = isFly ? 0.1f : 1.5f; 

        if (canRot)
            rb.MoveRotation(Quaternion.Euler(transform.localEulerAngles + (new Vector3(0, Input.GetAxisRaw("Horizontal"), 0) * tank.corpuses[tank.tankOptions.corpus].rotSpeed) * TankModificators.speedIncrease * Time.fixedDeltaTime));
    }
}
