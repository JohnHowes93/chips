using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    private Vector3 playRotation = new Vector3(29.1f, 0f, 0f);
    private Vector3 playPosition = new Vector3(0f, 4.23f, -8.5f);

    private Vector3 shootPosition = new Vector3(0f, 17f, 0f);
    private Vector3 shootRotation = new Vector3(90f, 0f, 0f);

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Camera1()
    {
        transform.rotation = Quaternion.Euler(playRotation);
        transform.position = playPosition;
    }
    public void Camera2()
    {
        transform.rotation = Quaternion.Euler(shootRotation);
        transform.position = shootPosition;
    }

}
