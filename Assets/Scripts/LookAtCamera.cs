using UnityEngine;

namespace DefaultNamespace
{
    public class LookAtCamera : MonoBehaviour
    {
        private void Start()
        {
             var camera = FindObjectOfType<Camera>().transform;
            // var lookDirection = (transform.position - camera.position).normalized;
            // // lookDirection.Normalize();
            // transform.rotation = Quaternion.LookRotation(lookDirection, transform.up); //смотреть на камеру
            
            transform.rotation = Quaternion.LookRotation(camera.forward); //смотреть куда смотрит камера
        }
    }
}