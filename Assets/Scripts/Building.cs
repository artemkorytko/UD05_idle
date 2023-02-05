﻿using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DefaultNamespace
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private BuildingConfig config;
        [SerializeField] private Transform modelPoint;

        private const string BUY_TEXT = "BUY";
        private const string UPGRADE_TEXT = "COST";
        private const float DELAY = 1f;

        private BuildingButton _button;
        
        private GameObject _currentModel;

        private BuildingData _data;
        
        private Coroutine _timerCor;

        private void Awake()
        {
            _button = GetComponentInChildren<BuildingButton>();
        }

        private void Start()
        {
            GameManager.Instance.OnMoneyChanged += OnMoneyChanged;
            _button.OnClickEvent += OnButtonClick;
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnMoneyChanged -= OnMoneyChanged;
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
            
            OnMoneyChanged(GameManager.Instance.Money);
        }
        
        private void OnMoneyChanged(float value)
        {
            _button.OnMoneyValueChanged(value);
        }
        
        private void OnButtonClick()
        {
            if (!_data.IsUnlock)
            {
                _data.IsUnlock = true;
                GameManager.Instance.Money -= config.UnlockPrice;
                SetButton(_data.UpgradeLevel);
                SetModel(_data.UpgradeLevel);
                return;
            }

            if (config.IsUpgradeExist(_data.UpgradeLevel + 1))
            {
                _data.UpgradeLevel++;
                GameManager.Instance.Money -= GetCost(_data.UpgradeLevel);
                SetModel(_data.UpgradeLevel);
                SetButton(_data.UpgradeLevel);
            }
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
                if (config.IsUpgradeExist(_data.UpgradeLevel + 1))
                {
                    _button.UpdateButton(UPGRADE_TEXT, GetCost(level + 1));
                }
                else
                {
                    _button.gameObject.SetActive(false);
                }
            }
        }

        private async void SetModel(int level)
        {
            var upgradeConfig = config.GetUpgrade(level);
            if (_currentModel)
            {
                Addressables.ReleaseInstance(_currentModel);
            }

            _currentModel = await Addressables.InstantiateAsync(upgradeConfig.Model, modelPoint);

            if (_timerCor == null)
                _timerCor = StartCoroutine(Timer());
        }
        
        private IEnumerator Timer()
        {
            while (true)
            {
                yield return new WaitForSeconds(DELAY);
                GameManager.Instance.Money += config.GetUpgrade(_data.UpgradeLevel).ProcessResult;
            }
        }

        private float GetCost(int level)
        {
            return (float) System.Math.Round(config.StartUpgradeCost * Mathf.Pow(config.CostMultiplier, level), 2);
        }
    }
}