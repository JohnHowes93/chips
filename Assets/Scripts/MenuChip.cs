using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuChip : MonoBehaviour
{
    public float upForce = 2f;
    public float sideForce = .1f;
    private float minVolume, maxVolume, boardMinMagnitude, boardMaxMagnitude, boardBounceThreshold, chipMinMagnitude, chipMaxMagnitude;


    // Start is called before the first frame update
    void Start()
    {
        Renderer objRenderer = GetComponent<Renderer>();
        objRenderer.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        float xForce = Random.Range(-sideForce, sideForce);
        float yForce = Random.Range(-upForce / 2f, upForce);
        float zForce = Random.Range(-sideForce, sideForce);
        Vector3 force = new Vector3(xForce, yForce, zForce);
        GetComponent<Rigidbody>().velocity = force;
        boardMinMagnitude = 0f;
        boardMaxMagnitude = 30f;
        boardBounceThreshold = 0.4f;
        minVolume = 0f;
        maxVolume = 1f;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "BoardBase")
        {

            float scaledVolumeLevel = scale(boardMinMagnitude, boardMaxMagnitude, minVolume, maxVolume, other.relativeVelocity.y);
            if (scaledVolumeLevel > boardBounceThreshold)
            {
                References.audioManager.RandomChipCollision(scaledVolumeLevel);
            }
        }
        else if (other.gameObject.tag == "Chip")
        {
            float scaledVolumeLevel = scale(chipMinMagnitude, chipMaxMagnitude, minVolume, maxVolume, other.relativeVelocity.magnitude);
            if (scaledVolumeLevel > 0.4f)
            {
                References.audioManager.HandleMenuChipCollision(scaledVolumeLevel);
            }
        }
    }

    public float scale(float inputMin, float inputMax, float outputMin, float outputMax, float inputValue)
    {
        float inputRange = (inputMax - inputMin);
        float outputRange = (outputMax - outputMin);
        float outputValue = (((inputValue - inputMin) * outputRange) / inputRange) + outputMin;
        return (outputValue);
    }
}
