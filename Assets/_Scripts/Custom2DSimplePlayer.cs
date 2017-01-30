using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Custom2DSimplePlayer : MonoBehaviour {
    public int speed = 10;

    Rigidbody2D rigidbody;
    Vector2 velocity;

    // Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * speed;
	}

    private void FixedUpdate() {
        rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
    }
}