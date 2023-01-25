using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "UpgradeConfig", menuName = "Configs/UpgradeConfig", order = 0)]
    public class UpgradeConfig : ScriptableObject
    {
        [SerializeField] private AssetReference model;
        [SerializeField] private int processeResult;

        public AssetReference Model => model;

        public int ProcesseResult => processeResult;
    }
}