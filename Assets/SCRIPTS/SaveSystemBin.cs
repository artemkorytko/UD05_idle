using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

// для 2 способа
using System.IO; //!!!!!! для работы с файлами
using System.Runtime.Serialization.Formatters.Binary; // бинарники!!!

//======================================== ВАРИАНТ 2 =======================================================
// за пределами проекта контролируя пусть
// сохранение подходит для мобилок нормально

namespace DefaultNamespace
{
    [Serializable]
    public class SaveSystemBin : ISaveSystem
    {
        private GameData gameGameData;

        public GameData GameData => gameGameData;
        private static string Path;
        

        public UniTask Initialize(int value)
        {
            //----------------- путь --------------------------------
            Path = Application.persistentDataPath + "/saveData.data";
            // или .bin .bat
            // путь тут ибо иначе не успеет вызваться
            //-------------------------------------------------------
            
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
            // параметры: путь куда сохраняем, create перезатрет если есть

            BinaryFormatter converter = new BinaryFormatter(); // конвертирует в биты и байты
            converter.Serialize(dataStream, gameGameData); // куда, кого
            dataStream.Close(); // закрыть стрим
        }

        public UniTask <bool> LoadData()
        {
            FileStream fileStream = new FileStream(Path, FileMode.Open); 
            // откуда, прочитать - открыть стрим

            BinaryFormatter converter = new BinaryFormatter();
            
            // записывает дату как GameData
            gameGameData = converter.Deserialize(fileStream) as GameData;
            
            fileStream.Close();
            return new UniTask<bool>(true);
        }
        
        
        public void ResetSaved()
        { 
            gameGameData = new GameData(444);
            SaveData();
        }
    }
}
