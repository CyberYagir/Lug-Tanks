using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon2 : Weapon
{
    public GameObject line;


    private void Start()
    {
        shootAction += () =>
        {
            var targets = Enemies(shootPoint);
            if (targets.Count == 0)
            {
                RaycastHit hit;
                if (Physics.Raycast(shootPoint.transform.position, shootPoint.forward, out hit))
                {
                    if (hit.transform.CompareTag("Enemy") && TeamCheck(hit.transform.gameObject))
                        hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, (float)damage, (string)PhotonNetwork.NickName, GetComponentInParent<Tank.Controller.Tank>().tankOptions.weapon);
                    CreateLine(shootPoint.transform.position, hit.point);
                }
                else
                {
                    CreateLine(shootPoint.transform.position, shootPoint.forward * 1000f);
                }
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(shootPoint.transform.position, targets[0].enemy.gameObject.transform.position - shootPoint.transform.position, out hit))
                {
                    targets[0].enemy.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, (float)damage, (string)PhotonNetwork.NickName, GetComponentInParent<Tank.Controller.Tank>().tankOptions.weapon);
                    Tank.Controller.Tank.SetLastPlayer(targets[0].enemy.gameObject);
                    CreateLine(shootPoint.transform.position, targets[0].enemy.transform.position);
                }
            }
        };
    }

    public void CreateLine(Vector3 pos1, Vector3 pos2)
    {
        var l = PhotonNetwork.Instantiate(line.gameObject.name, Vector2.zero, Quaternion.identity).GetPhotonView();
        l.TransferOwnership(PhotonNetwork.CurrentRoom.MasterClientId);
        l.RPC("SetLinePoses", RpcTarget.All, pos1, pos2);
        l.GetComponent<LineScript>().Start_Destroy();
    }
}
