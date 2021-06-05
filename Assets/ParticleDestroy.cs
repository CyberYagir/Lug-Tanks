using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    public void StartEnum()
    {
        StartCoroutine(wait());
    }


    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.7f);
        PhotonNetwork.Destroy(gameObject);
    }
}
