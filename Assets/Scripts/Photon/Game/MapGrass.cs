using System;
using UnityEngine;

namespace Photon.Game
{
    public class MapGrass : MonoBehaviour
    {
        private void Awake()
        {
            ChangeState();
        }

        public void ChangeState()
        {
            gameObject.SetActive(PlayerPrefs.GetInt("Grass", 1) == 1);
        }
    }
}
