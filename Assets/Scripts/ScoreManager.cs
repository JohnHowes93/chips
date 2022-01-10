using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int playerOneScore, playerTwoScore;
    public float innerCircleRange = 3.5f;
    public float outerCircleRange = 7f;
    public int pottedScore = 20;
    public int innerCircleScore = 15;
    public int middleCircleScore = 10;
    public int outerCircleScore = 5;
    // Start is called before the first frame update

    void Awake()
    {
        References.scoreManager = this;
    }
    void Start()
    {
        playerOneScore = 0;
        playerTwoScore = 0;
    }
    public void CalculateScoreForAllChips()
    {
        foreach (GameObject playerChip in References.playerOneChips)
        {
            ChipMovement chipData = playerChip.GetComponentInChildren<ChipMovement>();
            if (chipData.chipPotted == true)
            {
                playerOneScore += pottedScore;
            }
            else
            {
                float distanceFromCenter = Vector3.Distance(playerChip.transform.Find("PlayerChip").transform.position, Vector3.zero);
                if (distanceFromCenter < innerCircleRange)
                {
                    playerOneScore += innerCircleScore;
                }
                else if (distanceFromCenter < outerCircleRange)
                {
                    playerOneScore += middleCircleScore;
                }
                else if (distanceFromCenter < (References.boardSize - 0.1f))
                {
                    playerOneScore += outerCircleScore;
                }
            }
        }
        foreach (GameObject playerChip in References.playerTwoChips)
        {
            ChipMovement chipData = playerChip.GetComponentInChildren<ChipMovement>();
            if (chipData.chipPotted)
            {
                playerTwoScore += pottedScore;
            }
            else
            {
                float distanceFromCenter = Vector3.Distance(playerChip.transform.Find("PlayerChip").transform.position, Vector3.zero);
                if (distanceFromCenter < innerCircleRange)
                {
                    playerTwoScore += innerCircleScore;
                }
                else if (distanceFromCenter < outerCircleRange)
                {
                    playerTwoScore += middleCircleScore;
                }
                else if (distanceFromCenter < (References.boardSize - 0.1f))
                {
                    playerTwoScore += outerCircleScore;
                }
            }
        }
    }
}