using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMover : MonoBehaviour {

    public float moveSpeed;

    private Rigidbody rb;
    // Update is called once per frame
    private void Start()
    {
        rb  = this.gameObject.GetComponent<Rigidbody>();
        rb.AddRelativeForce(Vector3.forward * moveSpeed);
    }

   
}
