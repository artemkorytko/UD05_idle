using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;

namespace DefaultNamespace
{
    public class SaveSystem : MonoBehaviour
    {
        private const string SAVE_KEY = "GameData";
        
        private GameData _gameData;

        public GameData Data { get; private set; }

        public void Initialize()
        {
            // if (PlayerPrefs.HasKey(SAVE_KEY))
            // {
            //     LoadDataFromJson();
            // }
            string path = Application.persistentDataPath +"/saveData.data";
            if (File.Exists(path))
            {
               LoadDataFromBin(); 
            }
            else
            {
                Data = new GameData();
            }
        }
        
        private void LoadDataFromJson()
        {
            string jsonData = PlayerPrefs.GetString(SAVE_KEY);
            Data = JsonUtility.FromJson<GameData>(jsonData);
        }

        public void SaveDataToJson()
        {
            string jsonData = JsonUtility.ToJson(Data);
            PlayerPrefs.SetString(SAVE_KEY, jsonData);
        }

        private void LoadDataFromBin()
        {
            string path = Application.persistentDataPath + "/saveData.data";
            FileStream fileStream = new FileStream(path, FileMode.Open);
            
            BinaryFormatter converter = new BinaryFormatter();
            Data= converter.Deserialize(fileStream) as GameData;
            fileStream.Close();
        }

        public void SaveDataToBin()
        {
            string path = Application.persistentDataPath + "/saveData.data";
            FileStream dataStream = new FileStream(path, FileMode.Create);

            BinaryFormatter converter = new BinaryFormatter();
            converter.Serialize(dataStream, Data);
            dataStream.Close();
        }
        
        
    }

   
}