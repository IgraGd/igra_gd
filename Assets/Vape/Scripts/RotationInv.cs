using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationInv : MonoBehaviour
{

    public float speed = 100F;
    void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime, 0, Space.World);
    }

}
