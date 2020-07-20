using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorMovement : MonoBehaviour
{
    private MovementController movementScript;
    void Start() {
        movementScript = GameObject.Find("Scripts").GetComponent<MovementController>();
    }

    void OnMouseDrag()
    {
    }

    void OnMouseDown() {
        if (movementScript.isMoving()) {
            movementScript.setMirror(null);
        }
        else {
            movementScript.setMirror(transform.gameObject);
        }
    }
}