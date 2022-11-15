using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WoW
{
    public class Wheel : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private RectTransform writtenWordRect;
        [SerializeField] private TextMeshProUGUI writtenWordTMP;
        [SerializeField] private WheelLetter wheelLetterPrefab;

        private string _wordWritten = "";

        // private Word[] _words;
        private Dictionary<char, int> _charDictionary = new Dictionary<char, int>();

        private List<WheelLetter> wheelLetters = new List<WheelLetter>();

        private Camera mainCam;
        private Ray _ray;

        [SerializeField] GraphicRaycaster raycaster;
        PointerEventData pointerEventData;
        private EventSystem _eventSystem;

        [SerializeField] private LineRenderer lineRenderer;
        List<RaycastResult> results = new List<RaycastResult>();

        public static Action<string> OnWordWritten;

        void Awake()
        {
            GameController.InitializeWheel += InitWheel;
        }

        void OnDestroy()
        {
            GameController.InitializeWheel -= InitWheel;
        }

        private void Start()
        {
            mainCam = Camera.main;
            _eventSystem = EventSystem.current;

            _wordWritten = "";
            writtenWordRect.gameObject.SetActive(false);
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                lineRenderer.positionCount++;
            }
            if (Input.GetMouseButton(0))
            {
                _ray = mainCam.ScreenPointToRay(Input.mousePosition);

                pointerEventData = new PointerEventData(_eventSystem);
                pointerEventData.position = Input.mousePosition;
                raycaster.Raycast(pointerEventData, results);
                foreach (RaycastResult result in results)
                {
                    if (result.gameObject.TryGetComponent(out WheelLetter wheelLetter))
                    {
                        writtenWordRect.gameObject.SetActive(true);
                        _wordWritten += wheelLetter.Letter;
                        writtenWordTMP.SetText(_wordWritten);
                        writtenWordRect.sizeDelta = new Vector2(_wordWritten.Length * 55, writtenWordRect.sizeDelta.y);
                        wheelLetter.raycastImage.raycastTarget = false;
                        lineRenderer.enabled = true;
                        var screenToWorld = mainCam.ScreenToWorldPoint(wheelLetter.transform.position + Vector3.forward * 10);
                        lineRenderer.SetPosition(lineRenderer.positionCount - 1, screenToWorld);
                        lineRenderer.positionCount++;
                    }
                }
                results.Clear();

                lineRenderer.SetPosition(lineRenderer.positionCount - 1, mainCam.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10));
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (_wordWritten != "") OnWordWritten?.Invoke(_wordWritten);
                _wordWritten = "";
                writtenWordRect.gameObject.SetActive(false);
                for (int i = 0; i < wheelLetters.Count; i++)
                {
                    wheelLetters[i].raycastImage.raycastTarget = true;
                }
                lineRenderer.positionCount = 0;
            }
        }

        public void InitWheel(Word[] _words)
        {
            lineRenderer.positionCount = 0;
            lineRenderer.enabled = false;

            for (int i = 0; i < _words.Length; i++)
            {
                Word word = _words[i];
                for (int j = 0; j < word.Letters.Length; j++)
                {
                    if (_charDictionary.ContainsKey(word.Letters[j])) _charDictionary[word.Letters[j]]++;
                    else _charDictionary.Add(word.Letters[j], 1);
                }
            }

            float angle = 360 / _charDictionary.Count;
            Vector3 rotationVector = Vector3.up * 150;

            foreach (var item in _charDictionary)
            {
                var wheelLetter = Instantiate(wheelLetterPrefab, transform);
                wheelLetters.Add(wheelLetter);
                wheelLetter.Letter = item.Key;
                (wheelLetter.transform as RectTransform).anchoredPosition += (Vector2)rotationVector;
                rotationVector = Quaternion.AngleAxis(angle, Vector3.forward) * rotationVector;
            }

        }
    }
}
