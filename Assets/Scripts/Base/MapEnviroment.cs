using System;
using UnityEngine;

namespace Base
{
    public class MapEnviroment : MonoBehaviour
    {
        [SerializeField] private Material skybox;
        [SerializeField] private Color fogColor;
        [SerializeField] private float fogDest;
        [SerializeField] private bool fog;
        private void OnEnable()
        {
            RenderSettings.skybox = skybox;
            RenderSettings.fog = fog;
            RenderSettings.fogColor = fogColor;
            RenderSettings.fogDensity = fogDest;
        }
    }
}
