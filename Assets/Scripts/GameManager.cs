using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject chipPrefab;
    public List<GameObject> playerOneChips, playerTwoChips;
    public int turnReference;
    public Vector3 playerOneChipLocation, playerTwoChipLocation, chipPositionOffset;
    public static bool isPlayerOnesTurn;
    const float outOfBoundsDistance = 12.5f;

    public int chipsPerPlayer;

    void Start()
    {
        chipsPerPlayer = 3;
        References.outOfBoundsDistance = outOfBoundsDistance;
        References.isPlayerOnesTurn = true;
        References.gameManager = this;
        chipPositionOffset = new Vector3(1, 0, 0);
        References.boardSize = 19;
        playerOneChipLocation = new Vector3(0, 0, -References.boardSize);
        playerTwoChipLocation = new Vector3(0, 0, References.boardSize);
        turnReference = 0;
        BuildPlayerChips();
        NewTurn();
        References.cameraMovement.SetCamera(0);
    }

    void Update()
    {


    }

    void BuildPlayerChips()
    {
        playerOneChips = new List<GameObject>();
        playerTwoChips = new List<GameObject>();
        for (int i = 0; i < chipsPerPlayer; i++)
        {
            GameObject playerOneChip = Instantiate(chipPrefab);
            playerOneChip.name = "Player 1 Chip " + (i + 1);
            playerOneChip.transform.position = new Vector3(i, 0, References.boardSize);
            playerOneChips.Add(playerOneChip);

            GameObject playerTwoChip = Instantiate(chipPrefab);
            playerTwoChip.name = "Player 2 Chip " + (i + 1);
            playerTwoChip.transform.position = new Vector3(i, 0, -References.boardSize);
            playerTwoChips.Add(playerTwoChip);
        }
        References.playerOneChips = playerOneChips;
        References.playerTwoChips = playerTwoChips;
    }

    public void NewTurn()
    {
        if (References.isPlayerOnesTurn && playerOneChips[turnReference])
        {
            ChipMovement playerOneMovement = playerOneChips[turnReference].GetComponentInChildren<ChipMovement>();
            playerOneMovement.SetActivePiece(true);
        }
        else if (playerTwoChips[turnReference])
        {
            ChipMovement playerTwoMovement = playerTwoChips[turnReference].GetComponentInChildren<ChipMovement>();
            playerTwoMovement.SetActivePiece(true);
        }
    }

    public void AdvanceTurn()
    {
        if (References.isPlayerOnesTurn)
        {
            References.isPlayerOnesTurn = !References.isPlayerOnesTurn;
            NewTurn();
        }
        else
        {
            turnReference++;
            if (turnReference == playerTwoChips.Count)
            {
                StartCoroutine("HandleScore");
            }
            else
            {

                References.isPlayerOnesTurn = !References.isPlayerOnesTurn;
                NewTurn();
            }
        }
    }

    private IEnumerator HandleScore()
    {
        return References.scoreManager.CalculateScoreForAllChips();
    }

    public void RemoveOutOfBoundsPieces()
    {
        for (int i = 0; i < turnReference + 1; i++)
        {
            GameObject playerOneChip = playerOneChips[i];
            GameObject playerTwoChip = playerTwoChips[i];
            PushChipOffBoard(playerOneChip);
            PushChipOffBoard(playerTwoChip);
        }
    }
    private bool CheckIfPieceIsOutOfBounds(GameObject playerChip)
    {
        float distanceFromCenter = Vector3.Distance(playerChip.transform.Find("PlayerChip").transform.position, Vector3.zero);
        if (distanceFromCenter > outOfBoundsDistance)
        {
            return true;
        }
        else if (distanceFromCenter < -outOfBoundsDistance)
        {
            return true;
        }
        else return false;
    }

    private void PushChipOffBoard(GameObject playerChip)
    {
        ChipMovement chipData = playerChip.GetComponent<ChipMovement>();
        if (chipData.hasTakenShot)
        {
            bool isOutOfBounds = CheckIfPieceIsOutOfBounds(playerChip);
            Rigidbody chipRb = playerChip.transform.Find("PlayerChip").GetComponent<Rigidbody>();
            if (chipRb && isOutOfBounds)
            {
                Vector3 directionFromCenter = Vector3.zero - chipRb.transform.position;
                chipRb.AddForce(-directionFromCenter * 0.5f, ForceMode.Impulse);
            }
        }
    }

}
