using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class DeadTank : MonoBehaviour
{
    [SerializeField] private GameObject[] weapons, corpuses;
    [SerializeField] private Quaternion rot;
    [SerializeField] private GameObject particles;

    public int weapon { get; private set; }
    public int corpus { get; private set; }
    
    
    public Quaternion GetRot() => rot; 
    
    public void StartDestroy()
    {
        StartCoroutine(wait());
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(5);
        PhotonNetwork.Destroy(gameObject);
    }
    [PunRPC]
    public void Set(int w, int c, Quaternion r, Vector3 vel, float mass, float drag, Vector3 force, Vector3 torque, bool spawnp = true)
    {
        weapon = w;
        corpus = c;
        rot = r;
        if (spawnp)
        {
            particles.transform.parent = null;
            Destroy(particles, 2);
        }
        else
        {
            Destroy(particles);
        }
        weapons[w].SetActive(true);
        weapons[w].transform.position = corpuses[c].transform.GetChild(0).transform.position;
        weapons[w].transform.rotation = r;
        corpuses[c].SetActive(true);
        GetComponent<Rigidbody>().mass = mass;
        GetComponent<Rigidbody>().drag = drag;
        GetComponent<Rigidbody>().velocity = vel;
        GetComponent<Rigidbody>().AddRelativeForce(force, ForceMode.Impulse);
        GetComponent<Rigidbody>().AddRelativeTorque(torque);
    }
}
