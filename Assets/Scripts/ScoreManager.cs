using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

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
    public TextMeshProUGUI p1ScoreIndicator, p2ScoreIndicator, p1PostMatchScoreIndicator, p2PostMatchScoreIndicator;
    public GameObject postMatchUI, scoringUI;
    // Start is called before the first frame update

    void Awake()
    {
        References.scoreManager = this;
    }
    void Start()
    {
        NewGame();
    }

    public void NewGame()
    {

        References.isAPieceOnTheBoard = false;
        playerOneScore = 0;
        playerTwoScore = 0;
        p1ScoreIndicator.SetText(playerOneScore.ToString());
        p1PostMatchScoreIndicator.SetText(playerOneScore.ToString());
        p2ScoreIndicator.SetText(playerTwoScore.ToString());
        p2PostMatchScoreIndicator.SetText(playerTwoScore.ToString());
        postMatchUI.SetActive(false);
        scoringUI.SetActive(true);
    }
    public IEnumerator CalculateScoreForAllChips()
    {
        // merge the lists
        List<GameObject> mergedLists = new List<GameObject>();
        mergedLists.AddRange(References.playerOneChips);
        mergedLists.AddRange(References.playerTwoChips);
        List<GameObject> sortedList = mergedLists.OrderBy(x => -Vector3.Distance(x.transform.Find("PlayerChip").transform.position, Vector3.zero)).ToList();
        //use a single compare function
        foreach (GameObject playerChip in sortedList)
        {
            string playerNumber = playerChip.name.Substring(7, 1);
            ChipMovement chipData = playerChip.GetComponentInChildren<ChipMovement>();
            Vector3 playerChipPosition = playerChip.transform.Find("PlayerChip").transform.position;
            float distanceFromCenter = Vector3.Distance(playerChipPosition, Vector3.zero);
            if (distanceFromCenter > References.boardSize)
            {

            }
            if (chipData.chipPotted == true)
            {
                switch (playerNumber)
                {
                    case "1":
                        playerOneScore += pottedScore;
                        break;
                    case "2":
                        playerTwoScore += pottedScore;
                        break;
                }
                yield break;
            }
            else
            {
                if (distanceFromCenter < innerCircleRange)
                {
                    References.cameraMovement.SetDestination(playerChipPosition + (Vector3.up * 10));
                    References.cameraMovement.SetCamera(4);
                    yield return new WaitForSeconds(2);
                    switch (playerNumber)
                    {
                        case "1":
                            playerOneScore += innerCircleScore;
                            break;
                        case "2":
                            playerTwoScore += innerCircleScore;
                            break;
                    }
                    References.audioManager.Play("board-15");
                }
                else if (distanceFromCenter < outerCircleRange)
                {
                    References.cameraMovement.SetDestination(playerChipPosition + (Vector3.up * 10));
                    References.cameraMovement.SetCamera(4);
                    switch (playerNumber)
                    {
                        case "1":
                            playerOneScore += middleCircleScore;
                            break;
                        case "2":
                            playerTwoScore += middleCircleScore;
                            break;
                    }
                    References.audioManager.Play("board-10");
                }
                else if (distanceFromCenter < (References.boardSize - 0.1f))
                {
                    References.cameraMovement.SetDestination(playerChipPosition + (Vector3.up * 10));
                    References.cameraMovement.SetCamera(4);
                    switch (playerNumber)
                    {
                        case "1":
                            playerOneScore += outerCircleScore;
                            break;
                        case "2":
                            playerTwoScore += outerCircleScore;
                            break;
                    }
                    References.audioManager.Play("board-5");
                    yield return new WaitForSeconds(2);
                }
            }
            p1ScoreIndicator.SetText(playerOneScore.ToString());
            p1PostMatchScoreIndicator.SetText(playerOneScore.ToString());
            p2ScoreIndicator.SetText(playerTwoScore.ToString());
            p2PostMatchScoreIndicator.SetText(playerTwoScore.ToString());
            yield return new WaitForSeconds(2);
        }
        HandleEndGame();
        yield break;
        // TO DO / BUG SCORE NOT WORKING IF PIECE POTTED OR OFF BOARD
    }

    private void HandleEndGame()
    {
        References.cameraMovement.SetCamera(1);
        References.gameManager.HandlePostMatch();
        postMatchUI.SetActive(true);
        scoringUI.SetActive(false);
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
        if (playerOneScoreCurrentBoardState == 0 && playerTwoScoreCurrentBoardState == 0)
        {
            References.isAPieceOnTheBoard = false;
        }
        else
        {
            References.isAPieceOnTheBoard = true;
        }
    }
}