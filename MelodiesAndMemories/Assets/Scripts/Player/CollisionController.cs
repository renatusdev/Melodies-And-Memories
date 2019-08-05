using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    private Rigidbody myRigidbody;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionExit(Collision collision)
    {
        myRigidbody.velocity = Vector3.zero;
    }
}
