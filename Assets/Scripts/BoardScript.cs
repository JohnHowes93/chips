using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardScript : MonoBehaviour
{
    public GameObject pinObject;
    // Start is called before the first frame update
    void Start()
    {
        MakePins(pinObject, Vector3.zero, 8);
        DrawBoard();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MakePins(GameObject obj, Vector3 location, int howMany)
    {
        for (int i = 0; i < howMany; i++)
        {
            float radius = 3.5f;
            float angle = i * Mathf.PI * 2f / howMany;
            Vector3 newPos = transform.position + (new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius));
            Instantiate(obj, newPos, Quaternion.Euler(0, 0, 0), gameObject.transform);
            // obj.transform.parent = gameObject.transform;
        }
    }

    void DrawBoard()
    {
        GameObject marker15 = new GameObject { name = "marker15" };
        marker15.DrawCircle(References.scoreManager.innerCircleRange, 0.1f);
        GameObject marker10 = new GameObject { name = "marker10" };
        marker10.DrawCircle(References.scoreManager.outerCircleRange, 0.1f);
        GameObject marker5 = new GameObject { name = "marker5" };
        marker5.DrawCircle(13, 0.1f);
    }

}
