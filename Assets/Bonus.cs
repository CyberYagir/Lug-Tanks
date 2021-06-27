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
        transform.GetChild(0).GetComponent<Renderer>().material.color = bonuses[type].color;
        transform.parent = FindObjectOfType<BonusSpawns>().points[id];
    }
    [PunRPC]
    void SpawnAnimated(Vector3 pos, Quaternion rot, int type)
    {
        var n = Instantiate(Resources.Load("BonusAnimated") as GameObject, pos, rot);
        n.transform.GetChild(0).GetComponent<Renderer>().material.color = bonuses[type].color;

        Destroy(n, 1);
    }

    [PunRPC]
    public void DestroyObj()
    {
        gameObject.GetPhotonView().RPC("SpawnAnimated", RpcTarget.All, transform.position, transform.rotation, box_type);
        PhotonNetwork.Destroy(gameObject);
	Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PhotonView>() != null)
        {
            if (other.GetComponentInParent<PhotonView>().IsMine)
            {
                other.GetComponentInParent<TankModificators>().AddBonus(bonuses[box_type]);
                gameObject.GetPhotonView().RPC("DestroyObj", RpcTarget.MasterClient);
            }
        }
    }
}
