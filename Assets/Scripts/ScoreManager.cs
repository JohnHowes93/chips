using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ScoreManager : MonoBehaviour
{
    public int playerOneScore, playerTwoScore;
    public int playerOneScoreCurrentBoardState, playerTwoScoreCurrentBoardState;
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
    public IEnumerator CalculateScoreForAllChips()
    {
        References.cameraMovement.SetCamera(4);
        // merge the lists
        List<GameObject> mergedLists = new List<GameObject>();
        mergedLists.AddRange(References.playerOneChips);
        mergedLists.AddRange(References.playerTwoChips);

        //use a single compare function
        List<GameObject> sortedP1 = References.playerOneChips.OrderBy(x => -Vector3.Distance(x.transform.Find("PlayerChip").transform.position, Vector3.zero)).ToList();
        List<GameObject> sortedP2 = References.playerTwoChips.OrderBy(x => -Vector3.Distance(x.transform.Find("PlayerChip").transform.position, Vector3.zero)).ToList();
        foreach (GameObject playerChip in sortedP1)
        {
            ChipMovement chipData = playerChip.GetComponentInChildren<ChipMovement>();
            Vector3 playerChipPosition = playerChip.transform.Find("PlayerChip").transform.position;
            if (chipData.chipPotted == true)
            {
                playerOneScore += pottedScore;
            }
            else
            {
                References.cameraMovement.SetDestination(playerChipPosition + (Vector3.up * 10));
                yield return new WaitForSeconds(2);
                float distanceFromCenter = Vector3.Distance(playerChipPosition, Vector3.zero);
                if (distanceFromCenter < innerCircleRange)
                {
                    playerOneScore += innerCircleScore;
                    References.audioManager.Play("board-15");
                    yield return new WaitForSeconds(1);
                }
                else if (distanceFromCenter < outerCircleRange)
                {
                    playerOneScore += middleCircleScore;
                    References.audioManager.Play("board-10");
                    yield return new WaitForSeconds(1);
                }
                else if (distanceFromCenter < (References.boardSize - 0.1f))
                {
                    playerOneScore += outerCircleScore;
                    References.audioManager.Play("board-5");
                    yield return new WaitForSeconds(1);
                }
            }
        }
        foreach (GameObject playerChip in sortedP2)
        {
            ChipMovement chipData = playerChip.GetComponentInChildren<ChipMovement>();
            Vector3 playerChipPosition = playerChip.transform.Find("PlayerChip").transform.position;
            if (chipData.chipPotted)
            {
                playerTwoScore += pottedScore;
            }
            else
            {
                yield return new WaitForSeconds(2);
                References.cameraMovement.SetDestination(playerChipPosition + (Vector3.up * 10));
                float distanceFromCenter = Vector3.Distance(playerChipPosition, Vector3.zero);
                if (distanceFromCenter < innerCircleRange)
                {
                    playerTwoScore += innerCircleScore;
                    References.audioManager.Play("board-15");
                    yield return new WaitForSeconds(1);
                }
                else if (distanceFromCenter < outerCircleRange)
                {
                    playerTwoScore += middleCircleScore;
                    References.audioManager.Play("board-10");
                    yield return new WaitForSeconds(1);
                }
                else if (distanceFromCenter < (References.boardSize - 0.1f))
                {
                    playerTwoScore += outerCircleScore;
                    References.audioManager.Play("board-5");
                    yield return new WaitForSeconds(1);
                }
            }
        }
    }

    private int CompareChipDistanceFromCenter(GameObject chip1, GameObject chip2)
    {
        Debug.Log("testing");
        Debug.Log(chip1);
        Debug.Log(chip2);
        return 1;
    }

    public void CalculateCurrentBoardState()
    {
        playerOneScoreCurrentBoardState = 0;
        playerTwoScoreCurrentBoardState = 0;
        foreach (GameObject playerChip in References.playerOneChips)
        {
            ChipMovement chipData = playerChip.GetComponentInChildren<ChipMovement>();
            if (chipData.chipPotted == true)
            {
                playerOneScoreCurrentBoardState += pottedScore;
            }
            else
            {
                float distanceFromCenter = Vector3.Distance(playerChip.transform.Find("PlayerChip").transform.position, Vector3.zero);
                if (distanceFromCenter < innerCircleRange)
                {
                    playerOneScoreCurrentBoardState += innerCircleScore;
                }
                else if (distanceFromCenter < outerCircleRange)
                {
                    playerOneScoreCurrentBoardState += middleCircleScore;
                }
                else if (distanceFromCenter < (References.boardSize - 0.1f))
                {
                    playerOneScoreCurrentBoardState += outerCircleScore;
                }
            }
        }
        foreach (GameObject playerChip in References.playerTwoChips)
        {
            ChipMovement chipData = playerChip.GetComponentInChildren<ChipMovement>();
            if (chipData.chipPotted)
            {
                playerTwoScoreCurrentBoardState += pottedScore;
            }
            else
            {
                float distanceFromCenter = Vector3.Distance(playerChip.transform.Find("PlayerChip").transform.position, Vector3.zero);
                if (distanceFromCenter < innerCircleRange)
                {
                    playerTwoScoreCurrentBoardState += innerCircleScore;
                }
                else if (distanceFromCenter < outerCircleRange)
                {
                    playerTwoScoreCurrentBoardState += middleCircleScore;
                }
                else if (distanceFromCenter < (References.boardSize - 0.1f))
                {
                    playerTwoScoreCurrentBoardState += outerCircleScore;
                }
            }
        }
    }

    public void HighlightPieceAndCountScore()
    {
        // move the camera above the piece
        // play sound for score
    }
}