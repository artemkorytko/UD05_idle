using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

//----------------- это игровая панель, висит на GamePanel ---------------------------
namespace DefaultNamespace
{
    public class GamePanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _moneyfield; 
        
        // public Button _resetbutton;
        // public Button _undobutton;
        //private GameManager _gameManagerfile; 

        private float _howmuchmoneyinpanel;

        private void Awake()
        {
            //_gameManagerfile = FindObjectOfType<GameManager>();
        }

        private void Start()
        {
           
        }

        private void ResetClicked()
        {
            // реализовано через юнити onClick
            // var xxx = _gameManagerfile.GetComponent<SaveSystem>();
            // xxx.ResetAllSaved();
        }


        public void SetMoneyOnPanel(float moneyfromGameManager)
        {
            _howmuchmoneyinpanel = moneyfromGameManager;
            _moneyfield.text = $"Money: ${_howmuchmoneyinpanel.ToString()}";
            
            // панель посылает переменную 
        }
    }
}