using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float movementSpeed;

	private bool movingUp;
	private bool movingDown;
	private bool movingLeft;
	private bool movingRight;

    public enum ControlState
    {
        Move,
        Type,
        PauseMenu,
        GameOver
    }

    public ControlState controlState = ControlState.Move;

    private Camera cam;
	private BoxCollider2D boundingBox;

    // Start is called before the first frame update
    void Start()
    {
    	cam = Camera.main;
        boundingBox = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update() {
    	boundingBox.size = new Vector2(cam.orthographicSize*2, cam.orthographicSize*2);
        if (controlState == ControlState.Move) {
			//these axes should be configured to go up to 1 and down back to 0 almost instantly
			movingUp = Input.GetAxis("Vertical") > 0;
			movingDown = Input.GetAxis("Vertical") < 0;
			movingLeft = Input.GetAxis("Horizontal") < 0;
			movingRight = Input.GetAxis("Horizontal") > 0;
		}
    }

    void FixedUpdate() {
    	if (Cursor.lockState == CursorLockMode.Locked) {
	    	if (movingUp) {
	    		transform.Translate(Vector2.up * movementSpeed);
	    	}
	    	if (movingDown) {
	    		transform.Translate(Vector2.down * movementSpeed);
	    	}
	    	if (movingLeft) {
	    		transform.Translate(Vector2.left * movementSpeed);
	    	}
	    	if (movingRight) {
	    		transform.Translate(Vector2.right * movementSpeed);
	    	}
	    }
    }

    public void ResetPosition(Vector3 sc) {
    	transform.position = new Vector3(-sc.x/2 + transform.localScale.x/2, 0, 0);
    }
}
