using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera cam;
    public GameObject[] censorship;

    void Start()
    {
        cam = GetComponent<Camera>();
        RecalculateCameraBounds();
    }

    bool oneframe = false;

    void Update()
    {
        //if (cam.aspect < .99 || cam.aspect > 1.01) {
        	RecalculateCameraBounds();
        //}
    }

    public void RecalculateCameraBounds() {
    	float screenHeight = 2 * cam.orthographicSize;
	    float screenWidth = screenHeight * cam.aspect;
	    float x = (screenWidth - screenHeight)/2;

    	censorship[0].transform.localScale = new Vector3(x, screenHeight, 1);
    	censorship[1].transform.localScale = new Vector3(x, screenHeight, 1);
    	censorship[0].transform.localPosition = new Vector3(transform.position.x - x/2 - cam.orthographicSize, transform.position.y, transform.position.z + 5);
    	censorship[1].transform.localPosition = new Vector3(transform.position.x + x/2 + cam.orthographicSize, transform.position.y, transform.position.z + 5);

    	// cam.rect = new Rect(0f, 0f, 1f, 1f);

    	// float screenHeight = 2 * cam.orthographicSize;
	    // float screenWidth = screenHeight * cam.aspect;
	    // float w = screenHeight/screenWidth;
	    // float x = (1f - w)/2;

	    // cam.rect = new Rect(x, 0f, w, 1f);
    }

    public void UpdateZoom(float newZoom) {
    	cam = GetComponent<Camera>();
    	cam.orthographicSize = newZoom;
    }
}
