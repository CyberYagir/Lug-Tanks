using System;
using System.Collections;
using DG.Tweening;
using Scriptable;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MapsPreview : MonoBehaviour
    {
        [SerializeField] private GameDataObject gameData;
        [SerializeField] private Image preview, prev, next;

        private int selectedMap = 0;
        private bool isAnimated = false;

        public int SelectedMap => selectedMap;

        private void Awake()
        {
            UpdateSprites();
        }


        public void UpdateSprites()
        {
            preview.sprite = gameData.MapsData.GetMapSprite(SelectedMap);
            prev.sprite = gameData.MapsData.GetMapSprite(GetPrevMap());
            next.sprite = gameData.MapsData.GetMapSprite(GetNextMap());
        }


        public void NextMap()
        {
            if (isAnimated) return;
            selectedMap = GetNextMap();

            StartCoroutine(MoveRects(-1));
        }
        
        public void PrevMap()
        {
            if (isAnimated) return;
            selectedMap = GetPrevMap();
            StartCoroutine(MoveRects(1));
        }

        IEnumerator MoveRects(float dir = 1f)
        {
            isAnimated = true;

            var previewX = preview.rectTransform.anchoredPosition.x;
            var prevX = prev.rectTransform.anchoredPosition.x;
            var nextX = next.rectTransform.anchoredPosition.x;
            
            CreateTween(preview.rectTransform, dir);
            CreateTween(prev.rectTransform, dir);
            CreateTween(next.rectTransform, dir);

            yield return new WaitForSeconds(0.21f);

            preview.rectTransform.anchoredPosition = new Vector2(previewX, preview.rectTransform.anchoredPosition.y);
            prev.rectTransform.anchoredPosition = new Vector2(prevX, prev.rectTransform.anchoredPosition.y);
            next.rectTransform.anchoredPosition = new Vector2(nextX, next.rectTransform.anchoredPosition.y);
            
            UpdateSprites();

            isAnimated = false;
        }


        public void CreateTween(RectTransform rect,  float dir)
        {
            CreateTweenX(rect, rect.anchoredPosition.x + rect.sizeDelta.x * dir);
        }

        public void CreateTweenX(RectTransform rect, float x, float time = 0.2f)
        {
            rect.DOAnchorPosX(x, 0.2f);
        }
        

        public int GetNextMap()
        {
            if (SelectedMap + 1 >= gameData.MapsData.Count)
            {
                return 0;
            }
            else
            {
                return SelectedMap + 1;
            }
        }
        
        public int GetPrevMap()
        {
            if (SelectedMap - 1 < 0)
            {
                return gameData.MapsData.Count - 1;
            }
            else
            {
                return SelectedMap - 1;
            }
        }
    }
}
