using System;

// класс хранит инфу про одно отдельное здание
namespace DefaultNamespace
{
    [Serializable]

    public class BuildingsData
    {
        // хранит инфу об одном заднии
        // на инкапсуляцию забили из-за сериализации

        // два поля в классе:
        public bool IsUnlocked;
        public int UpgradeLevel;

        // 2 кастомных конструктора, которые инициализируют эти поля
        // ???????????? ???????????
        public BuildingsData()
        {
            IsUnlocked = false;
            UpgradeLevel = 0;
        }
        
        //кастомный конструктор, ему передаем значения для полей
        public BuildingsData(bool isUnlocked, int level)
        {
            IsUnlocked = isUnlocked;
            UpgradeLevel = level;
        }
    }
}