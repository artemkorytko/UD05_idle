using System;
using System.Collections;
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
        private const float DELAY = 1f;

        [SerializeField] private BuildingConfig config;
        [SerializeField] private Transform modelPoint;

        // внимание: билдинг баттон!
        public BuldingButton _button;

        private GameObject _currentModel;
        private string _whatsit = null;

        private BuildingsData _data;

        private bool coroutineWasSet = false; // флаг для перезапуска корутин по ресету

        private Coroutine timerCor;

        private float _howMuchEarn;

        //-----------------------------------------------------------------------------------------------------
        private void Awake()
        {
            // найти кнопку
            _button = GetComponentInChildren<BuldingButton>();
            config.MaxCool += SayMaxCool;
        }


        private void Start()
        {
            _button.OnClickEvent += OnButtonClick;
            GameManager.Instance.OnMoneyChanged += OnMoneyChanged;
        }

        //-----------------------------------------------------------------------------------------------------

        // Нажатие на кнопку зданий
        private void OnButtonClick()
        {
            if (!_data.IsUnlocked) //если здание еще не купили
            {
                RequestSnapshot(); // мое
                _data.IsUnlocked = true;
                GameManager.Instance.Money -= config.UnlockPrice;
                SetButton(_data.UpgradeLevel);
                SetModel(_data.UpgradeLevel);
                return;
            }

            if (config.DoesUgradeExist(_data.UpgradeLevel + 1))
            {
                RequestSnapshot(); // мое


                _data.UpgradeLevel++; // !!!!!!!! ПОДНИМАЕМ ЛЕВЕЛ


                // мы опять лезем в переменную денег в GM и меняем ее
                GameManager.Instance.Money -= GetCost(_data.UpgradeLevel);
                SetModel(_data.UpgradeLevel);
                SetButton(_data.UpgradeLevel);
            }


            /*
            //----------------- тут Д/з -------------------------
            // работаете с кнопочкой, меняете состояние - у нее есть метод который делает неактивной
            if (_data.IsUnlocked) // если разлочено и здание стоит
            {
                // меньше равно штуков в массиве upgrades в билдинг конфиг!
                // BuildingConfig xxx = new BuildingConfig();
                if (_data.UpgradeLevel <= config.upgrades.Length)
                {
                    _data.Kozel = "2";
                    RequestSnapshot(); //-----NEW пихаем в стек, который в GameManager
                    _data.UpgradeLevel++;
                    _data.Kozel = "22";
                    Initialize(_data);
                }
            }
            else // если НЕ разлочено, то разлочить и загрузить
            {
                _data.Kozel = "1";
                RequestSnapshot(); //-----NEW пихаем в стек, который в GameManager
                _data.IsUnlocked = true;
                _data.Kozel = "11";
                Debug.Log($" Нажалось, isUnlocked = {_data.IsUnlocked}");
                _gameManagerFile.KozelDebug();
                Initialize(_data);
            }

            _gameManagerFile.KozelDebug();
            */
        }

        private void RequestSnapshot() //-----NEW идет пихать в стек, который в GameManager
        {
            GameManager.Instance.Snapshot();
        }


        // --------- это из филд менеджера на старте геймменеджера -----
        public void Initialize(BuildingsData data)
        {
            // присваиваем на старте
            _data = data;

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

                ////----- УДАЛЯЕТ ЗДАНИЯ ПРИ РЕСЕТЕ -----
                if (_currentModel)
                    Addressables.ReleaseInstance(_currentModel); // удалить здание

                //_button.Razbetonirovat(); // разбетонировать кнопку, если была забетонирована
            }

            // !!!!!!!!
            OnMoneyChanged(GameManager.Instance.Money);
        }


        //------------------------ для филд менеджера: берет данные с этого здания ---------------------------
        public BuildingsData GetData()
        {
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
                if (config.DoesUgradeExist(_data.UpgradeLevel + 1))
                    //if (level >= config.upgrades.Length - 1) // ЫЫЫЫЫЫ!!!!
                {
                    // _button.Zabetonirovat();
                    _button.UpdateButton(UPGRADE_TEXT, GetCost(level + 1), _whatsit);
                }
                
                else
                {
                    SayMaxCool();
                    _button.Zabetonirovat();
                }
            }
        }


        private void SayMaxCool()
        {
            int levelhere = _data.UpgradeLevel;
            //----- если здание уже круче некуда 
            Debug.Log(" Здание круче некуда");
            float max = config.upgrades[levelhere].ProcessResult;
            _button.UpdateButtonToMax(max, _whatsit);
        }


        //-------------------- подгружает модельку зданию -------------------------------
        // async потому что внутри юнитаска
        private async void SetModel(int level)
        {
            var upgradeConfig = config.GetUpgrade(level); // модельку и кол-во денег, которые приносит

            // если там уже есть здание то удалить его ПОЛЮБОМУ
            if (_currentModel)
            {
                // без Adressable было:  Destroy(_currentModel);

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

            // высчитывает, сколько денег приносит здание на этом уровне
            HowMuchMoneyDoIEarn();


            // если стоит здание - то зарабатываем деньги, пока не задестроим
            //================= ТАЙМЕР  ========================== ТАЙМЕР ==========================

            if (!coroutineWasSet) // если еще не заупскали корутину (ваще только вошли)
            {
                if (timerCor == null)
                    timerCor = StartCoroutine(Timer());
                coroutineWasSet = true;
            }
            else // это после ресета
            {
                StartCoroutine(Timer());
                Debug.Log("После ресета вошло в старт корутину");
            }

            // или так:
            // if (_timerCor != null)
            //     StopCoroutine(_timerCor);


            //===========! если надо грузить новые объекты, пока юзер тупит в сплеш "нажмите следующее" !=============
            // var obj = await Addressables.LoadAssetAsync<GameObject>(upgradeConfig.Model); 
            // указать тип: грузим объекты, текст или музыку

            // await NextButtonClick;
            // + ***

            // ИЛИ грузить по лейблу в адрессэйблах - см. docs.unity3d.com - Loading by label
            // вернется лист объектов и с ним работаем
            //=========================================================================================================
        }

        public void StopThisTimer()
        {
            if (timerCor != null)
            {
                StopCoroutine(timerCor);
            }
            else
            {
                Debug.Log($"{_whatsit} можно было не останавливать");
            }
        }


        private void HowMuchMoneyDoIEarn()
        {
            // считает сколько приносит здание и пишет в местную переменную
            _howMuchEarn = config.GetUpgrade(_data.UpgradeLevel).ProcessResult;
        }


        // корутина живая пока жив объект
        private IEnumerator Timer()
        {
            while (true)
            {
                yield return new WaitForSeconds(DELAY);
                // сколько бабла за апдейт
                // это мы меняем переменную в чужом файле О_о
                // к деньгам менеджера прибавляем денег столько,
                // сколько получит функция из главного конфига,проверив в массиве конфиг текущего левела

                // GameManager.Instance.Money += config.GetUpgrade(_data.UpgradeLevel).ProcessResult;
                // ????? получается каждое здание каждую секунду дергает конфиг передавая туда свой левел? :/

                // переписала:
                GameManager.Instance.Money += _howMuchEarn;
            }
        }
        //======== к примеру про частичную подгрузку ==============================================================
        // ***
        // private async UniTask NextButtonClick()
        // {      await new UniTaskCompletionSource().Task;    }
        //=========================================================================================================


        // это....
        private float GetCost(int level)
        {
            return (float)System.Math.Round(config.StartUpgradeCost * Mathf.Pow(config.CostMultiplier, level), 2);
        }


        private void OnMoneyChanged(float value)
        {
            // посылать событие в кнопку, только если есть уровни впереди
            // config.upgrades.Length = 3, поэтому -1 блин
            if (_data.UpgradeLevel <= config.upgrades.Length - 1)
            {
                // идет в кнопку и там проверяется не сделать ли активной кнопку
                _button.OnMoneyValueChanged(value);
            }
        }


        private void OnDestroy()
        {
            _button.OnClickEvent -= OnButtonClick;
            GameManager.Instance.OnMoneyChanged -= OnMoneyChanged;
            config.MaxCool -= SayMaxCool;
        }
    }
}