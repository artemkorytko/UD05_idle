using System;
using Unity.VisualScripting;

namespace DefaultNamespace
{
    [Serializable]
    public class BildingData
    {
        public bool IsUnlock;
        public int UpgradeLevel;

        public BildingData()
        {
            IsUnlock = false;
            UpgradeLevel = 0;
        }

        public BildingData(bool isUnlock, int level)
        {
            IsUnlock = isUnlock;
            UpgradeLevel = level;
        }
    }
}