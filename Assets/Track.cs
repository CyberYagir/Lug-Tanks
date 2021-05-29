using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    public List<GameObject> objects;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Ground")
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
}
