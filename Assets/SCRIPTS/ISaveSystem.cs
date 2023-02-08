using System;
using Cysharp.Threading.Tasks;


namespace DefaultNamespace
{
    public interface ISaveSystem
    {
        // в интерфейсах все публик
        // нельзя объявить переменные, приватный метод
        // нельзя тела {}
        
        // типо контейнера где лежат названия методов - чистейшая инкапсуляция
        
        // можно - МЕТОДЫ, СВойсьтва, ИВЕНТЫ
        // только сигнатуры метода (названия)
        
        
        public GameData GameData { get;  }  // доступ на чтение

        public UniTask Initialize(int defaultValue);
        public void SaveData();

        public void ResetSaved();
        public UniTask<bool> LoadData();
        
        
    }
}
