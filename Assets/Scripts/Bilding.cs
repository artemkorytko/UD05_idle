using System;
using UnityEngine;

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

        private void Start()
        {
            _bildingButton = GetComponentInChildren<BildingButton>();
            SetModel(0);
            SetButton(0);
        }

        private void SetButton(int level)
        {
            if (level == 0)
            {
              _bildingButton.UpdateButton(BUY_TEXT, config.UnlockPrice);  
            }
            else
            {
                _bildingButton.UpdateButton(UPGRADE_TEXT, GetCost(level));
            }
        
            _bildingButton.UpdateButton("Upgrade", config.StartUpgradeCost * config.CostMultiplayer);
        }

        private void SetModel(int level)
        {
            var upgradeConf = config.GetUpgrade(level);
            
            if(_currentModel)
                Destroy(_currentModel);
            
            _currentModel = Instantiate(upgradeConf.Model, modelPoint);
        }

        private float GetCost(int level)
        {
           return  (float) System.Math.Round(config.StartUpgradeCost * Mathf.Pow(config.CostMultiplayer * 0.01f,level), 2);
        }
    }
}