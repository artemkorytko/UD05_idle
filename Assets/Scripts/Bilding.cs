using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DefaultNamespace
{
    public class Bilding : MonoBehaviour
    {
        private const string BUY_TEXT = "BUY";
        private const string UPGRADE_TEXT = "UPGRADE";
        
        [SerializeField] private BildingConfig config;
        [SerializeField] private Transform modelPoint;
        
         private BildingButton _bildingButton;
         private GameObject _currentModel;

         private BildingData _data;

        public void Initialize(BildingData data)
        {
            _data = data;
            _bildingButton = GetComponentInChildren<BildingButton>();
            
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

        public BildingData GetData()
        {
            _data.IsUnlock = true;
            return _data;
        }

        private void SetButton(int level)
        {
            if (level == -1)
            {
                _bildingButton.UpdateButton(BUY_TEXT, config.UnlockPrice);  
            }
            else
            { 
                _bildingButton.UpdateButton(UPGRADE_TEXT, GetCost(level));
            }
        
            _bildingButton.UpdateButton("Upgrade", config.StartUpgradeCost * config.CostMultiplayer);
        }

        private async void SetModel(int level)
        {
            var upgradeConf = config.GetUpgrade(level);

            if (_currentModel)
            {
                Addressables.ReleaseInstance(_currentModel);
                //Destroy(_currentModel);
            }


            _currentModel = await Addressables.InstantiateAsync(upgradeConf.Model, modelPoint);
            //_currentModel = Instantiate(upgradeConf.Model, modelPoint);
        }

        private float GetCost(int level)
        {
           return (float) Math.Round(config.StartUpgradeCost * Mathf.Pow(config.CostMultiplayer * 0.01f,level), 2);
        }
    }
}