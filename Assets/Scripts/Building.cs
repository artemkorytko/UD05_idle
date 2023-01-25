using System;
using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace
{
    public class Building : MonoBehaviour
    {
        private const string BUY_TEXT = "BUY";
        private const string UPGRADE_TEXT = "UPGRADE";

        [SerializeField] private BuildingConfig config;
        [SerializeField] private Transform modelPoint;
        private BuildingButton _button;

        private GameObject _currentModel;

        private void Start()
        {
            _button = GetComponentInChildren<BuildingButton>();
            SetModel(0);
            SetButton(1);
        }

        private void SetButton(int level)
        {
            if (level == 0)
            {
                _button.UpdateButton(BUY_TEXT, config.UnlockPrice);
            }
            else
            {
                _button.UpdateButton(UPGRADE_TEXT, GetCost(level));
            }
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
