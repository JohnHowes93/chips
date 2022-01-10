using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    [SerializeField] float cameraSpeed;

    void Start()
    {
        cameraSpeed = 50;
    }
    void Update()
    {
        transform.RotateAround(Vector3.zero + (Vector3.up * 5), Vector3.up, cameraSpeed * Time.deltaTime);
        transform.LookAt(Vector3.zero, Vector3.up);
    }
}
