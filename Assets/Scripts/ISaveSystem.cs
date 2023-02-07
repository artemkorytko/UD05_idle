using Cysharp.Threading.Tasks;

namespace DefaultNamespace
{
    public interface ISaveSystem
    {
        public GameData GameData { get; }
        
        public UniTask Initialize(int defaultValue);
        public void SaveData();
        public UniTask<bool> LoadData();
        
        
    }
}