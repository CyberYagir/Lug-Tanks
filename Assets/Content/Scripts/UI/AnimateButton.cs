using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class AnimateButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Image image;
        private Color startColor;

        private void Awake()
        {
            image = GetComponent<Image>();
            startColor = image.color;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            StopAllCoroutines();
            image.DOColor(Color.white * 0.82f, 0.2f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            StopAllCoroutines();
            image.DOColor(startColor, 0.2f);
        }
    }
}
