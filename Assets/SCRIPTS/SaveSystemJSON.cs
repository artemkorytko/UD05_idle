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
    public class SaveSystemJSON : MonoBehaviour
    {
        // ключ для PlayerPrefs
        private const string SAVE_KEY = "GameData";

        // ссылка на GameData
        private GameData _gameData;

        private FieldMaganer _fieldManagerFile;

        // вынесли ее в публичное поле заинкапуслировавши:
        public GameData Data => _gameData;

        // private static string Path = Application.persistentDataPath + "/saveData.data";

        //public void Initialize() // было private void Start()
        // public UniTask Initialize(int value)
        public void Initialize()
        {
            _fieldManagerFile = FindObjectOfType<FieldMaganer>();

            // проверяем, если мы уже что-то по этому пути сохранили
            if (PlayerPrefs.HasKey(SAVE_KEY))
            {
                // если нету, то тогда загружаем
                LoadData();
                // return UniTask.CompletedTask; //new
            }

            else // там нету ничего, то создать пустую гейм-дату с нуля
            {
                _gameData = new GameData();
                // заиниченная дата
                // ????????????????????? вот тут он пойдет в конcтруктор ????????????????????
            }
        }

        //-------------------------- достаем и приводим к нашему классу ------------------------------------------
        // public UniTask<bool> LoadData()
        public void LoadData()
        {
            // "десериализовать"
            // получаем строку и записываем в стринг
            string jsonData = PlayerPrefs.GetString(SAVE_KEY);

            Debug.Log($" Выгрузили: {jsonData}");

            // "парсить"
            // достать из джейсона. Указать в какой тип привести - тут надо распарсить в GameData
            _gameData = JsonUtility.FromJson<GameData>(jsonData);
            //return new UniTask<bool>(true);
        }

        //--------------------------- сохраняем -------------------------------------------------------------------
        public void SaveData(GameData _gameData)
        {
            // создать контейнер для сохранения - надо хранить: 1) разблок/заблок 2) уровень

            // передаем объект любого типа, который хотим привести к JSON

            // ТУТ НЕПРАВИЛЬНО РАБОТАЕТ, ОТРЕЗАЕТ ВСЕ КРОМЕ МАНИ!!!!!!!!
            string jsonData = JsonUtility.ToJson(_gameData);

            // эту строку сохраняем в PlayerPrefs
            PlayerPrefs.SetString(SAVE_KEY, jsonData);
        }


        //--------------------------- для полного ресета накликанного ----------------------------------------------
        public void ResetSaved()
        {
            // PlayerPrefs.DeleteAll();
            _gameData = new GameData();
            SaveData(_gameData);
        }
    }
}