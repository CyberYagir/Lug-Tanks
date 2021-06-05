using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    public int bonus_id;
    [PunRPC]
    public void SetParent(int id)
    {
        bonus_id = id;
        transform.parent = FindObjectOfType<BonusSpawns>().points[id];
    }
}
