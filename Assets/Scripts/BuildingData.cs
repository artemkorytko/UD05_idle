using System;

namespace DefaultNamespace
{
    [Serializable]
    public class BuildingData
    {
        public bool IsUnlock;
        public int UpgradeLevel;

        public BuildingData()
        {
            IsUnlock = false;
            UpgradeLevel = 0;
        }

        public BuildingData(bool isUnlock, int level)
        {
            IsUnlock = isUnlock;
            UpgradeLevel = level;
        }
    }
}