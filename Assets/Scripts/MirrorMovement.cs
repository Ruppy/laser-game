using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorMovement : MonoBehaviour
{
    public float timeToIgnoreDrag = 0.3f;
    public float rotationSpeed = 0.001f;
    public float scrollRotationSpeed = 5f;

    private MovementController movementScript;
    private int ignoreClickCount = 0;
    private bool isDragging = false;
    private float timeIgnoringDrag = -1;
    private float xRotationReference = 0;
    private float rotationDelta = 0f;


    void Start() {
        movementScript = GameObject.Find("Scripts").GetComponent<MovementController>();
    }

    void Update() {
        if (movementScript.mirror == gameObject) {
            rotationDelta = Input.mouseScrollDelta.y * scrollRotationSpeed;
            transform.Rotate(0, 0, rotationDelta);
        }

        if (timeIgnoringDrag > 0) {
            timeIgnoringDrag -= Time.deltaTime;
        }
    }

    void OnMouseDrag()
    {
        if (timeIgnoringDrag > 0) {return;}

        if (isDragging == false) {
            ignoreClickCount = 0;
            isDragging = true;

            movementScript.movementLock = true;
            xRotationReference = Input.mousePosition.x;
        }

        rotationDelta = (xRotationReference - Input.mousePosition.x) * rotationSpeed;
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