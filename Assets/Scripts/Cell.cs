using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace WoW
{
    public class Cell : MonoBehaviour, IClickable
    {
        public RectTransform rectTransform;
        public Vector2Int cellPos;

        public GameObject modelHolder;

        private char _letter;
        public char Letter
        {
            get => _letter;
            set
            {
                _letter = value;
                letterTMP.SetText(_letter.ToString());
            }
        }

        public void Open()
        {
            letterTMP.gameObject.SetActive(true);
            transform.DOScale(1.075f, .225f).SetEase(Ease.InOutSine).SetLoops(2, LoopType.Yoyo).SetLink(gameObject);
        }

        [SerializeField] private TextMeshProUGUI letterTMP;

        public void Click()
        {
        }
    }
}