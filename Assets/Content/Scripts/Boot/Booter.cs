using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Boot
{
    public class Booter : MonoBehaviour
    {
        private void Awake()
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
