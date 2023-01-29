using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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
        private Stack<GameData> _undoStack;
        
        private Stack<GameData> _timepassStack;
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
            _snap = _gameData;
            _snap.BuildingsData = _fieldManager.GetBuildingData();
            _undoStack.Push(_snap);

            Debug.Log($" snap added, {_undoStack.Count}");
        }

        public void Undo()
        {
            if (_undoStack.Count == 1)
            {
                _prewsnap = _undoStack.Peek();
                _fieldManager.Initialize(_prewsnap);
                
                ReloadAllBuildingsToPrew();
                return;
            }

            if (_undoStack.Count > 1)
            {
                // _gameData = new GameData();
                _prewsnap = new GameData();

                // ????????????????????????????????????????????????????????
                // ПРЕДПОЛОЖИТЕЛЬНО ПРОБЛЕМА ТУТ :((((((((((((((((
                // ????????????????????????????????????????????????????????
                // _undoStack.Pop(); // выкидывает верхнюю

                _prewsnap = _undoStack.Peek(); // смотрит что осталось сверху 
                //Debug.Log($" в стеке осталось {_undoStack.Count}");
                
                _undoStack.Pop();
                //Debug.Log($" в стеке осталось {_undoStack.Count}");

                ReloadAllBuildingsToPrew();
                return;
            }

            Debug.Log(" В стеке ничего нет! ");
        }

        //----------------- для Undo -----------------------------------------------------------------------------
        public void ReloadAllBuildingsToPrew()
        {
            _fieldManager.Initialize(_prewsnap);
            
        }


        //----------------- Прокрутка событий с начала до сейчас --------------------------------------------------
        //----------------- надо бы undo сохранять тоже.... -------------------------------------------------------
        public async void Timepass()
        { //-------- сейчас тут в стеках тасуется, значение передается, модельки мигают, но не меняются :/ --------- 
            
            _timepassStack = new Stack<GameData>();
            GameData inwork = null;
            Debug.Log($" в undo стеке - {_undoStack.Count}");

            
            
            // переложить все в новый стек, и первое будет свеху лежать. наверноое))
            int countem = _undoStack.Count;
            for (int i = 1; i < countem; i++)
            {
                inwork = _undoStack.Peek();
                _timepassStack.Push(inwork);
                _undoStack.Pop();
                Debug.Log($" в undo стеке - {_undoStack.Count}, в таймпасс {_timepassStack.Count}");
            }
            
            // типо очистить
            GameData empty = new GameData();
            _fieldManager.Initialize(empty);

            
            
            // типо по одному перебрать второй стек и показывать с бывшего нижнего
            int countemagain = _timepassStack.Count;
            for (int i = 1; i < countemagain; i++)
            {
                inwork = _timepassStack.Peek(); 
                _fieldManager.Initialize(inwork); 
                _timepassStack.Pop(); 
                await UniTask.Delay(TimeSpan.FromSeconds(1));
                Debug.Log($" в timepass стеке - {_timepassStack.Count}");
            }

        }
    }
}