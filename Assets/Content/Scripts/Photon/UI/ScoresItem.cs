using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Photon.UI
{
    public class ScoresItem : MonoBehaviour
    {
        [SerializeField] private RectTransform rect;
        [SerializeField] private Image rankIcon;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private TMP_Text nameT, killsT, deathsT, dkText;

        public RectTransform Rect => rect;


        public void Init(
            Color backgroundColor,
            string nickname,
            string kills,
            string deaths,
            string kd,
            Sprite rank
            )
        {
            backgroundImage.color = backgroundColor;
            nameT.text = nickname;
            killsT.text = kills;
            deathsT.text = deaths;
            dkText.text = kd;
            rankIcon.sprite = rank;
        }
    }
}
