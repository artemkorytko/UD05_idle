using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "BuildingConfig", menuName = "Configs/BuildingConfig", order = 0)]
    public class BuildingConfig : ScriptableObject
    {
        [SerializeField] private float unlockPrice;
        [SerializeField] private float startUpgradeCost;
        [SerializeField] private float costMultiplier;
        [SerializeField] private UpgradeConfig[] upgrades;

        public float UnlockPrice => unlockPrice;

        public float StartUpgradeCost => startUpgradeCost;

        public float CostMultiplier => costMultiplier;

        public UpgradeConfig GetUpgrade(int index)
        {
            if (index >= upgrades.Length) return null;
            
            return upgrades[index];
        }

        public bool IsUpgradeExist(int index)
        {
            return index >= 0 && index <= upgrades.Length;
        }
    }
}