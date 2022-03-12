using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class LineScript : MonoBehaviour
{
    public string prefabName;
    public void Start_Destroy()
    {
        StartCoroutine(destroy());
    }

    private void Update()
    {
        if (GetComponent<LineRenderer>().material.color.a > 0)
        {
            GetComponent<LineRenderer>().material.color -= new Color(0, 0, 0, Time.deltaTime / 4f);
        }
        else
        {
            GetComponent<LineRenderer>().material.color = new Color(0, 0, 0, 0);
        }
    }

    [PunRPC]
    public void SetLinePoses(Vector3 pos1, Vector3 pos2)
    {
        GetComponent<LineRenderer>().SetPosition(0, pos1);
        GetComponent<LineRenderer>().SetPosition(1, pos2);
    }

    

    IEnumerator destroy()
    {
        yield return new WaitForSeconds(5f);
        PhotonNetwork.Destroy(gameObject);
    }
}
