using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public GameObject mirror;
    public float speed = 10f;

    private bool rotateEnabled = true;
    private float rotatedSecondsAgo = 0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (mirror == null) { return; }
        float rotationDelta = 0;

        if (Input.GetMouseButton(1))
        {
            rotationDelta = 15f;
        }

        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
        mirror.transform.position = curPosition;

        if (rotateEnabled == false) {
            rotatedSecondsAgo += Time.deltaTime;
            rotateEnabled = rotatedSecondsAgo > 0.05;
        }
        else {
            if (Input.GetKey("z")) {
              rotationDelta = 3f;
            }
            if (Input.GetKey("x")) {
              rotationDelta = -3f;
            }

            mirror.transform.Rotate(0, 0, rotationDelta);
            rotateEnabled = false;
            rotatedSecondsAgo = 0;
        }



    }

    public void setMirror(GameObject selectedMirror) {
        mirror = selectedMirror;
    }

    public bool isMoving(){
        return mirror != null;
    }
}
