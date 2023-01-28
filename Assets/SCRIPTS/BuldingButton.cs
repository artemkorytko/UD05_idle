using System;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//------------------------- на кнопке ------------------------------------------

namespace DefaultNamespace
{
    public class BuldingButton : MonoBehaviour
    {
        // ссылки на значения
        [SerializeField] private TextMeshProUGUI costText;
        [SerializeField] private TextMeshProUGUI titleText;
        
        // и на кнопку
        private Button _button;
        private float _currentCost;

        public event Action OnClickEvent; 
        //---------------------------------------------------------------------------
        
        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
           _button.onClick.AddListener(OnClick);
        }


        // что будет по клинку
        private void OnClick()
        {
            OnClickEvent?.Invoke();
        }
        
        
        // приходит из Building
        public void UpdateButton(string text, float cost, string whatsit)
        {
            //------------ не входит! -----------------
            Debug.Log(" Вошел в апдейт кнопки");
            // когда обновилась
            titleText.text = text + $" the {whatsit}" ;
            _currentCost = cost;
            costText.text  = _currentCost.ToString() ;
        }
        
         // активна или не активна - если мало бабла - меняет состояние кнопки
        public void SetState(bool isActive)
        {
            _button.interactable = isActive;
        }
        
        //---------------------------------------------------------------------------
        private void OnDestroy() //отписываемся 
         {
           _button.onClick.RemoveListener(OnClick);
         }
    }
}