using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "UpgradeConfig", menuName = "Config/UpgradeConfig", order = 0)]
    public class UpgradeConfig : ScriptableObject
    {
        [SerializeField] private AssetReference model;
        [SerializeField] private int processResualt;

        public AssetReference Model => model;

        public int ProcessResualt => processResualt;
    }
}