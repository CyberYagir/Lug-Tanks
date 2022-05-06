using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    [SerializeField] private List<GameObject> objects;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Ground") || other.transform.CompareTag("Enemy"))
        {
            objects.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (objects.Contains(other.gameObject))
        {
            objects.Remove(other.gameObject);
        }
    }

    public int GetCount() => objects.Count;
}
