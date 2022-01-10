using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuChip : MonoBehaviour
{
    public float upForce = 2f;
    public float sideForce = .1f;

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
    }
}
