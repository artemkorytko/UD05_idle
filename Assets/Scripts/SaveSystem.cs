using UnityEngine;

namespace DefaultNamespace
{
    public class SaveSystem : MonoBehaviour
    {
        private const string SAVE_KEY = "GameData";

        private GameData _gameData;

        public GameData Data => _gameData;

        public void Initialize()
        {
            if (PlayerPrefs.HasKey(SAVE_KEY))
            {
                LoadData();
            }
            else
            {
                _gameData = new GameData();
            }
        }

        private void LoadData()
        {
            string jsonData = PlayerPrefs.GetString(SAVE_KEY);
            _gameData = JsonUtility.FromJson<GameData>(jsonData);
        }

        public void SaveData()
        {
            string jsonData = JsonUtility.ToJson(_gameData);
            PlayerPrefs.SetString(SAVE_KEY, jsonData);
        }

        public void AddCoins()
        {
           
        }
    }
}