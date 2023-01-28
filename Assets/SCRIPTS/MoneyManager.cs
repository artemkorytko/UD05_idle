using System;
using UnityEngine;

//--------------- повесила на GameManager но пока тут пустенько---------------
namespace DefaultNamespace
{
    public class MoneyManager : MonoBehaviour
    {
        private float _mainMoneyCounter;

        public float MoneyInMM => _mainMoneyCounter;
        private void Awake()
        {
            
        }
        
        // первоначально получает флоат денег из манагера и добавляет к текущему 
        public void AddMoney(float howmuchAdd)
        {
            _mainMoneyCounter += howmuchAdd;
            Debug.Log($"Прибавилось баблишка, стало:{_mainMoneyCounter}");
        }
        
        
    }
}