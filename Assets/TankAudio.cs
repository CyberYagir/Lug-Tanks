using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAudio : MonoBehaviour
{
    public Rigidbody rigidbody;
    public AudioSource audioSource;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (rigidbody != null)
        {
            audioSource.volume = Mathf.Clamp(new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z).magnitude/11f,0,1) * 0.05f;
        }           
    }
}
