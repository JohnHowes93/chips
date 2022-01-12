using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardScript : MonoBehaviour
{
    public GameObject pinObject, firingArea;
    public List<GameObject> pinsList;
    // Start is called before the first frame update
    private float pinRotateSpeed = 16f;
    void Start()
    {
        MakePins(pinObject, Vector3.zero, 8);
        DrawBoard();
        References.firingArea = firingArea;
    }

    void FixedUpdate()
    {
        if (References.isAPieceOnTheBoard == false)
        {
            SpinPins();
        }
    }

    public void MakePins(GameObject obj, Vector3 location, int howMany)
    {
        for (int i = 0; i < howMany; i++)
        {
            float radius = 3.5f;
            float angle = i * Mathf.PI * 2f / howMany;
            Vector3 newPos = transform.position + (new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius));
            GameObject newPin = Instantiate(obj, newPos, Quaternion.Euler(0, 0, 0), gameObject.transform);
            pinsList.Add(newPin);
        }
    }

    void DrawBoard()
    {
        GameObject marker15 = new GameObject { name = "marker15", layer = 11 };
        marker15.DrawCircle(References.scoreManager.innerCircleRange, 0.1f);
        GameObject marker10 = new GameObject { name = "marker10", layer = 11 };
        marker10.DrawCircle(References.scoreManager.outerCircleRange, 0.1f);
        GameObject marker5 = new GameObject { name = "marker5", layer = 11 };
        marker5.DrawCircle(13, 0.1f);
    }

    void SpinPins()
    {
        foreach (GameObject pin in pinsList)
        {
            pin.transform.RotateAround(Vector3.zero, Vector3.up, Time.fixedDeltaTime * pinRotateSpeed);
        }
    }



}
