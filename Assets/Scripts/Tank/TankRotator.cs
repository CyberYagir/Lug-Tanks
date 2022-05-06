using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankRotator : MonoBehaviour
{
    [SerializeField] bool down;
    [SerializeField] float speed;

    private Tank tank;
    private Camera camera;
    private void Start()
    {
        tank = GetComponent<Tank>();
        camera = Camera.main;
    }

    private void Update()
    {
        foreach (var t in tank.weapons)
        {
            if (t.transform.GetComponent<WeaponAnimate>())
            {
                t.transform.GetComponent<WeaponAnimate>().enabled = false;
            }
            t.enabled = false;
        }

        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Player"))
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
