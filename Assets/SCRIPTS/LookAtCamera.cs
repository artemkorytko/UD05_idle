using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//-------------- висит на canvas (world space!) в префабе билда (не в префабе канваса)!
public class LookAtCamera : MonoBehaviour
{
    private void Start()
    {
        //----------1-----------------------
        //------- если уверены что камера мэйн - тег включен, иначе налл
        //------- var camera = Camera.main;
        // var camera = FindObjectOfType<Camera>().transform;

        //------- рассчитали вектор куда смотрит 
        // var lookDirection = transform.position - camera.position;
        
        //------- нужен вектор с единичным значением - привести его магнитуду к 1
        // lookDirection.Normalize();
        //------- или можно сразу
        //------- var lookDirection = (transform.position - camera.position).normalized;
        
        //------- up - повернуть вокруг оси
        // transform.rotation = Quaternion.LookRotation(lookDirection,transform.up);
        
        //----------2-----------------------
        // 2й способ - СМОТРЕТЬ КУДА СМОТРИТ КАМЕРА
         var camera = FindObjectOfType<Camera>().transform;
         transform.rotation = Quaternion.LookRotation(camera.forward);

    }
}
