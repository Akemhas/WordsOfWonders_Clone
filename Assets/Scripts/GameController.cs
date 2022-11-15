using System;
using UnityEngine;

namespace WoW
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private TextAsset gameData;
        [SerializeField] private Grid grid;

        public Word[] words;
        public Level currentLevel;
        private LevelContainer levelContainer = new LevelContainer();

        public static Action<Word[]> InitializeWheel;

        private void OnEnable()
        {
            Wheel.OnWordWritten += CheckWrittenWord;
        }

        private void OnDisable()
        {
            Wheel.OnWordWritten -= CheckWrittenWord;
        }

        public int CurrentLevelIndex
        {
            get => PlayerPrefs.GetInt("CurrentLevelIndex", 0);
            set => PlayerPrefs.SetInt("CurrentLevelIndex", value);
        }

        void Awake()
        {
            levelContainer = JsonUtility.FromJson<LevelContainer>(gameData.text);
            currentLevel = levelContainer.Levels[CurrentLevelIndex];

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

        void Start()
        {
            InitializeWheel?.Invoke(words);
        }

        public void CheckWrittenWord(string writtenWord)
        {
            for (int i = 0; i < words.Length; i++)
            {
                Word word = words[i];
                if (writtenWord == word.Letters)
                {
                    for (int j = 0; j < word.cells.Length; j++)
                    {
                        word.cells[j].Open();
                    }
                }
            }
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
    }
}
