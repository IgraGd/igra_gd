using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosManager : MonoBehaviour
{
    public static PlayerPosManager Instance { get; private set; }
    public Vector3 playerPos;
    public Quaternion playerRot;

    public void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        playerPos = transform.position;
        playerRot = transform.rotation;
    }
}
