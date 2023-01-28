using UnityEngine;


//----- тут про все здания и общее кол-во баблишка
namespace DefaultNamespace
{
    public class GameData
    {
        // сколько зданий по умолчанию
        public const int BuildCount = 6;
        
        // поля где хранится значения - тут флотовое, будет храниться по ключу Money
        public float Money = 60f;
        
        // массив хранит дату зданий
        public BuildingsData[] BuildingsData; // массив зданий, там внутри инты и булей, будет храниться по ключу BuildingData
        
        // конструктор инициализирует наш массив и заполняет дефолтной информацией
        public GameData()
        {
            // перебираем сколько у нас есть домов
            BuildingsData = new BuildingsData[BuildCount];

            // заполняем их 
            for (int i = 0; i < BuildCount; i++)
            {
                BuildingsData[i] = new BuildingsData();
            }
        }

    }
}