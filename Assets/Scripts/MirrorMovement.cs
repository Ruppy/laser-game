using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorMovement : MonoBehaviour
{
    public float timeToIgnoreDrag = 0.3f;
    public float rotationSpeed = 0.001f;

    private MovementController movementScript;
    private int ignoreClickCount = 0;
    private bool isDragging = false;
    private float timeIgnoringDrag = -1;
    private float xRotationReference = 0;


    void Start() {
        movementScript = GameObject.Find("Scripts").GetComponent<MovementController>();
    }

    void Update() {
        if (timeIgnoringDrag < 0) {return;}
        timeIgnoringDrag -= Time.deltaTime;
    }

    void OnMouseDrag()
    {
        if (timeIgnoringDrag > 0) {return;}

        Debug.Log("MouseDrag");
        if (isDragging == false) {
            ignoreClickCount = 0;
            isDragging = true;

            movementScript.movementLock = true;
            xRotationReference = Input.mousePosition.x;
        }

        float rotationDelta = (xRotationReference - Input.mousePosition.x) * rotationSpeed;
        transform.Rotate(0, 0, rotationDelta);
        xRotationReference = Input.mousePosition.x;
    }

    void OnMouseDown() {
        timeIgnoringDrag = timeToIgnoreDrag;

        if (movementScript.isMoving() == false) {
            movementScript.setMirror(transform.gameObject);
            Cursor.visible = false;
            ignoreClickCount = 1;
        }
    }

    void OnMouseUp() {
        isDragging = false;
        if (ignoreClickCount > 0) {
            ignoreClickCount -= 1;
            movementScript.movementLock = false;
            return;
        }

        movementScript.setMirror(null);
        Cursor.visible = true;
    }
}