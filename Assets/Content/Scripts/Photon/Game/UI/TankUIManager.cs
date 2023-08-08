using System;
using System.Collections.Generic;
using UnityEngine;

namespace Photon.Game.UI
{
    public abstract class TankUIElement : MonoBehaviour
    {
        private Player player;

        public Player Player => player;

        public virtual void Init(Player player)
        {
            this.player = player;
        }

        public virtual void UpdateElement()
        {
            
        }
    }
    
    public class TankUIManager : MonoBehaviour
    {
        private Player player;
        [SerializeField] private List<TankUIElement> elements;


        public void Init(Player player)
        {
            this.player = player;
            foreach (var el in elements)
            {
                el.Init(player);
            }
        }

        private void Update()
        {
            foreach (var el in elements)
            {
                el.UpdateElement();
            }
        }
    }
}
