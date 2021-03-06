using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class References : MonoBehaviour
{
    public static GameManager gameManager;
    public static TimeManager timeManager;
    public static float boardSize;
    public static bool isPlayerOnesTurn, isAPieceOnTheBoard;
    public static CameraMovement cameraMovement;
    public static GameObject activeChip, firingArea;
    public static GameObject mainCamera;
    public static ScoreManager scoreManager;
    public static List<GameObject> playerOneChips, playerTwoChips;
    public static float outOfBoundsDistance;
    public static Color playerOneColour, playerTwoColour;
    public static AudioManager audioManager;
    void Start()
    {
        playerOneColour = Color.blue;
        playerTwoColour = Color.red;
    }
}
