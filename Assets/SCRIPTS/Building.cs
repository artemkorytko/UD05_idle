using System;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using TMPro;
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

        // внимание: билдинг баттон!
        private BuldingButton _button;

        private GameObject _currentModel;
        private string _whatsit = null;
        
        private BuildingsData _data;

        private GameManager _gameManagerFile;

        //-----------------------------------------------------------------------------------------------------
        private void Awake()
        {
            // найти кнопку
            _button = GetComponentInChildren<BuldingButton>();
            _gameManagerFile = FindObjectOfType<GameManager>();
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
            if (_data.IsUnlocked) // если разлочено и здание стоит
            {
                // меньше равно штуков в массиве upgrades в билдинг конфиг!
                // BuildingConfig xxx = new BuildingConfig();
                if (_data.UpgradeLevel <= config.upgrades.Length)
                {
                    RequestSnapshot(); //-----NEW пихаем в стек, который в GameManager
                    _data.UpgradeLevel++;
                    Initialize(_data);
                    
                }
            }
            else // если НЕ разлочено, то разлочить и загрузить
            {
                RequestSnapshot(); //-----NEW пихаем в стек, который в GameManager
                _data.IsUnlocked = true;
                Debug.Log($" Нажалось, isUnlocked = {_data.IsUnlocked}");
                Initialize(_data);
                
            }
        }

        private void RequestSnapshot() //-----NEW идет пихать в стек, который в GameManager
        {
            _gameManagerFile.Snapshot();
        }


        public void Initialize(BuildingsData data)
        {
            // присваиваем на старте
            //////!!!!!!
            _data = data;

            //---------------------------------- !! вернуть потом ---------------------------------------------
            // модель устанавливается, если 
            if (_data.IsUnlocked)
            {
                SetModel(_data.UpgradeLevel);
                SetButton(_data.UpgradeLevel);
            }
            else // очистить территорию и подготовить к новой продаже
            {
                //SetModel(-1);
                SetButton(-1);
                if (_currentModel)
                    Addressables.ReleaseInstance(_currentModel); // удалить здание
                _button.Razbetonirovat(); // разбетонировать кнопку, если была забетонирована
            }
            //----- а эти две выпилить:
            // SetModel(0); // было просто без if 
            // SetButton(1);
        }


        //------------------------ для филд менеджера: берет данные с этого здания ---------------------------
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

            // без JSON было: (level == 0)
            // ыыыы почему -1 ????????????????????
            if (level == -1)
            {
                // уходит в кнопку:
                _button.UpdateButton(BUY_TEXT, config.UnlockPrice, _whatsit);
            }
            else
            {
                if (level >= config.upgrades.Length - 1) // ЫЫЫЫЫЫ!!!!
                {
                    // _button.Upinteractable = false;
                    _button.Zabetonirovat();
                }

                //_button.UpdateButton(UPGRADE_TEXT, GetCost(level), _whatsit);
                // от Вовы:
                _button.UpdateButton(UPGRADE_TEXT, config.StartUpgradeCost * config.CostMultiplier * (level + 1),
                    _whatsit);
            }
            
        }

        //-------------------- подгружает модельку зданию -------------------------------
        // async потому что внутри юнитаска
        private async void SetModel(int level)
        {
            var upgradeConfig = config.GetUpgrade(level); // модельку и кол-во денег, которые приносит

            // если там уже есть здание то удалить его ПОЛЮБОМУ
            if (_currentModel)
            {
                // без Adressable было:
                // Destroy(_currentModel);

                // теперь чтобы Adressabl-ы знали что память можно освободить, пишем (не асинхронное)
                // УДАЛЕНИЕ МОДЕЛИ:
                Addressables.ReleaseInstance(_currentModel);
            }

            Debug.Log($" Модель заменили, {_whatsit} level {level}");
            // без Adressable было:
            // _currentModel = Instantiate(upgradeConfig.Model, modelPoint);

            // асинхронно, потому что объект не лежит в памяти и надо подгружать, если большое не справимся в 1 кадр
            // надо дождаться, пока выполнится асинхронная операция, АК рекомендует таски - поэтому "await"
            
            
            // ?????????????-------------- где-то тут не грузит новые модели в undo  ---------------????????????????
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

        
        // это вроде бы не используется (ибо хз что в нем происходит)
        private float GetCost(int level)
        {
            return (float)System.Math.Round(config.StartUpgradeCost * Mathf.Pow(config.CostMultiplier, level), 2);
        }


        private void OnDestroy()
        {
            _button.OnClickEvent -= OnButtonClick;
        }
    }
}