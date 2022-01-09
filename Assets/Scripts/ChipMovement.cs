using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ChipMovement : MonoBehaviour
{
    private Rigidbody chipRb;
    public float thrust;
    public bool isActivePiece;
    private GameObject placeholderChip;
    private GameObject playerChip;
    private int turnPhase;
    private bool inputLocked;
    private bool chipHasLeftTheBoard;
    private ChipData data;
    Color transparentColour;

    const float firingLowRange = 12.8f;
    const float firingHighRange = 14.5f;
    Color playerOneColour;
    Color playerTwoColour;
    private int golfShootPhase;

    private Vector3 chipTarget;
    private float targetPowerLevel;
    private float powerLevel;
    private float targetDirectionLevel;
    private float directionLevel;
    private bool directionIncrease;
    private float powerTimer, directionTimer, waitTime;
    private float directionOffsetAmount = 10;

    // Start is called before the first frame update
    void Start()
    {
        targetDirectionLevel = 50;
        waitTime = 0.01f;
        thrust = 6;
        playerOneColour = Color.blue;
        playerTwoColour = Color.red;
        transparentColour = new Color(1, 1, 1, 0.3f);
        chipHasLeftTheBoard = false;
        UnlockInput();
        turnPhase = 0;
        placeholderChip = transform.Find("PlaceholderChip").gameObject;
        playerChip = transform.Find("PlayerChip").gameObject;
        placeholderChip.SetActive(false);
        playerChip.SetActive(false);
        chipRb = playerChip.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (isActivePiece)
        {
            References.activeChip = playerChip;
            HandlePieceLeavingBoard();
            if (turnPhase == 0)
            {
                PositionChipOnBoard();
            }
            if (turnPhase == 1)
            {
                GolfShoot();
            }
            if (turnPhase == 2)
            {
                WaitForPieceToStopMoving();
            }
            if (turnPhase == 3)
            {
                Advance();
            }
        }
    }

    void Shoot()
    {
        playerChip.SetActive(true);
        if (Input.GetButtonDown("Fire1") && !inputLocked)
        {
            inputLocked = true;
            Ray rayFromCameraToCursor = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane playerPlane = new Plane(Vector3.up, transform.position);
            playerPlane.Raycast(rayFromCameraToCursor, out float distanceFromCamera);
            Vector3 cursorPosition = rayFromCameraToCursor.GetPoint(distanceFromCamera);
            Vector3 distanceToTravel = cursorPosition - transform.position;
            chipRb.AddForce(distanceToTravel * -thrust, ForceMode.Impulse);
            turnPhase++;
        }
    }

    void GolfShoot()
    {
        playerChip.SetActive(true);
        References.cameraMovement.SetCamera(2);
        Vector3 directionOfShot = playerChip.transform.position - References.mainCamera.transform.position;
        directionOfShot.y = 0;
        Vector3 playerPosition3 = playerChip.transform.position;
        playerPosition3.y = 0;
        // Debug.Log("playerPosition3" + playerPosition3);
        Vector3 cameraPosition3 = References.mainCamera.transform.position;
        cameraPosition3.y = 0;
        // Debug.Log("cameraPosition3" + cameraPosition3);
        Vector3 targetDir = playerPosition3 - cameraPosition3;
        float angle = Vector3.Angle(References.mainCamera.transform.forward, Vector3.zero);
        Debug.Log(angle);
        // Debug.Log(playerChip.transform.position);
        Vector3 positionFromCameraToChip = References.mainCamera.transform.position - playerChip.transform.position;
        positionFromCameraToChip.y = 0;
        chipTarget = positionFromCameraToChip;
        Debug.DrawRay(playerChip.transform.position, chipTarget);
        if (golfShootPhase == 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                golfShootPhase++;
            }
        }
        else if (golfShootPhase == 1)
        {
            //set powerlevel
            References.cameraMovement.SetCamera(0);
            HandlePowerMeter();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                powerLevel = targetPowerLevel;
                golfShootPhase++;
            }

        }
        else if (golfShootPhase == 2)
        {
            // set direction
            HandleDirectionMeter();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                directionLevel = targetDirectionLevel - 50;
                Debug.Log(directionLevel);
                References.cameraMovement.ApplyCameraRotationOffset(directionLevel / 10);
                golfShootPhase++;
            }
        }
        else if (golfShootPhase == 3)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // modify thrust based on power level
                float powerMultiplier = powerLevel / 100;
                Debug.Log("powerMultiplier" + powerMultiplier);
                float finalThrust = thrust * powerMultiplier;
                Debug.Log("finalThrust" + finalThrust);

                // add 50 here so that the range is 50 - 150, 100 being in the center
                // float directionErrorFactor = (directionLevel + 50) / 100;
                // Debug.Log("directionErrorFactor" + directionErrorFactor);
                // Vector3 directionCalculated = (Vector3.right * directionErrorFactor);
                // Debug.Log("directionCalculated" + directionCalculated);
                // Vector3 finalDirection = chipTarget + directionCalculated;
                // Debug.Log("finalDirection" + finalDirection);
                // Vector3 positionFromCameraToChip = References.mainCamera.transform.position - playerChip.transform.position;
                // positionFromCameraToChip.y = 0;
                // chipTarget = positionFromCameraToChip;
                // Debug.DrawRay(playerChip.transform.position, chipTarget);
                chipRb.AddForce(chipTarget * -finalThrust, ForceMode.Impulse);

                // modify direction based on direction level
                // Vector3 localAngles = References.mainCamera.transform.localEulerAngles;
                // Vector3 localAnglesClone = new Vector3(localAngles.x, localAngles.y, localAngles.z);
                // localAnglesClone.y = ((localAnglesClone.y / 360) * directionErrorFactor) * 360;


                Debug.Log("eulercamera" + References.mainCamera.transform.localEulerAngles);






                // Debug.Log(finalDirection);
                // Debug.Log(finalDirection);

            }
        }
        if (Input.GetButtonDown("Fire1"))
        {
            // inputLocked = true;
            // Ray rayFromCameraToCursor = Camera.main.ScreenPointToRay(Input.mousePosition);
            // Plane playerPlane = new Plane(Vector3.up, transform.position);
            // playerPlane.Raycast(rayFromCameraToCursor, out float distanceFromCamera);
            // Vector3 cursorPosition = rayFromCameraToCursor.GetPoint(distanceFromCamera);
            // Vector3 distanceToTravel = cursorPosition - transform.position;
            // chipRb.AddForce(distanceToTravel * -thrust, ForceMode.Impulse);
            // turnPhase++;
        }
    }

    void HandlePowerMeter()
    {
        powerTimer += Time.deltaTime;
        if (powerTimer > waitTime)
        {
            powerTimer = 0;
            if (targetPowerLevel < 100)
            {
                targetPowerLevel++;
            }
            else if (targetPowerLevel == 100)
            {
                targetPowerLevel = 0;
            }
            GameObject.Find("PowerLevelSlider").GetComponent<Slider>().value = targetPowerLevel;
        }
    }

    void HandleDirectionMeter()
    {
        directionTimer += Time.deltaTime;
        if (directionTimer > waitTime)
        {
            directionTimer = 0;

            if (targetDirectionLevel > 99)
            {
                directionIncrease = false;
            }
            else if (targetDirectionLevel <= 0)
            {
                directionIncrease = true;
            }
            if (directionIncrease)
            {
                targetDirectionLevel++;
            }
            else { targetDirectionLevel--; }
            GameObject.Find("DirectionLevelSlider").GetComponent<Slider>().value = targetDirectionLevel;
        }
    }

    void PositionChipOnBoard()
    {
        if (placeholderChip.activeSelf)
        {
            References.cameraMovement.SetCamera(1);
            Ray rayFromCameraToCursor = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane playerPlane = new Plane(Vector3.up, transform.position);
            playerPlane.Raycast(rayFromCameraToCursor, out float distanceFromCamera);
            Vector3 cursorPosition = rayFromCameraToCursor.GetPoint(distanceFromCamera);
            transform.position = cursorPosition;
            float distanceFromCenter = Vector3.Distance(cursorPosition, Vector3.zero);
            if (distanceFromCenter > firingLowRange && distanceFromCenter < firingHighRange)
            {
                if (References.isPlayerOnesTurn)
                {
                    placeholderChip.GetComponentInChildren<Renderer>().material.color = playerOneColour;
                    playerChip.GetComponentInChildren<Renderer>().material.color = playerOneColour;
                }
                else
                {
                    placeholderChip.GetComponentInChildren<Renderer>().material.color = playerTwoColour;
                    playerChip.GetComponentInChildren<Renderer>().material.color = playerTwoColour;
                    // position the camera above the chip
                }

                if (Input.GetButtonDown("Fire1"))
                {
                    References.cameraMovement.PositionCameraAboveChip();
                    placeholderChip.SetActive(false);
                    turnPhase++;
                    golfShootPhase = 0;
                }
                // if (Input.GetButtonUp("Fire1"))
                // {

                // }
            }
            else
            {
                placeholderChip.GetComponentInChildren<Renderer>().material.color = transparentColour;
            }
        }
        else
        {
            placeholderChip.SetActive(true);
        }
    }

    void WaitForPieceToStopMoving()
    {
        if (chipRb.velocity == Vector3.zero)
        {
            turnPhase++;
        }
    }

    IEnumerator LockInputForXSeconds(int seconds)
    {
        inputLocked = true;
        yield return new WaitForSeconds(seconds);
        inputLocked = false;
    }

    private void LockInput()
    {
        inputLocked = true;
    }

    private void UnlockInput()
    {
        inputLocked = false;
    }

    private void HandlePieceLeavingBoard()
    {
        if (playerChip.transform.position.y < -40)
        {
            chipHasLeftTheBoard = true;
        }
        if (chipHasLeftTheBoard)
        {
            // move the chip, then the child to the side of the board
            transform.position = new Vector3(0, 0, References.isPlayerOnesTurn ? References.boardSize : -References.boardSize);
            playerChip.transform.position = transform.position;
            playerChip.transform.rotation = transform.rotation;
        }
    }

    public void SetId(int id)
    {
        data.id = id;
    }

    public void SetActivePiece(bool isActive)
    {
        isActivePiece = isActive;
    }

    public void Advance()
    {
        turnPhase++;
        References.gameManager.AdvanceTurn();
        // isActivePiece = false;
    }


}
