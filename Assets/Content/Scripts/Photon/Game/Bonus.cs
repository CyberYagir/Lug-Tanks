using System.Collections;
using System.Collections.Generic;
using Base.Modifyers;
using Photon.Pun;
using UnityEngine;

namespace Photon.Game
{
    [System.Serializable]
    public class BonusBox: PlayerBonus
    {
        public Color color;
    }

    public enum BonusType
    {
        FireRate = 0,
        SpeedBoost = 1,
        Defence = 2,
        Repair = 3
    }
    
    public class Bonus : MonoBehaviour
    {
        [SerializeField] private int bonus_id;
        [SerializeField] private BonusType box_type;
        [SerializeField] private List<BonusBox> bonuses;

        public int BonusID => bonus_id;
        public BonusType BoxType => box_type;

        [PunRPC]
        public void SetParent(int id, int type = 0)
        {
            box_type = (BonusType)Mathf.Clamp(type, 0, bonuses.Count - 1);
            bonus_id = id;
            transform.GetChild(0).GetComponent<Renderer>().material.color = bonuses[type].color;
            transform.parent = GameManager.Instance.ActiveMap.BonusSpawns.points[id];
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
            gameObject.GetPhotonView().RPC("SpawnAnimated", RpcTarget.All, transform.position, transform.rotation, (int)BoxType);
            PhotonNetwork.Destroy(gameObject);
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<PhotonView>() != null)
            {
                if (other.GetComponentInParent<PhotonView>().IsMine)
                {
                    other.GetComponentInParent<TankBoosters>().AddBonus(bonuses[(int)BoxType]);
                    gameObject.GetPhotonView().RPC("DestroyObj", RpcTarget.MasterClient);
                }
            }
        }
    }
}