using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "UpgradeConfig", menuName = "Config/UpgradeConfig", order = 0)]
    public class UpgradeConfig : ScriptableObject
    {
        [SerializeField] private GameObject model;
        [SerializeField] private int processResualt;

        public GameObject Model => model;

        public int ProcessResualt => processResualt;
    }
}