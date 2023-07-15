using System;
using Base.Controller;
using Photon.Game;
using UnityEngine;

namespace Menu
{
    public class TankRotator : MonoBehaviour
    {
        [SerializeField] float speed;
        [SerializeField] private Camera camera;
        [SerializeField] private Tank tank;
        
        private bool down;

        private void Awake()
        {
            foreach (var t in tank.weapons)
            {
                var animations = t.transform.GetComponent<WeaponAnimate>();
                if (animations)
                {
                    animations.enabled = false;
                }
                t.enabled = false;
            }
        }

        private void Update()
        {

            if (Input.GetKey(KeyCode.Mouse0))
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var hit))
                {
                    if (hit.transform.GetComponent<Player>())
                    {
                        down = true;
                    }
                }
            }
            else
            {
                down = false;
            }
            
            if (down)
            {
                transform.Rotate(Vector3.up * speed * -Input.GetAxis("Mouse X") * Time.deltaTime);
            }
        }
    }
}
