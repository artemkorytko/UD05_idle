using System;

namespace DefaultNamespace
{
    [Serializable]
    public class GameData
    {
        public const int BuildCount = 4;
        
        public float Money = 60f;
        public BuildingData[] BuildingsData;
        
        public GameData(int money)
        {
            Money = money;
            BuildingsData = new BuildingData[BuildCount];

            for (int i = 0; i < BuildCount; i++)
            {
                BuildingsData[i] = new BuildingData();
            }
        }
    }
}