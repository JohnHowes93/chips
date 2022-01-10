using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBoard : MonoBehaviour
{
    public GameObject menuPinPrefab;
    void Start()
    {
        int howMany = 8;
        for (int i = 0; i < howMany; i++)
        {
            float radius = 3.5f;
            float angle = i * Mathf.PI * 2f / howMany;
            Vector3 newPos = transform.position + (new Vector3(Mathf.Cos(angle) * radius, -8, Mathf.Sin(angle) * radius));
            MenuPinAnimation newObject = Instantiate(menuPinPrefab, newPos, Quaternion.Euler(0, 0, 0), gameObject.transform).GetComponent<MenuPinAnimation>();
        }
    }
}
