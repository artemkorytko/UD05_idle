using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private SaveSystem _saveSystem;
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


            _saveSystem = GetComponent<SaveSystem>();
            _fieldManager = GetComponentInChildren<FieldManager>();
        }

        private void Start()
        {
            if (Instance != this)
                return;

            _saveSystem.Initialize(); // load data
            _gameData = _saveSystem.Data; // get data
            _fieldManager.Initialize(_gameData); //set data
        }

        private void OnDestroy()
        {
            if (Instance != this)
                return;
            _gameData.BuildingsData = _fieldManager.GetBuildingData(); // get data from buildings
            _saveSystem.SaveData(); // save data
        }
    }
}