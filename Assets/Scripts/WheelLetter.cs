using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WoW
{
    public class WheelLetter : MonoBehaviour
    {
        public Image raycastImage;
        [SerializeField] private TextMeshProUGUI letterTMP;

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

    }
}
