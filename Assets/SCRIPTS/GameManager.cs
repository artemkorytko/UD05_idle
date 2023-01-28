using System;
using UnityEngine;

//------ хранит, получает и инициализирует всех остальных --------------
//----- вместе с save системой висит на GameManager ! ------------------
//----- это точка входа в игру - у него вызовется юнитёвский метод awake 
namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {

        private SaveSystem _saveSystem;
        private FieldMaganer _fieldManager;

        private GameData _gameData; //вот это чего? массив??????????????

        private void Awake() // нахрдим наши системы 
        {
            _saveSystem = GetComponent<SaveSystem>();
            _fieldManager = GetComponentInChildren<FieldMaganer>();
        }

        private void Start()
        {
            _saveSystem.Initialize(); // обращаемся к сейв системе, она ищет ключ и грузит дату - дoad data
            _gameData = _saveSystem.Data; // get data - получить ссылку на нушу дату из сейв системы

            // говорим филдменеджеру инициализируйся и передаем ему туда вглубь геймдату
            // а он передаст зданиям и инициализирует
            _fieldManager.Initialize(_gameData); // set data - отправить/присвоить
        }

        // конец игры 
        private void OnDestroy()
        {
            // обращаемся к FieldManager, он соберет дату в массив со всех зданий, вернет обратно
            // вот тут не совсем понятно ----- куда оно?
            // массив в файле BuildingsData - шлём в FieldManager в функцию получить инфу,
            // тот делает новый массив из отдельных дат зданий, туда берет внутри каждого здания 
            // свой экземпляр BuildingsData
            _gameData.BuildingsData = _fieldManager.GetBuildingData();
            
            // говорим сохранить - в этот момент филд менеджер уже распихал инфу по массиву в файле GameData
            _saveSystem.SaveData();
        }
    }
}