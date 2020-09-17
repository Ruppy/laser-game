using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float maxMoveSpeed = 50f;
    public float shiftMoveSpeed = 20f;
    private float inputSpeed = 0;
    private Rigidbody2D body;

    void Start() {
        body = GetComponent<Rigidbody2D>();
        inputSpeed = maxMoveSpeed;
    }

    void Update() {
        inputSpeed = maxMoveSpeed;

        if (Input.GetKey("left shift")) {
            inputSpeed = shiftMoveSpeed;
        }
    }

    void FixedUpdate(){
        float movex = Input.GetAxisRaw("Horizontal");
        float movey = Input.GetAxisRaw("Vertical");
        body.AddRelativeForce(new Vector2(movex * inputSpeed, movey * inputSpeed));

        if (Mathf.Abs(movex) > 0.1 || Mathf.Abs(movey) > 1) { return; }
    }
}