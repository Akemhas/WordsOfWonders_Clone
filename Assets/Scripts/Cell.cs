using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace WoW
{
    public class Cell : MonoBehaviour, IClickable
    {
        public RectTransform rectTransform;
        [SerializeField] private Image cellBG;
        [SerializeField] private Color fullColor = Color.red;

        public Vector2Int cellPos;

        public GameObject modelHolder;
        public char letter;

        public bool full;


        public void Open()
        {
            transform.DOScale(1.075f, .225f).SetEase(Ease.InOutSine).SetLoops(2, LoopType.Yoyo).SetLink(gameObject);
            cellBG.color = fullColor;
        }

        public void Click()
        {
        }
    }
}