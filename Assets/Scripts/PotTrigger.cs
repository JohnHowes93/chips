using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        GameObject chip = collision.gameObject;
        ChipMovement chipData = chip.GetComponentInParent<ChipMovement>();
        chipData.chipPotted = true;
        References.audioManager.Play("board-20");
    }
}
