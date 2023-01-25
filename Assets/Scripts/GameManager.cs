using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        private SaveSystem _saveSystem;
        private FieldManager _fieldManager;


        private GameData _gameData;
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