using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSimplePlayer : MonoBehaviour {
    public float speed = 10.0f;

    Rigidbody rigidbody;
    Vector3 velocity;

    // Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;

        rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        //velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * speed;
        // replaced above code with:
        float forwardAndBackward = Input.GetAxisRaw("Vertical") * speed;
        float strafe = Input.GetAxisRaw("Horizontal") * speed;
        forwardAndBackward *= Time.deltaTime;
        strafe *= Time.deltaTime;

        transform.Translate(strafe, 0, forwardAndBackward);

        // The position of this if statement matters.  
        // If you put this in the beginning of the Update function, the code will not get to "velocity", and it will keep on staing on this if statement.
        if (Input.GetKeyDown("Escape"))
            Cursor.lockState = CursorLockMode.None;
    }

    private void FixedUpdate() {
        // Commented this out because the code in Update() made this redundant.
        //rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);

        // Commented this out because the code in CameraMouseLook.cs made this redundant.
        //rigidbody.rotation = Quaternion.Euler(rigidbody.rotation.eulerAngles + new Vector3(1f * Input.GetAxisRaw("Mouse Y"), 1f * Input.GetAxisRaw("Mouse X"), 0f));
    }
}