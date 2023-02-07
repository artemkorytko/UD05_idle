using System;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class MoneyPanel : MonoBehaviour
    {
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
            
        }

        private void Start()
        {
            GameManager.Instance.OnMoneyChanged += OnMoneyChanged; 
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnMoneyChanged -= OnMoneyChanged;
        }

        private void OnMoneyChanged(float value)
        {
            _text.text = $"Money: {value}";
        } 
    }
    
}