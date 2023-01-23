﻿using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class BildingButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI costText;
        [SerializeField] private TextMeshProUGUI titleText;

        private Button _button;
        private float _curretCost;

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
            throw new NotImplementedException();
        }

        public void UpdateButton(string text, float cost)
        {
            titleText.text = text;
            _curretCost = cost;
            costText.text = _curretCost.ToString();
        }

        public void SetState(bool isActive)
        {
            _button.interactable = isActive;
        }
    }
}