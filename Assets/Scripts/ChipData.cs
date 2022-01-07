using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ChipData
{
    public int id;
    public bool isActive;
    public GameObject chip;
    public ChipMovement movement;

    public ChipData(int newId, GameObject chipPrefab)
    {
        id = newId;
        isActive = false;
        chip = chipPrefab;
    }

}
