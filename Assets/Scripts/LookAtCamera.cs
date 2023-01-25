using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    void Start()
    {
        var camera = FindObjectOfType<Camera>().transform;
        //var lookDiretion = (transform.position - camera.position).normalized;
       // transform.rotation = Quaternion.LookRotation(lookDiretion, transform.up);
        
        transform.rotation = Quaternion.LookRotation(camera.forward);
    }
    
}
