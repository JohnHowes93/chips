using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBoard : MonoBehaviour
{
    public GameObject menuChipPrefab;
    public GameObject menuPinPrefab;
    void Start()
    {
        MakePins(menuPinPrefab, Vector3.zero, 8);
    }

    public void MakePins(GameObject obj, Vector3 location, int howMany)
    {
        for (int i = 0; i < howMany; i++)
        {
            float radius = 3.5f;
            float angle = i * Mathf.PI * 2f / howMany;
            Vector3 newPos = transform.position + (new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius));
            Instantiate(obj, newPos, Quaternion.Euler(0, 0, 0), gameObject.transform);
        }
    }

    void Update() { }

}
