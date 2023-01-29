using System;
using UnityEngine;

//---------- висит на префабе с горами и камнями, который дочерний у гейм-менеджера  ---------
// знает о всех зданях на локации и им рассказывает им

namespace DefaultNamespace
{
    // будет получать инфу от сейв системы
    // хранить ссылки на все здания какие есть на уровне
    // раздает и собирает инфу о зданиях на старте
    public class FieldMaganer : MonoBehaviour
    {
        private Building[] _buildings;

        private void Awake()
        {
            // ------- тут проблема
            // находим их ВСЕХ
            _buildings = GetComponentsInChildren<Building>();
        }

        //--------------- получаем инфу GameData из Start() в GameManager и раздает массив по зданиям ---------------
        public void Initialize(GameData gameData)
        {
            // получаем
            var data = gameData.BuildingsData;

            // раздаем по зданиям и говорим им инициализироваться, передавая им дату
            // for (int i = 0; i < _buildings.Length; i++)
            for (int i = 0; i < _buildings.Length; i++)
            {
                _buildings[i].Initialize(data[i]);
            }
        }

        //--------------- полученме текущее состояние со всемх зданий -----------------------------
        // собираем инфу и передаем сейв системе
        // возвращает массив!
        public BuildingsData[] GetBuildingData()
        {
            var data = new BuildingsData[GameData.BuildCount]; // или по размеру массива выше

            for (int i = 0; i < _buildings.Length; i++)
            {
                data[i] = _buildings[i].GetData();
            }

            return data;
        }
    }
}