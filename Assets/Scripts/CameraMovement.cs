using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector3 topDownPosition = new Vector3(0f, 30f, 0f);
    private Vector3 topDownRotation = new Vector3(90f, 0f, 0f);
    public static int cameraMode;

    private float cameraSpeed, v, h;
    private Vector3 golfCamOffset = new Vector3(0f, 10f, -0f);
    private float travelSpeed = 2;

    private Vector3 destination;

    void Awake()
    {
        References.cameraMovement = this;
        References.mainCamera = this.gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        cameraSpeed = 30f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (cameraMode)
        {
            case 0:
                Camera0();
                break;
            case 1:
                Camera1();
                break;
            case 2:
                Camera2();
                break;
            case 3:
                Camera3();
                break;
            case 4:
                Camera4();
                break;
        }
        // Debug.Log(cameraMode);
        // if (cameraMode == 0)
        // {
        //     Camera0();
        // }
        // else if (cameraMode == 1)
        // {
        //     Camera1();
        // }
        // else if (cameraMode == 2)
        // {
        //     Camera2();
        // }
        // else if (cameraMode == 3)
        // {
        //     Camera3();
        // }
        // else if (cameraMode == 4)
        // {
        //     Camera4();
        // }

    }

    public void Camera0()
    {

    }
    public void Camera1()     // top down view
    {
        // transform.rotation = Quaternion.Euler(topDownRotation);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(topDownRotation), travelSpeed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, topDownPosition, travelSpeed * Time.deltaTime);
        // transform.position = topDownPosition;
    }

    public void Camera2()    // golf cam
    {
        if (Input.GetKey("left"))
        {
            transform.RotateAround(References.activeChip.transform.position, Vector3.up, cameraSpeed * Time.deltaTime);
        }
        else if (Input.GetKey("right"))
        {
            transform.RotateAround(References.activeChip.transform.position, Vector3.up, -cameraSpeed * Time.deltaTime);
        }
        transform.LookAt(References.activeChip.transform);
    }

    public void Camera3() // move towards destination while looking at it
    {

        transform.position = Vector3.Lerp(transform.position, destination, travelSpeed * Time.deltaTime);
        Vector3 direction = Vector3.zero - transform.position;
        // Quaternion toRotation = Quaternion.LookRotation(transform.forward, direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), (travelSpeed * 1.8f) * Time.deltaTime);
    }

    public void Camera4() // focus chip when scoring
    {
        transform.position = Vector3.Lerp(transform.position, destination, travelSpeed * Time.deltaTime);
        Vector3 direction = transform.position - (destination - Vector3.down * 10);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), travelSpeed * Time.deltaTime);
    }

    public void SetCamera(int mode)
    {
        cameraMode = mode;
        if (mode == 3)
        {
            PositionCameraRandomlyAroundBoard();
        }
    }

    public void PositionCameraAboveChip()
    {
        Vector3 positionFromCenterToChip = Vector3.Lerp(Vector3.zero, References.activeChip.transform.position, 1);
        transform.position = (positionFromCenterToChip * 2) + golfCamOffset;
    }

    public void ApplyCameraRotationOffset(float angle)
    {
        transform.RotateAround(References.activeChip.transform.position, Vector3.up, angle);
    }

    private void PositionCameraRandomlyAroundBoard()
    {
        // x - 15 15, y 20 30 z - 15 15
        float x = Random.Range(-25, 25);
        float y = Random.Range(25, 35);
        float z = Random.Range(-25, 25);
        destination = new Vector3(x, y, z);
    }

    public void SetDestination(Vector3 newPosition)
    {
        destination = newPosition;
    }
}
