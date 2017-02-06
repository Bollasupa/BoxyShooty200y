using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    Camera cam;

	// Initializes lookat to world centre
	void Start () {
        //Vector3 camToCenter = Vector3.zero - transform.position

        transform.rotation = Quaternion.LookRotation(-transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
