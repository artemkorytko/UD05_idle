using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private void Start()
    {
        var camera = FindObjectOfType<Camera>().transform;
        var lookDirection = (transform.position - camera.position).normalized;
        transform.rotation = Quaternion.LookRotation(lookDirection, transform.up);
    }
}
