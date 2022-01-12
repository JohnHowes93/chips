
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float slowdownFactor = 0.5f;
    public void DoSlowMotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    public void DoNormalMotion()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    float currentAmount = 0f;
    float maxAmount = 5f;

    // Use this for initialization
    void Start()
    {
        References.timeManager = this;
        DoSlowMotion();
    }
}
