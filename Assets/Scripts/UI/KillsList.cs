using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class KillsList : MonoBehaviour
    {
        public static KillsList Instance;
        [SerializeField] private GameObject holder;
        [SerializeField] private GameObject item;
        [SerializeField] private Sprite[] icons;

        private void Awake()
        {
            Instance = this;
        }

        public void Create(string playerKiller, string playerKilled, int weapon)
        {
            var it = Instantiate(item.gameObject, holder.transform).GetComponent<KillsFeedItem>();
            it.Init(playerKiller, playerKilled, icons[weapon]);
            it.gameObject.SetActive(true);
            Destroy(it.gameObject, 3);
        }
    }
}
