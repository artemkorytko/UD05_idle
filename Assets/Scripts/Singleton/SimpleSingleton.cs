using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class SimpleSingleton : MonoBehaviour
    {
        public  static SimpleSingleton Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                DestroyImmediate(gameObject);
                return;
            }

            var df = gameObject.name;
        }

        private void Start()
        {
            if(Instance != this)
                return;
            
            var df = gameObject.transform;
        }

        private void OnDestroy()
        {
            if(Instance != this)  // для того чтоб логика в этом методе не отрабатывала т.к. объект живет один кадр(все юнити методы)
                return;
            
            var df = gameObject.transform;
        }
    }
}