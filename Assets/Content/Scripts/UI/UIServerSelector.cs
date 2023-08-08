using System;
using System.Collections;
using System.Collections.Generic;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIServerSelector : MonoBehaviour
    {
        [SerializeField] private Transform item;
        
        [SerializeField] private List<string> servers = new List<string>() {"ru", "eu", "asia", "jp", "kr", "us", "usw", "ussc", "sa", "tr"};

        private List<GameObject> items = new List<GameObject>(10);

        private void Start()
        {
            PhotonLobbyService.Instance.OnConnectToMaster.AddListener(SpawnItems);
        }

        private void SpawnItems()
        {
            foreach (var it in items)
            {
                Destroy(it.gameObject);
            }

            foreach (var variableServer in servers)
            {
                var spawned = Instantiate(item, item.parent);
                var text = spawned.GetComponentInChildren<TMP_Text>();
                var button = spawned.GetComponent<Button>();


                spawned.gameObject.SetActive(true);
                text.text = variableServer;
                
                if (variableServer == PhotonLobbyService.Instance.AutoRegion)
                {
                    text.color = Color.green;
                }
                else
                {
                    text.color = Color.white * 0.76f;
                }
                
                button.onClick.AddListener(delegate
                {
                    var region = variableServer;
                    StartCoroutine(Wait(region));
                });

                items.Add(spawned.gameObject);
            }

            item.gameObject.SetActive(false);
        }

        IEnumerator Wait(string rg)
        {
            if (rg == PhotonNetwork.NetworkingClient.CloudRegion) yield break;
            
            
            PhotonNetwork.Disconnect();

            while (PhotonNetwork.IsConnected)
            {
                yield return null;
            }


            bool haveError = false;
            
            try
            {
                PhotonNetwork.ConnectToRegion(rg);
            }
            catch (Exception e)
            {
                haveError = true;
            }

            if (haveError)
            {
                PhotonNetwork.ConnectToRegion(PhotonLobbyService.Instance.AutoRegion);
            }
        }
    }
}
