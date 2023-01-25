using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class Building : MonoBehaviour
    {
        private const string BUY_TEXT = "BUY";
        private const string UPGRADE_TEXT = "UPGRADE";
        
        [SerializeField] private BuildingConfig config;
        [SerializeField] private Transform modelPoint;

        // внимание билдинг баттон!
        private BuldingButton _button;
        //private Button _button;
        
        private GameObject _currentModel;
        private string whatsit = null;
        

        private void Start()
        {    
            _button = GetComponentInChildren<BuldingButton>();
             SetModel(0);
             SetButton(1);

            
        }

         private void SetButton(int level)
                {  
                    whatsit = config.BuildingName;
                    
                    if (level == 0)
                    {
                        // уходит в кнопку:
                        _button.UpdateButton(BUY_TEXT, config.UnlockPrice, whatsit);
                    }
                    else
                    {
                        _button.UpdateButton(UPGRADE_TEXT, GetCost(level), whatsit);
                    }
                }
         
        private void SetModel(int level)
        {
            var upgradeConfig = config.GetUpgrade(level);
            
            // если там уже есть здание то удалить его
            if (_currentModel)
            {
                Destroy(_currentModel);
            }

            
            _currentModel = Instantiate(upgradeConfig.Model, modelPoint);
            _currentModel.transform.position = modelPoint.transform.position;
        }
        
       

        private float GetCost(int level)
        {
            return (float) System.Math.Round(config.StartUpgradeCost * Mathf.Pow(config.CostMultiplier, level), 2);
        }
        
        
        
    }
    
    
}