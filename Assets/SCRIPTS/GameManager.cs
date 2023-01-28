using System;
using System.Collections.Generic;
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
        
        //мое
        private MoneyManager _moneyManagerfile;

        private GameData _gameData; //вот это чего? массив??????????????
        public GameData GameDataInGameManager => _gameData; // на выход в GamePanel

        private GamePanel _gamePanelFile;

        //------- стек для undo
        private Stack <GameData> _undoStack;
        private GameData _prewsnap;
        private GameData _snap;

        private void Awake() // нахрдим наши системы 
        {
            _saveSystem = GetComponent<SaveSystem>();
            _fieldManager = GetComponentInChildren<FieldMaganer>();

            _moneyManagerfile = FindObjectOfType<MoneyManager>();
            _gamePanelFile = FindObjectOfType<GamePanel>();

            _undoStack = new Stack<GameData>();
        }

        private void Start()
        {
            _saveSystem.Initialize(); // обращаемся к сейв системе, она ищет ключ и грузит дату - дoad data
            _gameData = _saveSystem.Data; // get data - получить ссылку на нашу дату из сейв системы

            // говорим филдменеджеру инициализируйся и передаем ему туда вглубь геймдату
            // а он передаст зданиям и инициализирует
            _fieldManager.Initialize(_gameData); // set data - отправить/присвоить
            
            // посылаю в панель сколько денег и пусть отображает, также шлю их в файл про бабло
            _gamePanelFile.SetMoneyOnPanel(_gameData.Money);
            _moneyManagerfile.AddMoney(_gameData.Money);
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

        //============= стек для отмены действий =====================================
        //----------- ну работало же нормально!!!! -----------------------------------
        public void Snapshot()
        {
            _snap = new GameData();
            _snap.BuildingsData = _fieldManager.GetBuildingData();
            _undoStack.Push(_snap);
            
            Debug.Log($" snap added, {_undoStack.Count}");
        }

        public void Undo()
        {
            if (_undoStack.Count > 1)
            {
                _prewsnap = new GameData();
                
                // какого-то перестало работать :((((((((((((((((
                _undoStack.Pop(); // выкидывает верхнюю
                _prewsnap = _undoStack.Peek(); // смотрит что осталось сверху 
                Debug.Log($" в стеке осталось {_undoStack.Count}");
                
                _fieldManager.Initialize(_prewsnap);
            }
            else
            {
                Debug.Log(" В стеке ничего не осталось!");
            }


            
        }
    }
}