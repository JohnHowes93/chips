using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject chipPrefab;
    public List<GameObject> playerOneChips;
    public List<GameObject> playerTwoChips;
    public int turnReference;
    public Vector3 playerOneChipLocation;
    public Vector3 playerTwoChipLocation;
    public Vector3 chipPositionOffset;
    public static bool isPlayerOnesTurn;

    void Start()
    {
        References.isPlayerOnesTurn = true;
        References.gameManager = this;
        chipPositionOffset = new Vector3(1, 0, 0);
        References.boardSize = 16;
        playerOneChipLocation = new Vector3(0, 0, -References.boardSize);
        playerTwoChipLocation = new Vector3(0, 0, References.boardSize);
        turnReference = 0;
        BuildPlayerChips();
        NewTurn();
    }

    void BuildPlayerChips()
    {
        playerOneChips = new List<GameObject>();
        playerTwoChips = new List<GameObject>();
        for (int i = 0; i < 6; i++)
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
            References.isPlayerOnesTurn = !References.isPlayerOnesTurn;
            NewTurn();
        }
    }
}
