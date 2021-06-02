using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Keys
{
    public string name;
    public object data;
}
[System.Serializable]
public class TankOptions {
    public float hp;
    public int k, d;
    public int corpus;
    public int weapon;
    public Quaternion turretRotation;
    public List<Keys> keys = new List<Keys>();
}
[System.Serializable]
public class Corpus
{
    public float hp;
    public float speed;
    public float rotSpeed;
    public GameObject obj;
    public Transform weaponPoint;
    public Vector3 centerOfMass;
    public Transform[] hitPoints;
    public List<Track> tracks;
}
public class Tank : MonoBehaviour
{
    public CameraLook cameraLook;
    public TankOptions tankOptions;
    public List<Corpus> corpuses;
    public List<Weapon> weapons;
    public Rigidbody rb;

    private void Start()
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Ignore Raycast"), LayerMask.NameToLayer("NoCollisions"));
    }

    private void Update()
    {
        for (int i = 0; i < corpuses.Count; i++)
        {
            corpuses[i].obj.SetActive(i == tankOptions.corpus);
            if (i == tankOptions.corpus)
            {
                rb.centerOfMass = corpuses[i].centerOfMass;
            }
        }
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].gameObject.SetActive(i == tankOptions.weapon);
            if (i == tankOptions.weapon)
            {
                weapons[i].transform.position = corpuses[tankOptions.corpus].weaponPoint.transform.position;
            }
        }
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < corpuses.Count; i++)
        {
            if (corpuses[i].obj.active)
            {
                if (corpuses[i].weaponPoint != null)
                {
                    Gizmos.DrawWireSphere(corpuses[i].weaponPoint.position, .2f);
                }
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.TransformPoint(corpuses[i].centerOfMass), .2f);
            }
        }
    }
}
