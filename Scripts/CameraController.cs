using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float panSpeed = 30f;
    public float panBorderThickness = 10f;

    public float scrollSpeed = 5f;
    public float minZoom = 10f;
    public float maxZoom = 80;
    public float zoom = 10f;

    private bool doMovement = true;

    public float maxX;
    public float minX;
    public float maxY;
    public float minY;
    float xPos = 0;
    float yPos = 0;

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
            doMovement = !doMovement;

        if (!doMovement)
            return;

        if (Input.GetKey("w") || Input.GetKey(KeyCode.UpArrow) || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            yPos += panSpeed * Time.deltaTime;
            yPos = Mathf.Clamp(yPos, minY, maxY);
            transform.position = new Vector3(xPos, yPos, -10);
        }
        if (Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow) || Input.mousePosition.y <= panBorderThickness)
        {
            yPos -= panSpeed * Time.deltaTime;
            yPos = Mathf.Clamp(yPos, minY, maxY);
            transform.position = new Vector3(xPos, yPos, -10);
        }
        if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            xPos += panSpeed * Time.deltaTime;
            xPos = Mathf.Clamp(xPos, minX, maxX);
            transform.position = new Vector3(xPos, yPos, -10);
        }
        if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow) || Input.mousePosition.x <= panBorderThickness)
        {
            xPos -= panSpeed * Time.deltaTime;
            xPos = Mathf.Clamp(xPos, minX, maxX);
            transform.position = new Vector3(xPos, yPos, -10);
        }


        float scroll = Input.GetAxis("Mouse ScrollWheel");

        zoom -= scroll * 700 * scrollSpeed * Time.deltaTime;
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);

        Camera.main.orthographicSize = zoom;
    }
}
