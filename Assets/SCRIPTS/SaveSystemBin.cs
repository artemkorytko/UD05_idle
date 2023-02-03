using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Cysharp.Threading.Tasks;
using UnityEngine;


//======================================== ВАРИАНТ 2 =======================================================
// за пределами проекта контролируя пусть
// сохранение подходит для мобилок нормально
/*
namespace DefaultNamespace
{
    [Serializable]
    public class SaveSystemBin : ISaveSystem
    {
        private GameData gameGameData;

        public GameData GameData => gameGameData;
        private string Path;

        public UniTask Initialize(int value)
        {
            Path = Application.persistentDataPath + "/saveData.data";
             // или .bin .bat
            
            if (File.Exists(Path))
            {
                LoadData();
            }
            else
            {
                gameGameData = new GameData(value);
            }
            return UniTask.CompletedTask;
        }

        public void SaveData()
        {
            FileStream dataStream = new FileStream(Path, FileMode.Create);
             // create перезатрет если есть

            BinaryFormatter converter = new BinaryFormatter();
            converter.Serialize(dataStream, gameGameData);
            dataStream.Close();
        }

        public UniTask <bool> LoadData()
        {
            FileStream fileStream = new FileStream(Path, FileMode.Open);

            BinaryFormatter converter = new BinaryFormatter();
            gameGameData = converter.Deserialize(fileStream) as GameData;
            fileStream.Close();
            return new UniTask<bool>(true);
        }
    }
}
*/