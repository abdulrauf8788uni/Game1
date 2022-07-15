using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour
{
    private float spinSpeed = 8f;
    public int PowerUpId = 1;

    // Update is called once per frame
    void FixedUpdate(){
        transform.Rotate(Vector3.up * spinSpeed, Space.World);
    }
}
