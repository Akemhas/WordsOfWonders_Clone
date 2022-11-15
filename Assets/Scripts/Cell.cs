using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        [SerializeField] private TextMeshProUGUI letterTMP;

        public void Click()
        {
        }
    }
}