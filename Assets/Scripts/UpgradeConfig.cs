using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "UpgradeConfig", menuName = "Configs/UpgradeConfig", order = 0)]
    public class UpgradeConfig : ScriptableObject
    {
        [SerializeField] private GameObject model;
        [SerializeField] private int processeResult;

        public GameObject Model => model;

        public int ProcesseResult => processeResult;
    }
}