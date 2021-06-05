using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonuses : MonoBehaviour
{
    public GameObject prefab;
    public float time;

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            time += Time.deltaTime;
            if (time > 5f)
            {
                var spawns = FindObjectOfType<BonusSpawns>();

                GameObject spawn = null;
                int id = -1;
                for (int i = 0; i < 5; i++)
                {
                    id = Random.Range(0, spawns.points.Length);
                    if (spawns.points[id].childCount == 0)
                    {
                        spawn = spawns.points[id].gameObject;
                        break;
                    }
                }
                if (spawn != null)
                {
                    var n = PhotonNetwork.Instantiate(prefab.name, spawn.transform.position, Quaternion.identity);
                    n.GetPhotonView().RPC("SetParent", RpcTarget.AllBuffered, id);
                }
                time = 0;
            }
        }    
    }
}
