using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class LookAtCamera : MonoBehaviour
    {
        private void Start()
        {
            var camera = FindObjectOfType<Camera>().transform;
            // var lookDirection = transform.position - camera.position;
            // lookDirection.Normalize();
            // transform.rotation = Quaternion.LookRotation(lookDirection, transform.up);
            
            transform.rotation = Quaternion.LookRotation(camera.forward);
        }
    }
}