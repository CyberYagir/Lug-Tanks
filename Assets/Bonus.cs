using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BonusBox: PlayerBonus
{
    public Color color;
}

public class Bonus : MonoBehaviour
{
    public int bonus_id, box_type;
    public List<BonusBox> bonuses;
    [PunRPC]
    public void SetParent(int id, int type = 0)
    {
        box_type = type;
        bonus_id = id;
        transform.parent = FindObjectOfType<BonusSpawns>().points[id];
    }
    [PunRPC]
    void SpawnAnimated(Vector3 pos, Quaternion rot)
    {
        Destroy(Instantiate(Resources.Load("BonusAnimated") as GameObject, pos, rot), 1);
    }

    [PunRPC]
    public void DestroyObj()
    {
        gameObject.GetPhotonView().RPC("SpawnAnimated", RpcTarget.All, transform.position, transform.rotation);
        PhotonNetwork.Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Player>())
        {
            gameObject.GetPhotonView().RPC("DestroyObj", RpcTarget.MasterClient);
        }
    }
}
