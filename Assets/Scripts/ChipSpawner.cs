using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipSpawner : MonoBehaviour
{
    ObjectPooler objectPooler;
    public TimeManager timeManager;
    private float interval = 0.1f;
    private float currentTime;

    private void Start()
    {
        currentTime = 0;
        timeManager.DoSlowMotion();
        objectPooler = ObjectPooler.Instance;
    }

    void FixedUpdate()
    {
        currentTime += Time.deltaTime;
        if (currentTime > interval)
        {
            objectPooler.SpawnFromPool("Chip", transform.position, Quaternion.identity);
            currentTime = 0;
        }
    }
}
