using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private BuildingConfig config;
        [SerializeField] private Transform modelPoint;
        private BuildingButton _button;
        
        private GameObject _currentModel;

        private void Start()
        {
            _button = GetComponentInChildren<BuildingButton>();
            SetModel(0);
            SetButton();
            
        }

        private void SetButton()
        { 
            _button.UpdateButton("Upgrade", config.StartUpgradeCost * config.CostMultiplier);
        }


        private void SetModel(int level)
        {
            var upgradeConfig = config.GetUpgrade(level);
            if (_currentModel)
            {
                Destroy(_currentModel);
            }

            _currentModel = Instantiate(upgradeConfig.Model, modelPoint);
        }

        private float GetCost(int level)
        {
           return (float) System.Math.Round(config.StartUpgradeCost * Mathf.Pow(config.CostMultiplier, level), 2);
        }
    }
}