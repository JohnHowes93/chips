using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipCollision : MonoBehaviour
{
    private float pinMinMagnitude, pinMaxMagnitude, minVolume, maxVolume, chipMinMagnitude, chipMaxMagnitude;

    void Start()
    {
        pinMinMagnitude = 0f;
        pinMaxMagnitude = 30f;
        chipMinMagnitude = 0f;
        chipMaxMagnitude = 30f;
        minVolume = 0f;
        maxVolume = 1f;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Pin")
        {
            float scaledVolumeLevel = scale(pinMinMagnitude, pinMaxMagnitude, minVolume, maxVolume, other.relativeVelocity.magnitude);
            if (scaledVolumeLevel > 0)
            {
                References.audioManager.HandlePinCollision(scaledVolumeLevel);
            }
        }
        else if (other.gameObject.tag == "Chip")
        {
            float scaledVolumeLevel = scale(chipMinMagnitude, chipMaxMagnitude, minVolume, maxVolume, other.relativeVelocity.magnitude);
            Debug.Log(scaledVolumeLevel);
            if (scaledVolumeLevel > 0)
            {
                References.audioManager.HandleChipCollision(scaledVolumeLevel);
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
