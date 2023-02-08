using System;
using System.Reflection.Emit;
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
            // идет в билдинг
        }
        
        
        // приходит из Building
        public void UpdateButton(string text, float cost, string whatsit)
        {
            // когда обновилась
            titleText.text = text + $" the {whatsit}" ;
            _currentCost = cost;
            costText.text  = "$" + _currentCost.ToString();
        }
        
        
        public void UpdateButtonToMax(float dohod, string whatsit)
        {
            // когда обновилась
            titleText.text =  $"So cool!" ;
            costText.text = $"Your {whatsit} brings {dohod}";
            //SetState(false); // бетонирует

            Zabetonirovat();
        }
        
        

        public void Zabetonirovat()
        {
            _button.interactable = false;
        }

        public void Razbetonirovat()
        {
            _button.interactable = true;
        }

        
        // вызывается зданием, если денег хватает - делает активным
        public void OnMoneyValueChanged(float value) // валуе 
        {
            // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
            // %%%%%%%%%%%%%%%%%%%%%%%% ВОТ ТУТ ПРОВЕРИТЬ %%%%%%%%%%%%%%%%%%%%%%%%%%
            // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
            // меньше равно
            SetState(_currentCost <= value);
            // если цена <= бабла в наличии -- true
            // если цена > бабла в наличии -- false
            
            
        }

        // активна или не активна - если мало бабла - меняет состояние кнопки
        private void SetState(bool isActive)
        {
            // вот тут надо как-то передавать из билдинг, не максимальное ли оно????
            _button.interactable = isActive;
            //------ вот тут хрень выходит ----
            // когда максимум разбетонировывает обратно
        }

        //---------------------------------------------------------------------------
        private void OnDestroy() //отписываемся 
         {
           _button.onClick.RemoveListener(OnClick);
         }
    }
}