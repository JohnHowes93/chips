using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision);
    }
}
