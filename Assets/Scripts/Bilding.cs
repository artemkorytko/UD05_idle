using System;
using System.Collections;
using System.Timers;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DefaultNamespace
{
    public class Bilding : MonoBehaviour
    {
         private const string BUY_TEXT = "BUY";
         private const string UPGRADE_TEXT = "UPGRADE";
         private const float DELAY = 1f;
        
         [SerializeField] private BildingConfig bildingConfig;
         [SerializeField] private Transform modelPoint;
        
         private BildingButton _bildingButton;
         private GameObject _currentModel;
         private BildingData _bildingData;

         private Coroutine _timerCoroutine;
         
         private void Awake()
         {
             _bildingButton = GetComponentInChildren<BildingButton>();
         }

         private void Start()
         {
             GameManager.Instance.OnManeyChanged += OnManeyChanged;
             _bildingButton.OnClickUpgrade += OnButtonClick;
         }
         
         private void OnDestroy()
         {
             GameManager.Instance.OnManeyChanged -= OnManeyChanged;
             _bildingButton.OnClickUpgrade -= OnButtonClick; 
         }

         private void OnManeyChanged(float value)
         {
             _bildingButton.OnManeyValueChanged(value);
         }
         
         private void OnButtonClick()
         {
             if (!_bildingData.IsUnlock)
             {
                 _bildingData.IsUnlock = true;
                 GameManager.Instance.Money -= bildingConfig.UnlockPrice;
                 SetModel(_bildingData.UpgradeLevel);
                 SetButton(_bildingData.UpgradeLevel);
                 return;
             }

             if (bildingConfig.IsUpgradeExist(_bildingData.UpgradeLevel + 1))
             {
                 _bildingData.UpgradeLevel++;
                 GameManager.Instance.Money -= GetCost(_bildingData.UpgradeLevel);
                 SetModel(_bildingData.UpgradeLevel);
                 SetButton(_bildingData.UpgradeLevel);
             }
         }

         public void Initialize(BildingData data)
         {
             _bildingData = data;
            if (_bildingData.IsUnlock)
            {
                SetModel(_bildingData.UpgradeLevel); // _bildingData.UpgradeLevel
                SetButton(_bildingData.UpgradeLevel);
            }
            else
            {
                SetButton(-1);
            }
            
            OnManeyChanged(GameManager.Instance.Money);
         }
         
        public BildingData GetData() 
        {
            _bildingData.IsUnlock = true;
            return _bildingData;
        }

        private void SetButton(int level) 
        {
            if (level == -1)
            {
                _bildingButton.UpdateButton(BUY_TEXT, bildingConfig.UnlockPrice);  
            }
            else
            { 
                if(bildingConfig.IsUpgradeExist(_bildingData.UpgradeLevel +1))
                    _bildingButton.UpdateButton(UPGRADE_TEXT, GetCost(level + 1));
                else
                    _bildingButton.gameObject.SetActive(false);
            }
        
            _bildingButton.UpdateButton("Upgrade", bildingConfig.StartUpgradeCost * bildingConfig.CostMultiplayer);
        }

        private async void SetModel(int level) 
        {
            var upgradeConf = bildingConfig.GetUpgrade(level);

            if (_currentModel)
            {
                Addressables.ReleaseInstance(_currentModel);
                //Destroy(_currentModel);
            }
            
            _currentModel = await Addressables.InstantiateAsync(upgradeConf.Model, modelPoint);
            //_currentModel = Instantiate(upgradeConf.Model, modelPoint);
            if(_timerCoroutine == null)
                _timerCoroutine = StartCoroutine(Timer());
        }

        private IEnumerator Timer()
        {
            while (true)
            {
                yield return  new WaitForSeconds(DELAY);
                GameManager.Instance.Money += bildingConfig.GetUpgrade(_bildingData.UpgradeLevel).ProcesseResult;
            }
        }

        private float GetCost(int level) 
        {
           return (float) Math.Round(bildingConfig.StartUpgradeCost * Mathf.Pow(bildingConfig.CostMultiplayer * 0.01f,level), 2);
        }
    }
}