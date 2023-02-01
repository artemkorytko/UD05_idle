using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class FieldManager : MonoBehaviour
    {
        private Bilding[] _bildings;

        private void Awake()
        {
            _bildings = GetComponentsInChildren<Bilding>();
        }

        public void Initialize(GameData gameData)
        {
            var data = gameData.BildingsData;
            for (int i = 0; i < _bildings.Length; i++)
            {
                _bildings[i].Initialize(data[i]);
            }
        }

        public BildingData[] GetBildingDatas()
        {
            var data = new BildingData[GameData.BildCount];
            for (int i = 0; i < _bildings.Length; i++)
            {
                data[i] = _bildings[i].GetData();
            }

            return data;
        }
        
        
        
        private void Singl() // просто метод как работет СинглТон
        {
            var a = GameManager.Instance.name; // правило двух точек (. .) больше желательно не использовать!! 
        }
    }
}