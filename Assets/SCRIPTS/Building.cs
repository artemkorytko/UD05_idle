using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;

// каждое здание в вакууме, котнтролирует свои кнопки и прогресс

namespace DefaultNamespace
{
    public class Building : MonoBehaviour
    {
        private const string BUY_TEXT = "BUY";
        private const string UPGRADE_TEXT = "UPGRADE";
        
        [SerializeField] private BuildingConfig config;
        [SerializeField] private Transform modelPoint;

        // внимание билдинг баттон!
        private BuldingButton _button;
        //private Button _button;
        
        private GameObject _currentModel;
        private string _whatsit = null;

        // ссылка на дату ? ://
        private BuildingsData _data;

        //-----------------------------------------------------------------------------------------------------
        private void Awake()
        {
            // найти кнопку
            _button = GetComponentInChildren<BuldingButton>();
        }


        private void Start()
        {
            _button.OnClickEvent += OnButtonClick;
        }

        //-----------------------------------------------------------------------------------------------------

        private void OnButtonClick()
        {
            //----------------- тут Д/з -------------------------
            // работаете с кнопочкой, меняете состояние - у нее есть метод который делает неактивной
        }



        public void Initialize(BuildingsData data)
        {
            // присваиваем на старте
            _data = data;

            //---------------------------------- !! вернуть потом ---------------------------------------------
            // модель устанавливается, если 
            if (_data.IsUnlocked) 
            {
                SetModel(_data.UpgradeLevel); 
                SetButton(_data.UpgradeLevel);
            }
            else
            {
                SetButton( -1);
            }
            //----- а эти две выпилить:
            // SetModel(0); // было просто без if 
            // SetButton(1);
        }


        public BuildingsData GetData()
        {
            //======================= дебажный анлок =======================
            // _data.IsUnlocked = true;
            //==============================================================
            return _data; //возвращаем
        }



        private void SetButton(int level)
                {  
                    _whatsit = config.BuildingName;
                    
                    // вернуть когда починю json ------ if (level == 0)
                    // почему -1 ????????????????????
                    if (level == 0)
                    {
                        // уходит в кнопку:
                        _button.UpdateButton(BUY_TEXT, config.UnlockPrice, _whatsit);
                    }
                    else
                    { 
                        //_button.UpdateButton(UPGRADE_TEXT, GetCost(level), _whatsit);
                        _button.UpdateButton(UPGRADE_TEXT, config.StartUpgradeCost * config.CostMultiplier, _whatsit);
                    }
                    
                    // от Вовы:
                    // но оно сюда придет вообще когда-нибудь разве?
                    // _button.UpdateButton(UPGRADE_TEXT, config.StartUpgradeCost * config.CostMultiplier, _whatsit);
                }
         
        //-------------------- подгружает модельку зданию -------------------------------
        // async потому что внутри юнитаска
        private async void SetModel(int level)
        {
            var upgradeConfig = config.GetUpgrade(level);
            
            // если там уже есть здание то удалить его
            if (_currentModel)
            {
                // без Adressable было:
                // Destroy(_currentModel);
                
                // теперь чтобы Adressabl-ы знали что память можно освободить, пишем (не асинхронное)
                Addressables.ReleaseInstance(_currentModel);
            }

            // без Adressable было:
            // _currentModel = Instantiate(upgradeConfig.Model, modelPoint);
            
            // асинхронно, потому что объект не лежит в памяти и надо подгружать, если большое не справимся в 1 кадр
            // надо дождаться, пока выполнится асинхронная операция, АК рекомендует таски - поэтому "await"
            _currentModel = await Addressables.InstantiateAsync(upgradeConfig.Model, modelPoint);
            
            // моё: принудительно ставлю на место
            _currentModel.transform.position = modelPoint.transform.position;
            
            
            //===========! если надо грузить новые объекты, пока юзер тупит в сплеш "нажмите следующее" !=============
            // var obj = await Addressables.LoadAssetAsync<GameObject>(upgradeConfig.Model); 
            // указать тип: грузим объекты, текст или музыку
            
            // await NextButtonClick;
            // + ***
            
            // ИЛИ грузить по лейблу в адрессэйблах - см. docs.unity3d.com - Loading by label
            // вернется лист объектов и с ним работаем
            //=========================================================================================================

        }
        
        //======== к примеру про частичную подгрузку ==============================================================
        // ***
        // private async UniTask NextButtonClick()
        // {      await new UniTaskCompletionSource().Task;    }
        //=========================================================================================================

        private float GetCost(int level)
        {
            return (float) System.Math.Round(config.StartUpgradeCost * Mathf.Pow(config.CostMultiplier, level), 2);
        }


        private void OnDestroy()
        {
            _button.OnClickEvent -= OnButtonClick;
        }
    }
    
    
}