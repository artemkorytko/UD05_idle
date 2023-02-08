using System;
using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "BuildingConfig", menuName = "Configs/BuildingConfig", order = 0)]
    public class BuildingConfig : ScriptableObject
    {
        [SerializeField] private string buildingName;
        [SerializeField] private float unlockPrice;
        
        //завести массив цен, ИЛИ будем увеличивать цену на 17% - на любое кол-во апгрейдов
        [SerializeField] private float startUpgradeCost;
        [SerializeField] private float costMultiplier;
        

            // массиd апгрейдов
        [SerializeField] public UpgradeConfig[] upgrades;

        // инкапсулейт alt enter только read
        public float UnlockPrice => unlockPrice;
        public float StartUpgradeCost => startUpgradeCost;

        public float CostMultiplier => costMultiplier;
        public string BuildingName => buildingName;

        public event Action MaxCool; 
        //  возвращает конфиг, в котором моделька и сколько здание приносит денег
        public UpgradeConfig GetUpgrade(int index)
        {
            // не больше ли чем у нас ступеней
            //--------- ТУТ ОШИБКА "ВНЕ МАССИВА" ЕСЛИ АПГРЕЙДИМ ВЫШЕ КОЛИЧЕСТВА КОНФИГОВ У ЗДАНИЯ
            if (index >= upgrades.Length )
            {
                MaxCool?.Invoke(); // идет в здание и оттуда пишет в кнопке что круче некуда
                return null;
            }
            
            // if (index >= upgrades.Length) Debug.Log( $"index стал {index}" );

            return upgrades[index]; 
        }

        public bool DoesUgradeExist(int index)
        {
            return index >= 0 && index <= upgrades.Length -1;
            // без -1 покупался несуществующий апгрейд! 
        }
    }
}