using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ChipMovement : MonoBehaviour
{
    private Rigidbody chipRb;
    public float thrust, targetPowerLevel, powerLevel, targetDirectionLevel, directionLevel, powerTimer, directionTimer, waitTime, finalThrust;
    public bool isActivePiece, inputLocked, chipHasLeftTheBoard, directionIncrease, hasTakenShot, chipPotted;
    private GameObject playerChip, placeholderChip;
    private int turnPhase, golfShootPhase;
    Color transparentColour;
    private Vector3 chipTarget;
    const float firingLowRange = 12.8f;
    const float firingHighRange = 14.5f;
    const float secondsToSkipTurn = 3;
    float skipTurnTimer;

    // Start is called before the first frame update
    void Awake()
    {
        HandleNewGame();
    }

    void HandleNewGame()
    {
        Debug.Log("handlenewgame");
        hasTakenShot = false;
        skipTurnTimer = 0f;
        targetDirectionLevel = 50;
        waitTime = 0.005f;
        thrust = 1.9f;
        References.playerOneColour = Color.blue;
        References.playerTwoColour = Color.red;
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
                References.audioManager.PlayTurnIndicator();
                turnPhase++;
            }
            if (turnPhase == 1)
            {
                PositionChipOnBoard();
                return;
            }
            if (turnPhase == 2)
            {
                GolfShoot();
                return;
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

        if (golfShootPhase == 0)
        {
            References.firingArea.SetActive(false);
            References.cameraMovement.SetCamera(2);
            GameObject.Find("UI").transform.Find("PowerLevelSlider").gameObject.SetActive(true);
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
                References.audioManager.Play("golf-powertrig");
                powerLevel = targetPowerLevel;
                golfShootPhase++;
            }
        }
        else if (golfShootPhase == 2)
        {
            GameObject.Find("UI").transform.Find("DirectionLevelSlider").gameObject.SetActive(true);
            // set direction
            HandleDirectionMeter();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                directionLevel = targetDirectionLevel - 50;
                References.cameraMovement.ApplyCameraRotationOffset(directionLevel / 10);
                Vector3 playerPosition3 = playerChip.transform.position;
                playerPosition3.y = 0;
                Vector3 cameraPosition3 = References.mainCamera.transform.position;
                cameraPosition3.y = 0;
                Vector3 targetDir = playerPosition3 - cameraPosition3;
                Vector3 positionFromCameraToChip = References.mainCamera.transform.position - playerChip.transform.position;
                positionFromCameraToChip.y = 0;
                chipTarget = positionFromCameraToChip;
                References.audioManager.Play("golf-directiontrig");

                References.cameraMovement.ApplyCameraRotationOffset(-directionLevel / 10);
                golfShootPhase++;
            }
        }
        else if (golfShootPhase == 3)
        {
            GameObject.Find("PowerLevelSlider").SetActive(false);
            GameObject.Find("DirectionLevelSlider").SetActive(false);

            golfShootPhase++;
            // modify thrust based on power level
            float powerMultiplier = (powerLevel + 20) / 100; // gets a value between 0.2 and 1.2
            finalThrust = thrust * powerMultiplier;
            References.cameraMovement.SetCamera(3);
            StartCoroutine(FireChipAfterXSeconds(3));
        }
        else if (golfShootPhase == 4)
        {

        }
        else if (golfShootPhase == 5)
        {
            skipTurnTimer += Time.deltaTime;
            if (skipTurnTimer > secondsToSkipTurn)
            {
                golfShootPhase++;
            }
            else if (chipRb.velocity == Vector3.zero)
            {
                golfShootPhase++;
            }
            else if (chipHasLeftTheBoard)
            {
                golfShootPhase++;
            }
        }
        else if (golfShootPhase == 6)
        {
            // check if piece is out of bounds
            References.gameManager.RemoveOutOfBoundsPieces();
            golfShootPhase++;
        }
        else if (golfShootPhase == 7)
        {
            References.scoreManager.CalculateCurrentBoardState();
            golfShootPhase++;
        }
        else if (golfShootPhase == 8)
        {
            golfShootPhase++;
            turnPhase++;
        }

    }

    void HandlePowerMeter()
    {
        powerTimer += Time.deltaTime * 1.2f;
        if (powerTimer > waitTime)
        {
            powerTimer = 0;
            if (targetPowerLevel < 101)
            {
                targetPowerLevel++;
            }
            else
            {
                targetPowerLevel = 0;
            }
            GameObject.Find("PowerLevelSlider").GetComponent<Slider>().value = targetPowerLevel;
        }
    }

    void HandleDirectionMeter()
    {
        GameObject.Find("DirectionLevelSlider").SetActive(true);
        directionTimer += Time.deltaTime * 1.2f;
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
            References.firingArea.SetActive(true);
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
                    placeholderChip.GetComponentInChildren<Renderer>().material.color = References.playerOneColour;
                    playerChip.GetComponentInChildren<Renderer>().material.color = References.playerOneColour;
                }
                else
                {
                    placeholderChip.GetComponentInChildren<Renderer>().material.color = References.playerTwoColour;
                    playerChip.GetComponentInChildren<Renderer>().material.color = References.playerTwoColour;
                    // position the camera above the chip
                }

                if (Input.GetButtonDown("Fire1"))
                {
                    References.audioManager.Play("chip-placed");
                    References.cameraMovement.PositionCameraAboveChip();
                    placeholderChip.SetActive(false);
                    turnPhase++;
                    golfShootPhase = 0;
                    References.audioManager.chipCollisionsThisTurn = 0;
                }
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

    IEnumerator FireChipAfterXSeconds(int seconds)
    {
        hasTakenShot = true;
        yield return new WaitForSeconds(seconds);
        References.audioManager.Play("chip-fired");
        chipRb.AddForce(chipTarget * -finalThrust, ForceMode.Impulse);
        yield return new WaitForSeconds(3);
        golfShootPhase++;
    }

    private void UnlockInput()
    {
        inputLocked = false;
    }

    private void HandlePieceLeavingBoard()
    {
        if (playerChip.transform.position.y < -40 && chipHasLeftTheBoard == false)
        {
            References.audioManager.Play("chip-outofbounds");
            chipHasLeftTheBoard = true;
        }
        if (chipHasLeftTheBoard)
        {
            // move the chip, then the child to the side of the board
            transform.position = new Vector3(0, 0, References.isPlayerOnesTurn ? References.boardSize : -References.boardSize);
            playerChip.transform.position = transform.position;
            playerChip.transform.rotation = transform.rotation;
            playerChip.SetActive(false);
        }
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
