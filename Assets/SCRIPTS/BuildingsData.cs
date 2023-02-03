using System;
using System.Security.Cryptography.X509Certificates;

// класс хранит инфу про одно отдельное здание
namespace DefaultNamespace
{
    [Serializable]
    
    public class BuildingsData
    {
        // хранит инфу об одном заднии
        // на инкапсуляцию забили из-за сериализации

        // два поля в классе и дебажный козёл:
        public bool IsUnlocked;
        public int UpgradeLevel;
        public String Kozel;

        // 2 кастомных конструктора, которые инициализируют эти поля
        // эта фигня по умолчанию и команде new 
        public BuildingsData()
        {
            IsUnlocked = false;
            UpgradeLevel = 0;
        }

        // а это записывает если передали снаружи
        // кастомный конструктор, ему передаем значения для полей
        public BuildingsData(bool isUnlocked, int level)
        {
            IsUnlocked = isUnlocked;
            UpgradeLevel = level;
        }
    }
}