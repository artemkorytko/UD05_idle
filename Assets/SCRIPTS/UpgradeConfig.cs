using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "UpgradeComfig", menuName = "Configs/UpgradeComfig", order = 0)]
    public class UpgradeConfig : ScriptableObject
    {
        // без Adressable было:
        // [SerializeField] private GameObject model;
        
        // c Adressable:
        // Подключение: package manager > Unity Registry > Adressables > Install
        // окно: Window > Asset Manager > Groups
        [SerializeField] private AssetReference model;
        
        
        [SerializeField] private int processResult;

        // без Adressable было:
        // public GameObject Model => model;
        public AssetReference Model => model;
        public int ProcessResult => processResult;
        
        
    }
}