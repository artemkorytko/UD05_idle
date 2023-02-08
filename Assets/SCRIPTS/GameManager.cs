using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Firebase.Analytics;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using UnityEngine;

//------ хранит, получает и инициализирует всех остальных --------------
//----- вместе с save системой висит на GameManager ! ------------------
//----- это точка входа в игру - у него вызовется юнитёвский метод awake 
namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] private SaveSystemType saveSystemType;

        // private SaveSystemJSON _saveSystem;
        private ISaveSystem _saveSystem;
        private FieldMaganer _fieldManager;


        private GameData _gameData; //вот это как бы массив :///

        private GamePanel _gamePanelFile;

        //------- стеки для undo и таймпасс
        // private Stack <GameData> _undoStack;
        //  private Stack <GameData> _timepassStack;

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


            //================ ПЕРЕКЛЮЧАТЕЛЬ наследников интерфейса ======================
            // было:
            // _saveSystem = GetComponent<SaveSystemJSON>();
            // _saveSystem = new SaveSystemJSON();
            // _saveSystem = new SaveSystemBin();
            _saveSystem = new SaveSystemFirebase();
            //============================================================================
            // проверка что работает вообще:
            // Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventLogin);

            //=============== новый большой пепеключатель систем сохранения ==============
            
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLogin);
            switch (saveSystemType)
            {
                case SaveSystemType.None:
                    Debug.LogError("No save system type");
                    break;
                case SaveSystemType.Json:
                    _saveSystem = new SaveSystemJSON();
                    break;
                case SaveSystemType.Bin:
                    _saveSystem = new SaveSystemBin();
                    break;
                case SaveSystemType.Firebase:
                    _saveSystem = new SaveSystemFirebase();
                    break;
            }


            _fieldManager = GetComponentInChildren<FieldMaganer>();

            _gamePanelFile = FindObjectOfType<GamePanel>();

            //_undoStack = new Stack<GameData>();
            //_timepassStack = new Stack<GameData>();
        }

        private async void Start()
        {
            if (Instance != this)
                return;

            await _saveSystem.Initialize(await FetchDataAsync()); // load data
            _gameData = _saveSystem.GameData; // get data
            _fieldManager.Initialize(_gameData); //set data
        }

        // -----------------------------------------
        
        private async UniTask<int> FetchDataAsync()
        {
            await FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero).AsUniTask();
            await FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWithOnMainThread(
                task => { Debug.Log($"Remote data loaded and ready for use."); }).AsUniTask();

            // ОШИБКА ТУТ, если пишем ноль вместо этой строчки, то пойдет нормально дальше
            // юнити тут выдает: "FormatException: Input string was not in a correct format."
            // var value = 55;
            var value = FirebaseRemoteConfig.DefaultInstance.GetValue("Money").LongValue;
            return (int)value;
        }

        //--------------------------- конец игры -----------------------------------------------
        private void OnDestroy()
        {
            if (Instance != this)
                return;
            _gameData.BuildingsData = _fieldManager.GetBuildingData(); // get data from buildings
            _saveSystem.SaveData(); // save data
        }


//=================== для полного ресета накликанного =========================================================
        public void ResetAllSaved()
        {
            _fieldManager.StopBuildingTimers();
            _saveSystem.ResetSaved();
            
            //_gameData = new GameData(); // очистит плеерпрефс
            //_saveSystem.SaveData(_gameData); // и сохранит пустой
            //%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
            _gameData = new GameData(85); // очистит плеерпрефс
            _saveSystem.SaveData(); // и сохранит пустой

            _fieldManager.Initialize(_gameData);
            _gamePanelFile.SetMoneyOnPanel(_gameData.Money);
        }
    }
}