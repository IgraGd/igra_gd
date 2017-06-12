using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class daycycle : MonoBehaviour
{
    public int axe = 1; //1 - x, 2 - y, 3 - z
    public float speed = 10F;
    // Update is called once per frame
    void Update()
    {
        float x = 0;
        float y = 0;
        float z = 0;
        if (axe == 1) { x = speed * Time.deltaTime; }
        if (axe == 2) { y = speed * Time.deltaTime; }
        if (axe == 3) { z = speed * Time.deltaTime; }
        transform.Rotate(x, y, z, Space.World);
    }
}
