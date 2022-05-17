using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTracks : MonoBehaviour
{
    [SerializeField] private int idLeft, idRight;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private MeshRenderer meshRenderer;
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        rigidbody = GetComponentInParent<Rigidbody>();
    }
    private void Update()
    {
        meshRenderer.materials[idLeft].mainTextureOffset += new Vector2(rigidbody.velocity.z + Input.GetAxis("Horizontal"), 0);
        meshRenderer.materials[idRight].mainTextureOffset += new Vector2(rigidbody.velocity.z - Input.GetAxis("Horizontal"), 0);
    }
}
