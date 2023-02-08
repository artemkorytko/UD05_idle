using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

//----------------- это игровая панель, висит на GamePanel ---------------------------
// кнопка Reset - на нее навешена ResetALL из GM в инспекторе

namespace DefaultNamespace
{
    public class GamePanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _moneyfield; 
        //private GameManager _gameManagerfile; 
        

        //---------------------------------------------------------------------
        private void Awake()
        {
             GameManager.Instance.OnMoneyChanged += SetMoneyOnPanel;
            // паттерн Singleton локальная версия
        }

        private void Start()
        {
            GameManager.Instance.OnMoneyChanged += SetMoneyOnPanel;
        }
        
        
        public void SetMoneyOnPanel(float value)
        {
            // вызывается GM если здание поменяло в нем переменную
            _moneyfield.text = $"Money: ${value}";
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnMoneyChanged -= SetMoneyOnPanel;
        }

    }
}