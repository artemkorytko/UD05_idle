using System;
using Cysharp.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
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

        private BuildingData _data;

        private void Awake()
        {
            _button = GetComponentInChildren<BuildingButton>();
        }

        private void Start()
        {
            _button.OnClickEvent += OnButtonClick;
        }

        private void OnDestroy()
        {
            _button.OnClickEvent -= OnButtonClick;
        }

        public void Initialize(BuildingData data)
        {
            _data = data;
            if (_data.IsUnlock)
            {
                SetModel(_data.UpgradeLevel);
                SetButton(_data.UpgradeLevel);
            }
            else
            {
                SetButton(-1);
            }
        }
        
        private void OnButtonClick()
        {
            
        }

        public BuildingData GetData()
        {
            return _data;
        }

        private void SetButton(int level)
        {
            if (level == -1)
            {
                _button.UpdateButton(BUY_TEXT, config.UnlockPrice);
            }
            else
            {
                _button.UpdateButton(COST_TEXT, config.StartUpgradeCost * config.CostMultiplier);
            }
        }

        private async void SetModel(int level)
        {
            var upgradeConfig = config.GetUpgrade(level);
            if (_currentModel)
            {
                Addressables.ReleaseInstance(_currentModel);
                
                // Destroy(_currentModel);
            }

            _currentModel = await Addressables.InstantiateAsync(upgradeConfig.Model, modelPoint);

            // _currentModel = Instantiate(upgradeConfig.Model, modelPoint);
        }

        private float GetCost(int level)
        {
            return (float) System.Math.Round(config.StartUpgradeCost * Mathf.Pow(config.CostMultiplier, level), 2);
        }
    }
}