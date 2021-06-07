using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankRotator : MonoBehaviour
{
    public bool down;
    public float speed;
    private void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Player")
            {
                if (Input.GetKey(KeyCode.Mouse0))
                    down = true;
            }
            
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            down = false;
        }
        if (down)
        {
            transform.Rotate(Vector3.up * speed * -Input.GetAxis("Mouse X") * Time.deltaTime);
        }
    }
}
