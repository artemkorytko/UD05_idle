using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class FieldManager : MonoBehaviour
    {
        private Building[] _buildings;

        private void Awake()
        {
            _buildings = GetComponentsInChildren<Building>();
        }

        public void Initialize(GameData gameData)
        {
            var data = gameData.BuildingsData;
            for (int i = 0; i < _buildings.Length; i++)
            {
                _buildings[i].Initialize(data[i]);
            }
        }

        public BuildingData[] GetBuildingData()
        {
            var data = new BuildingData[GameData.BuildCount];

            for (int i = 0; i < _buildings.Length; i++)
            {
                data[i] = _buildings[i].GetData();
            }

            return data;
        }
    }
}