using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int playerOneScore, playerTwoScore;
    const float innerCircleRange = 3.5f;
    const float outerCircleRange = 7f;
    const int pottedScore = 20;
    const int innerCircleScore = 15;
    const int middleCircleScore = 10;
    const int outerCircleScore = 5;
    // Start is called before the first frame update
    void Start()
    {
        playerOneScore = 0;
        playerTwoScore = 0;
        References.scoreManager = this;
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
        Debug.Log("playerOneScore " + playerOneScore);
        Debug.Log("playerTwoScore " + playerTwoScore);
    }
}