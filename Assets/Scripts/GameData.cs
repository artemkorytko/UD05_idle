namespace DefaultNamespace
{
    public class GameData
    {
        public const int BildCount = 5;
        
        public float Money = 60f;
        public BildingData[] BildingsData;

        public GameData()
        {
            BildingsData = new BildingData[BildCount];
            for (int i = 0; i < BildCount; i++)
            {
                BildingsData[i] = new BildingData();
            }
        }
    }
}