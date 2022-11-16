using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace WoW
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private TextAsset gameData;
        [SerializeField] private Grid grid;
        [SerializeField] private Button hintButton;
        [SerializeField] private Word[] words;
        [SerializeField] private Level currentLevel;
        [SerializeField] private TextMeshProUGUI letterTMPPrefab;

        public static Action<Word[]> InitializeWheel;

        private LevelContainer levelContainer = new LevelContainer();

        public int CurrentLevelIndex
        {
            get => PlayerPrefs.GetInt("CurrentLevelIndex", 0);
            set => PlayerPrefs.SetInt("CurrentLevelIndex", value);
        }

        private int _foundWordCount;

        void Awake()
        {
            levelContainer = JsonUtility.FromJson<LevelContainer>(gameData.text);
            currentLevel = levelContainer.Levels[CurrentLevelIndex % levelContainer.Levels.Length];

            string[] wordGroups = currentLevel.Words.Split('|');

            words = new Word[wordGroups.Length];
            int length = wordGroups.Length;
            for (int i = 0; i < length; i++)
            {
                string[] wordString = wordGroups[i].Split(',');
                words[i] = new Word();
                Word word = words[i];

                word.SRow = Int32.Parse(wordString[0]);
                word.SColumn = Int32.Parse(wordString[1]);
                word.Letters = wordString[2];
                word.H = wordString[3][0] == 'H';
            }

            grid.InitializeGrid(new Vector2Int(currentLevel.Column, currentLevel.Row), words);
        }

        private void OnEnable()
        {
            Wheel.OnWordWritten += CheckWrittenWord;
            hintButton.onClick.AddListener(Hint);
        }

        private void OnDisable()
        {
            Wheel.OnWordWritten -= CheckWrittenWord;
            hintButton.onClick.RemoveListener(Hint);
        }


        void Start()
        {
            InitializeWheel?.Invoke(words);
        }

        public void CheckWrittenWord(string writtenWord, RectTransform writtenWordRect)
        {
            for (int i = 0; i < words.Length; i++)
            {
                Word word = words[i];
                if (writtenWord == word.Letters)
                {
                    if (word.found) return;
                    word.found = true;
                    _foundWordCount++;
                    for (int j = 0; j < word.cells.Length; j++)
                    {
                        Cell cell = word.cells[j];
                        if (cell.full) continue;
                        var letterTMP = Instantiate(letterTMPPrefab, writtenWordRect);
                        cell.full = true;
                        letterTMP.SetText(cell.letter.ToString());
                        letterTMP.transform.SetParent(cell.transform);
                        letterTMP.transform.DOLocalMove(Vector3.zero, .5f).OnComplete(() =>
                        {
                            cell.Open();
                            if (_foundWordCount == words.Length) LoadNextLevel();
                        }).SetLink(letterTMP.gameObject);
                    }
                }
            }
        }

        private void Hint()
        {
            int k = 0;
            int randomIndex = Random.Range(0, words.Length);
            Word selectedWord = null;
            for (int i = randomIndex; k < words.Length; i = ((i + 1) % words.Length))
            {
                if (!words[i].found)
                {
                    selectedWord = words[i];
                    break;
                }
                k++;
            }

            if (selectedWord == null) return;

            Cell[] cells = selectedWord.cells;

            randomIndex = Random.Range(0, cells.Length);
            k = 0;
            Cell selectedCell = null;
            for (int i = randomIndex; k < cells.Length; i = ((i + 1) % cells.Length))
            {
                if (!cells[i].full)
                {
                    selectedCell = cells[i];
                    break;
                }
                k++;
            }

            if (selectedCell == null) return;

            var letterTMP = Instantiate(letterTMPPrefab, selectedCell.transform);
            selectedCell.Open();
            selectedCell.full = true;
            letterTMP.SetText(selectedCell.letter.ToString());

            CheckAllWords();
        }

        private void CheckAllWords()
        {
            int length = words.Length;

            for (int i = 0; i < length; i++)
            {
                Word word = words[i];
                if (word.found) continue;

                bool wordRevealed = true;
                for (int j = 0; j < word.cells.Length; j++) wordRevealed &= word.cells[j].full;

                if (wordRevealed)
                {
                    word.found = true;
                    _foundWordCount++;
                }
            }

            if (_foundWordCount == words.Length) LoadNextLevel();
        }

        private void LoadNextLevel()
        {
            CurrentLevelIndex++;
            SceneManager.LoadScene(0);
        }
    }

    [Serializable]
    public class LevelContainer
    {
        public Level[] Levels;
    }

    [Serializable]
    public class Level
    {
        public int Row;
        public int Column;
        public string Words;
    }

    [Serializable]
    public class Word
    {
        public int SRow;
        public int SColumn;
        public string Letters;
        public bool H;
        public Cell[] cells;
        public bool found;
    }
}
