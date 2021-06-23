using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float Speed = 10;
    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, Speed * Time.deltaTime);
    }
}
