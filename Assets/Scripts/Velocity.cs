using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Velocity : MonoBehaviour
{
    public float speed;

    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.TransformDirection(Vector3.forward * speed);
    }

}
