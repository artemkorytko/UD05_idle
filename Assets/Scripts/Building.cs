using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private BuildingConfig config;
        [SerializeField] private Transform modelPoint;

        private const string BUY_TEXT = "BUY";
        private const string COST_TEXT = "COST";

        private BuildingButton _button;
        
        private GameObject _currentModel;

        private void Start()
        {
            _button = GetComponentInChildren<BuildingButton>();
            SetModel(0);
            SetButton(0);
        }

        private void SetButton(int level)
        {
            if (level == 0)
            {
                _button.UpdateButton(BUY_TEXT, config.UnlockPrice);
            }
            _button.UpdateButton(COST_TEXT, config.StartUpgradeCost * config.CostMultiplier);
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