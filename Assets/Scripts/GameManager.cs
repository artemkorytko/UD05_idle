using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : BaseSingleton<GameManager>
    {
        private SaveSystem _saveSystem;  
        private FieldManager _fieldManager; 
        private GameData _gameData;
        public event Action<float> OnManeyChanged;

        public float Money
        {
            get => _gameData.Money;
            set
            {
                if(value == _gameData.Money)
                    return;
                
                if (value < 0)
                {
                    _gameData.Money = 0;
                }

                _gameData.Money = (float) Math.Round(value, 2);
                OnManeyChanged?.Invoke(_gameData.Money);
            }
        }

        private void Awake()
        {
            _saveSystem = GetComponent<SaveSystem>();
            _fieldManager = GetComponentInChildren<FieldManager>();
        }

        private void Start()
        {
            _saveSystem.Initialize();
            _gameData = _saveSystem.GameData;
            _fieldManager.Initialize(_gameData);
        }

        private void OnDestroy()
        {
           _gameData.BildingsData = _fieldManager.GetBildingDatas();
           _saveSystem.SaveData();
        }
    }
}