using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviourPun, IPunObservable
{
    Tank tank;
    public GameObject canvas;
    private void Awake()
    {
        tank = GetComponent<Tank>();
        tank.name = photonView.Owner.NickName;
        if (!photonView.IsMine)
        {
            tank.transform.tag = "Enemy";
            canvas.SetActive(false);
            GetComponent<Move>().enabled = false;
            tank.cameraLook.gameObject.SetActive(false);
            foreach (var t in tank.weapons)
            {
                if (t.transform.GetComponent<WeaponAnimate>())
                {
                    t.transform.GetComponent<WeaponAnimate>().enabled = false;
                }
                t.enabled = false;
            }
            foreach (var t in tank.corpuses)
            {
                t.obj.layer = LayerMask.NameToLayer("Default");
            }
        }
        else
        {
            tank.tankOptions.weapon = WebData.playerData.weapon;
            tank.tankOptions.corpus = WebData.playerData.corpus;
            tank.tankOptions.hp = tank.corpuses[tank.tankOptions.corpus].hp;
        }
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            tank.weapons[tank.tankOptions.weapon].transform.rotation = Quaternion.Lerp(tank.weapons[tank.tankOptions.weapon].transform.rotation, tank.tankOptions.turretRotation, tank.weapons[tank.tankOptions.weapon].getRotSpeed() * 2f * Time.deltaTime);
        }
        else
        {
            Dead();
            tank.tankOptions.turretRotation = tank.weapons[tank.tankOptions.weapon].transform.rotation;
        }
    }
    [PunRPC]
    public void TakeDamage(float damage, string actorName, int weapon)
    {
        if (photonView.IsMine)
        {
            if (tank.tankOptions.hp > 0)
            {
                tank.tankOptions.hp -= damage;
                if (tank.tankOptions.hp <= 0)
                {
                    photonView.RPC("KillRPC", RpcTarget.All, actorName, photonView.Owner.NickName, weapon);
                }
            }
        }
    }
    [PunRPC]
    public void KillRPC(string playerKiller, string playerKilled, int weapon)
    {
        KillsList.killsList.Create(playerKiller, playerKilled, weapon);
    }

    public void Dead()
    {
        if (photonView.IsMine)
        {
            if (tank.tankOptions.hp <= 0)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    public static void RefreshInstance(ref Player player, Player playerPrefab, bool withMasterClient = false)
    {
        if (PhotonNetwork.IsMasterClient == false || withMasterClient == true)
        {
            print("Respawn");
            var pos = FindObjectOfType<GameManager>().spawns[Random.Range(0, FindObjectOfType<GameManager>().spawns.Length)].position;
            var rot = Quaternion.identity;
            if (player != null)
            {
                pos = player.transform.position;
                rot = player.transform.rotation;
                PhotonNetwork.Destroy(player.gameObject);
            }
            player = PhotonNetwork.Instantiate(playerPrefab.gameObject.name, pos, rot).GetComponent<Player>();

        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(JsonUtility.ToJson(tank.tankOptions, true));
        }
        else
        {
            tank.tankOptions = JsonUtility.FromJson<TankOptions>((string)stream.ReceiveNext());
        }
    }
}
