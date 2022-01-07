using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        thrust = 2;
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
            HandlePieceLeavingBoard();
            if (turnPhase == 0)
            {
                PositionChipOnBoard();
            }
            if (turnPhase == 1)
            {
                Shoot();
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

    void PositionChipOnBoard()
    {
        if (placeholderChip.activeSelf)
        {
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
                }

                if (Input.GetButtonDown("Fire1") && !inputLocked)
                {
                    StartCoroutine(LockInputForXSeconds(1));
                    placeholderChip.SetActive(false);
                    turnPhase++;
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
