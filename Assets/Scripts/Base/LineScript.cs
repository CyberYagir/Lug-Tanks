using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class LineScript : MonoBehaviour
{
    public string prefabName;

    private LineRenderer line;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    public void Start_Destroy()
    {
        StartCoroutine(DestroyWait());
    }

    private void Update()
    {
        if (line.material.color.a > 0)
        {
            line.material.color -= new Color(0, 0, 0, Time.deltaTime / 4f);
        }
        else
        {
            line.material.color = new Color(0, 0, 0, 0);
        }
    }

    [PunRPC]
    public void SetLinePoses(Vector3 pos1, Vector3 pos2)
    {
        line.SetPosition(0, pos1);
        line.SetPosition(1, pos2);
    }

    

    IEnumerator DestroyWait()
    {
        yield return new WaitForSeconds(5f);
        PhotonNetwork.Destroy(gameObject);
    }
}
