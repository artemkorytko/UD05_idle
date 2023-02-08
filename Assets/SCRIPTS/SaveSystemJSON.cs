using System;
using UnityEngine;
using Cysharp.Threading.Tasks;


// висит на пустом объекте GameManager
// отвечает только за сохранение инфы - загрузить, выгрузить

//--------------- сохраняет 1 пачкой - построена на PlayerPrefs + JSON --------------------------------
// сохранять будем здания - разблокировано ли + его уровень
// это в классе BuildingData
// Можно хранить в JSon: string, int, float, bool, char / null.

namespace DefaultNamespace
{
    // для интерфейса
    // public class SaveSystem : MonoBehaviour, ISaveSystem
    
    [Serializable]
    public class SaveSystemJSON : ISaveSystem
    {
        // ключ для PlayerPrefs
        private const string SAVE_KEY = "GameData";

        // ссылка на GameData
        private GameData gameGameData;

        private FieldMaganer _fieldManagerFile;

        // вынесли ее в публичное поле заинкапуслировавши:
        public GameData GameData => gameGameData;

        // private static string Path = Application.persistentDataPath + "/saveData.data";

        //public void Initialize() // было private void Start()
        // public UniTask Initialize(int value)
        public UniTask Initialize(int value)
        {
            //_fieldManagerFile = FindObjectOfType<FieldMaganer>();

            // проверяем, если мы уже что-то по этому пути сохранили
            if (PlayerPrefs.HasKey(SAVE_KEY))
            {
                // если нету, то тогда загружаем
                LoadData();
                // return UniTask.CompletedTask; //new
            }

            else // там нету ничего, то создать пустую гейм-дату с нуля
            {
                gameGameData = new GameData(value);
                // заиниченная дата
                // ????????????????????? вот тут он пойдет в конcтруктор ????????????????????
            }
            return UniTask.CompletedTask;
        }

        //-------------------------- достаем и приводим к нашему классу ------------------------------------------
        // public UniTask<bool> LoadData()
        public UniTask<bool> LoadData()
        {
            // "десериализовать"
            // получаем строку и записываем в стринг
            string jsonData = PlayerPrefs.GetString(SAVE_KEY);

            Debug.Log($" Выгрузили: {jsonData}");

            // "парсить"
            // достать из джейсона. Указать в какой тип привести - тут надо распарсить в GameData
            gameGameData = JsonUtility.FromJson<GameData>(jsonData);
            //return new UniTask<bool>(true);
            
            return new UniTask<bool>(true);
        }

        //--------------------------- сохраняем -------------------------------------------------------------------
        public void SaveData()
        {
            // создать контейнер для сохранения - надо хранить: 1) разблок/заблок 2) уровень

            // передаем объект любого типа, который хотим привести к JSON

            // ТУТ НЕПРАВИЛЬНО РАБОТАЕТ, ОТРЕЗАЕТ ВСЕ КРОМЕ МАНИ!!!!!!!!
            string jsonData = JsonUtility.ToJson(gameGameData);

            // эту строку сохраняем в PlayerPrefs
            PlayerPrefs.SetString(SAVE_KEY, jsonData);
        }


        //--------------------------- для полного ресета накликанного ----------------------------------------------
        public void ResetSaved()
        {
            // PlayerPrefs.DeleteAll();
            gameGameData = new GameData(666);
            //SaveData(gameGameData);
            SaveData();
        }
    }
}