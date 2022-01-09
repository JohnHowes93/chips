using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    private Vector3 playRotation = new Vector3(29.1f, 0f, 0f);
    private Vector3 playPosition = new Vector3(0f, 4.23f, -8.5f);

    private Vector3 topDownPosition = new Vector3(0f, 30f, 0f);
    private Vector3 topDownRotation = new Vector3(90f, 0f, 0f);
    public static int cameraMode;

    private float cameraSpeed, v, h;
    private Vector3 golfCamOffset = new Vector3(0f, 10f, -0f);
    public float cameraDistanceFromChip;

    // Start is called before the first frame update
    void Start()
    {
        References.cameraMovement = this;
        References.mainCamera = this.gameObject;
        cameraSpeed = 30f;
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraMode == 0)
        {
            Camera0();
        }
        else if (cameraMode == 1)
        {
            Camera1();
        }
        else if (cameraMode == 2)
        {
            Camera2();
        }
        else if (cameraMode == 3)
        {
            Camera3();
        }

    }

    public void Camera0()
    {

    }
    public void Camera1()
    {
        // top down view
        transform.rotation = Quaternion.Euler(topDownRotation);
        transform.position = topDownPosition;
    }
    public void Camera2()
    {
        // golf mode
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

    public void Camera3()
    {
        transform.LookAt(Vector3.zero, Vector3.up);
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
        float x = Random.Range(-15, 15);
        float y = Random.Range(20, 30);
        float z = Random.Range(-15, 15);
        transform.position = new Vector3(x, y, z);
    }

}
