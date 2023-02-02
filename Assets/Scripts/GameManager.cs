using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using Firebase.Analytics;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        [SerializeField] private SaveSystemType saveSystemType;
        private ISaveSystem saveSystem;
        private FieldManager _fieldManager;

        private GameData _gameData;

        public event Action<float> OnMoneyChanged;

        public float Money
        {
            get => _gameData.Money;

            set
            {
                if (value == _gameData.Money)
                    return;

                if (value < 0)
                {
                    _gameData.Money = 0;
                }

                _gameData.Money = (float) Math.Round(value, 2);
                OnMoneyChanged?.Invoke(_gameData.Money);
            }
        }

        private void Awake()
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


            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLogin);
            switch (saveSystemType)
            {
                case SaveSystemType.None:
                    Debug.LogError("No save system type");
                    break;
                case SaveSystemType.Json:
                    saveSystem = new SaveSystemJson();
                    break;
                case SaveSystemType.Bin:
                    saveSystem = new SaveSystemBin();
                    break;
                case SaveSystemType.Firebase:
                    saveSystem = new SaveSystemFirebase();
                    break;
            }


            _fieldManager = GetComponentInChildren<FieldManager>();
        }

        private async void Start()
        {
            if (Instance != this)
                return;
           
           
            await saveSystem.Initialize(await FetchDataAsync()); // load data
            _gameData = saveSystem.GameData; // get data
            _fieldManager.Initialize(_gameData); //set data
        }

        private void OnDestroy()
        {
            if (Instance != this)
                return;
            _gameData.BuildingsData = _fieldManager.GetBuildingData(); // get data from buildings
            saveSystem.SaveData(); // save data
        }

        private async UniTask<int> FetchDataAsync()
        {
            await FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero).AsUniTask();
            await FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWithOnMainThread(
                task => { Debug.Log($"Remote data loaded and ready for use."); }).AsUniTask();
            var value = FirebaseRemoteConfig.DefaultInstance.GetValue("Money").LongValue;
            return (int) value;
        }
    }
}