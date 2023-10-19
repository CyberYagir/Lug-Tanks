using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTracks : MonoBehaviour
{
    [SerializeField] private int idLeft, idRight;
    private Rigidbody rigidbody;
    private MeshRenderer meshRenderer;
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        rigidbody = GetComponentInParent<Rigidbody>();
    }
    private void Update()
    {
        meshRenderer.materials[idLeft].mainTextureOffset += new Vector2(0, rigidbody.velocity.z + Input.GetAxis("Horizontal"));
        meshRenderer.materials[idRight].mainTextureOffset += new Vector2(0, rigidbody.velocity.z - Input.GetAxis("Horizontal"));
    }
}
