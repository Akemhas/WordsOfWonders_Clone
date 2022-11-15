using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WoW
{
    public class Cell : MonoBehaviour, IClickable
    {
        public Vector2 size;
        public Vector2Int cellPos;

        public char letter;

        [SerializeField] private TextMeshProUGUI letterTMP;

        public void Click()
        {
        }
    }
}