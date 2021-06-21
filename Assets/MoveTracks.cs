using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTracks : MonoBehaviour
{
    public int idLeft, idRight;
    public Rigidbody rigidbody;
    public MeshRenderer meshRenderer;
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
           rigidbody = GetComponentInParent<Rigidbody>();
    }
    private void Update()
    {
        var local = transform.TransformDirection(rigidbody.velocity);
        meshRenderer.materials[idLeft].mainTextureOffset += new Vector2(rigidbody.velocity.z + Input.GetAxis("Horizontal"), 0);
        meshRenderer.materials[idRight].mainTextureOffset += new Vector2(rigidbody.velocity.z - Input.GetAxis("Horizontal"), 0);
    }
}
