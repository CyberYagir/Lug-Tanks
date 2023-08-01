using System;
using Base.Controller;
using Photon.Game;
using UnityEngine;
using Web;

namespace Menu
{
    public class TankRotator : MonoBehaviour
    {
        [SerializeField] float speed;
        [SerializeField] private Camera camera;
        [SerializeField] private Tank tank;


        private float velocity;
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
            if (WebDataService.data == null) return;
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

                if (Mathf.Abs(Input.GetAxis("Mouse X")) > 0.5f)
                {
                    velocity = speed * -Input.GetAxis("Mouse X");
                }
                else
                {
                    velocity = 0;
                }
            }
            else
            {
                down = false;

                velocity = Mathf.Lerp(velocity, 0, Time.deltaTime * 5f);
            }

            if (down)
            {
                transform.Rotate(Vector3.up * speed * -Input.GetAxis("Mouse X") * Time.deltaTime);
            }
            else
            {
                
                transform.Rotate(Vector3.up * velocity * Time.deltaTime);
            }
        }
    }
}
