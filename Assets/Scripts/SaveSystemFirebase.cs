using Cysharp.Threading.Tasks;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

namespace DefaultNamespace
{
    public class SaveSystemFirebase : ISaveSystem
    {
        private GameData _gameData;
        public GameData GameData => _gameData;

        private DatabaseReference _reference;

        public async UniTask Initialize(int value)
        {
            _reference = FirebaseDatabase.DefaultInstance.RootReference;
            if (!await LoadData())
            {
                _gameData = new GameData(value);
            }
        }

        public void SaveData()
        {
            string json = JsonUtility.ToJson(_gameData);
            _reference.Child("users").Child(SystemInfo.deviceUniqueIdentifier).SetRawJsonValueAsync(json);
        }

        public async UniTask<bool> LoadData()
        {
            await FirebaseDatabase.DefaultInstance.GetReference($"users/{SystemInfo.deviceUniqueIdentifier}")
                .GetValueAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.IsFaulted)
                    {
                    }
                    else if (task.IsCompleted)
                    {
                        DataSnapshot snapshot = task.Result;
                        _gameData = JsonUtility.FromJson<GameData>(snapshot.GetRawJsonValue());
                    }
                });
            return _gameData != null;
        }
    }
}