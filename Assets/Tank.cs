using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TankOptions {
    public float hp;
    public int corpus;
    public int weapon;
    public Quaternion turretRotation;
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
    public int[] bonuses;
    public Rigidbody rb;
    public Transform damageDisplayPoint;
    public static GameObject lastPlayer;
    public static float lastPlayerClearTime;
    private void Start()
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Ignore Raycast"), LayerMask.NameToLayer("NoCollisions"));
        if (gameObject.GetPhotonView() != null && !gameObject.GetPhotonView().IsMine)
        {
            SetEqup();
        }
    }
    public static void SetLastPlayer(GameObject obj)
    {
        lastPlayer = obj;
        lastPlayerClearTime = 0;
    }
    

   void SetEqup()
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
    private void Update()
    {

        if (gameObject.GetPhotonView() == null || gameObject.GetPhotonView().IsMine)
        {
            lastPlayerClearTime += Time.deltaTime;
            SetEqup();
        }
        else
        {
            weapons[tankOptions.weapon].transform.position = corpuses[tankOptions.corpus].weaponPoint.transform.position;
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
