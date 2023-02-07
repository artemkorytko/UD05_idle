using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public class SaveSystemJson : ISaveSystem 
    {
        private const string SAVE_KEY = "GameData";
        
        private GameData gameGameData;

        public GameData GameData { get; private set; }
        public UniTask Initialize(int value)
        {
            if (PlayerPrefs.HasKey(SAVE_KEY))  
            {
                LoadData();
            }
            else
            {
                gameGameData = new GameData(value);
            }

            return UniTask.CompletedTask;
        }
        
        public UniTask<bool> LoadData()
        {
            string jsonData = PlayerPrefs.GetString(SAVE_KEY);
            GameData = JsonUtility.FromJson<GameData>(jsonData);
            return new UniTask<bool>(true);
        }

        public void SaveData()
        {
            string jsonData = JsonUtility.ToJson(gameGameData);
            PlayerPrefs.SetString(SAVE_KEY, jsonData);
        }
        
    }

   
}