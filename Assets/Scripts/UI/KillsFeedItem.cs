using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class KillsFeedItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text player1T, player2T;
        [SerializeField] private Image image;

        public void Init(string player1, string player2, Sprite weapon)
        {
            player1T.text = player1;
            player2T.text = player2;
            image.sprite = weapon;


            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }
    }
}
