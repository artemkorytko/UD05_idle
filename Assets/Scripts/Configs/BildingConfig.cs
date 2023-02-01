using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "BildingConfig", menuName = "Configs/BildingConfig", order = 0)]
    public class BildingConfig : ScriptableObject
    {
        [SerializeField] private float unlockPrice;
        [SerializeField] private float startUpgradeCost;
        [SerializeField] private float costMultiplayer;
        [SerializeField] private UpgradeConfig[] upgrade;

        public float UnlockPrice => unlockPrice;
        public float StartUpgradeCost => startUpgradeCost;
        public float CostMultiplayer => costMultiplayer;

        
        public UpgradeConfig GetUpgrade(int index)
        {
            if(index >= upgrade.Length) return null;
            
            return upgrade[index];
        }

        public bool IsUpgradeExist(int index)
        {
            return index >= 0 && index <= upgrade.Length;
        }
    }
}