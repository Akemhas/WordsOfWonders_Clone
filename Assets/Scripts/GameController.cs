using System;
using UnityEngine;

namespace WoW
{
    public class GameController : MonoBehaviour
    {
        #region Singleton

        private static object _lock = new object();
        private static GameController _instance;

        public static GameController Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null) _instance = (GameController)FindObjectOfType(typeof(GameController));

                    return _instance;
                }
            }
        }

        #endregion

        [SerializeField] private TextAsset gameData;
        [SerializeField] private Grid grid;

        public Word[] words;
        public Level currentLevel;

        private LevelContainer levelContainer = new LevelContainer();


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
    }
}
