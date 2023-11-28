using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    public Camera mainCamera;
    public Transform target;
    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        transform.rotation = mainCamera.transform.rotation; 
        transform.position = target.position + new Vector3(0, 1, 0);
    }
}
