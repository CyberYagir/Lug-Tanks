using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed;
    public float rotSpeed;
    public float maxSlope;
    public Rigidbody rb;
    public Tank tank;

    private void FixedUpdate()
    {
        bool canRot = false;
        var crp = tank.corpuses[tank.tankOptions.corpus];
        bool isFly = true;
        for (int i = 0; i < crp.tracks.Count; i++)
        {
            if (crp.tracks[i].objects.Count != 0)
            {
                canRot = true;
                isFly = false;
                rb.AddForce((crp.tracks[i].transform.forward * (tank.corpuses[tank.tankOptions.corpus].speed / crp.tracks.Count) * Input.GetAxisRaw("Vertical")) + (new Vector3(0, 0.02f * Input.GetAxis("Vertical"))) * Time.fixedDeltaTime, ForceMode.Acceleration);
            }
        }
        rb.drag = isFly ? 0.1f : 1.5f; 

        if (canRot)
            rb.MoveRotation(Quaternion.Euler(transform.localEulerAngles + (new Vector3(0, Input.GetAxisRaw("Horizontal"), 0) * tank.corpuses[tank.tankOptions.corpus].rotSpeed) * Time.fixedDeltaTime));
    }
}
