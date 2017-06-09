using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScroll : MonoBehaviour
{

    public float coefficient = 5;

    void Update()
    {
        float rightXEnd = Mathf.Max((inventory.Instance.invList.Count - 1) * inventory.Instance.invDistance, 5);
        transform.Translate(Vector3.left * coefficient * Input.GetAxis("Mouse ScrollWheel"));
        if (transform.localPosition.x < 0)
        {
            transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
        }
        if (transform.localPosition.x > rightXEnd)
        {
            transform.localPosition = new Vector3(rightXEnd, transform.localPosition.y, transform.localPosition.z);
        }
    }
}
