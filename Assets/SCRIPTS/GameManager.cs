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
        public static GameManager Instance { get; private set; }

        private SaveSystem _saveSystem;
        private FieldMaganer _fieldManager;
        

        private GameData _gameData; //вот это как бы массив :///

        private GamePanel _gamePanelFile;

        //------- стеки для undo и таймпасс
        private Stack<GameData> _undoStack;
        private Stack<GameData> _timepassStack;
        
        private GameData _prewsnap;
        private GameData _snap;

         public event Action<float> OnMoneyChanged;

         
         //---------------------------------------------------------------------------------
         public float Money
         {
             get => _gameData.Money;

             //----- !! эту переменную меняют здания !!------------------------
             set // записывать сюда значение - под контролем читать и записывать 
             {
                 if (value == _gameData.Money)
                     return; // ниче не делаем если денег столько и было
                 
                 // проверка шоб не отрицательное
                 if (value < 0)
                 {
                     _gameData.Money = 0;
                 }
                 else _gameData.Money = (float)Math.Round(value, 2); // округлить

                 // СОБЫТИЕ ПРОИСХОДИТ КОГДА МЕНЯЮТ ПЕРЕМЕННУЮ ИЗВНЕ, ДА????
                 OnMoneyChanged?.Invoke(_gameData.Money); // и вот это будет value!
             }
         }

         private void Awake() // находим наши системы 
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                DestroyImmediate(gameObject);
                return;
            }

            
            
            _saveSystem = GetComponent<SaveSystem>();
            _fieldManager = GetComponentInChildren<FieldMaganer>();
            
            _gamePanelFile = FindObjectOfType<GamePanel>();

            _undoStack = new Stack<GameData>();
            _timepassStack = new Stack<GameData>();
        }

        private void Start()
        {
            if (Instance != this)
                return;
            
            
            _saveSystem.Initialize(); // обращаемся к сейв системе, она ищет ключ и грузит дату - дoad data
            _gameData = _saveSystem.Data; // get data - получить ссылку на нашу дату из сейв системы

            if (_gameData != null)
            {
                // говорим филдменеджеру инициализируйся и передаем ему туда вглубь геймдату
                // а он передаст зданиям и инициализирует
                _fieldManager.Initialize(_gameData); // set data - отправить/присвоить

                // посылаю в панель сколько денег и пусть отображает
                _gamePanelFile.SetMoneyOnPanel(_gameData.Money);
            }
            else _gameData = new GameData();

        }


        //--------------------------- конец игры ---
        private void OnDestroy()
        {
            // обращаемся к FieldManager, он соберет дату в массив со всех зданий, вернет обратно
            // вот тут не совсем понятно ----- куда оно?
            // массив в файле BuildingsData - шлём в FieldManager в функцию получить инфу,
            // тот делает новый массив из отдельных дат зданий, туда берет внутри каждого здания 
            // свой экземпляр BuildingsData
            _gameData.BuildingsData = _fieldManager.GetBuildingData();

            // говорим сохранить - в этот момент филд менеджер уже распихал инфу по массиву в файле GameData
            _saveSystem.SaveData(_gameData);
        }


        //============= стек для отмены действий =====================================
        //----------- ну работало же нормально!!!! -----------------------------------
        public void Snapshot() // по любому нажатию кнопки на здании, снимает ДО замены здания
        {
            //как задать уникальное имя
            /*
             GameData _snap = new GameData();
            _snap.BuildingsData = _fieldManager.GetBuildingData();

            _undoStack.Push(_snap);

            //Debug.Log($" snap added, {_undoStack.Count}");
            */
        }

        // ---------------------------------------------------------------------------------------------------------
        public void KozelDebug()
        {
            Debug.Log($"KOZEL----- Stack count: {_undoStack.Count} ");
        }


        /*
        // ---------------------------------------------------------------------------------------------------------
        public void Undo()
        {
            // анду 1 раз ---  и то не работает
            // _gameData = _snap;
            // _fieldManager.Initialize(_snap);
            // ReloadAllBuildingsToPrew();

            if (_undoStack.Count == 1)
            {
                // АК сказал ПОПать!!
                // поп берет крайнюю, а потом выкидывает --------
                //_prewsnap = _undoStack.Pop();
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

                //_prewsnap = _undoStack.Peek(); // смотрит что осталось сверху 
                //Debug.Log($" в стеке осталось {_undoStack.Count}");

                _prewsnap = _undoStack.Pop();
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

        public async void Timepass()
        {
            //-------- сейчас тут в стеках тасуется, значение передается, модельки мигают, но не меняются :/ --------- 

            //_timepassStack = new Stack<GameData>();
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
        */

        //=================== для полного ресета накликанного =========================================================
        public void ResetAllSaved()
        {
            _saveSystem.ResetSaved(); 
            _gameData = new GameData(); // очистит плеерпрефс
            _saveSystem.SaveData(_gameData); // и сохранит пустой
            
            //--------- больше НЕ РАБОТАЕТ! -----------------
            _fieldManager.Initialize(_gameData);
            _gamePanelFile.SetMoneyOnPanel(_gameData.Money);
            _fieldManager.StopBuildingTimers();
            
            // очистить стеки
            // EmptyAllStacks();
            // _saveSystem.SaveData(_gameData);
        }

        //----------------- очистка стеков по ресету из SaveSystem
        private void EmptyAllStacks()
        {
            _undoStack.Clear();
            _timepassStack.Clear();
        }
    }
}