using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class BuildingButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI costText;
        [SerializeField] private TextMeshProUGUI titleText;

        private Button _button;
        private float _currentCost;

        public event Action OnClickEvent;

        private void Awake()
        {
            _button = GetComponent<Button>();
            
        }

        private void Start()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            OnClickEvent?.Invoke();   
        }

        public void UpdateButton(string text, float cost)
        {
            titleText.text = text;
            _currentCost = cost;
            costText.text = _currentCost.ToString();
        }

        private void SetState(bool isActive)
        {
            _button.interactable = isActive;
        }
        
        public void OnMoneyValueChanged(float value)
        {
            SetState(_currentCost <= value);
        }
    }
}