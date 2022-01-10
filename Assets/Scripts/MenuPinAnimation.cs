using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPinAnimation : MonoBehaviour
{
    float speed, time, timingOffset;
    float yLowerThreshold = -0.8f;
    float yUpperThreshold = 0.3f;
    Vector3 lowerPosition, upperPosition;
    void Awake()
    {
        timingOffset = Random.value * (Mathf.PI / 2);
    }
    void Start()
    {
        speed = 1f;
        lowerPosition = new Vector3(transform.position.x, yLowerThreshold, transform.position.z);
        upperPosition = new Vector3(transform.position.x, yUpperThreshold, transform.position.z);
    }

    void Update()
    {

        time = Mathf.PingPong(Mathf.Sin((Time.time + timingOffset) * speed), 1);
        transform.position = Vector3.Lerp(lowerPosition, upperPosition, time);
    }

}