using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSimplePlayer : MonoBehaviour {
    public int speed = 10;

    Rigidbody rigidbody;
    Vector3 velocity;

    // Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * speed;
	}

    private void FixedUpdate() {
        rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
    }
}
