using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float maxMoveSpeed = 50f;
    public float shiftMoveSpeed = 20f;
    private float inputSpeed = 0;
    private Rigidbody2D body;

    public GameObject collidingMirror;
    public bool isMovingMirror = false;
    public Vector3 mirrorPositionDifference;

    private void OnEnable() {
        EventHandler.onStepChange += OnStepChange;
    }

    private void OnDisable() {
        EventHandler.onStepChange -= OnStepChange;
    }

    void Start() {
        body = GetComponent<Rigidbody2D>();
        inputSpeed = maxMoveSpeed;
    }

    void Update() {
        inputSpeed = maxMoveSpeed;

        if (Input.GetKey("left shift")) {
            inputSpeed = shiftMoveSpeed;
        }

        if (Input.GetKeyDown("space")) {
            if (isMovingMirror) {
                CancelDrag();
            }
            else {
                CheckForMirrorNearby();
            }
        }
    }

    void CheckForMirrorNearby() {
        Collider2D[] allOverlappingColliders = new Collider2D[16];
        Collider2D collider = gameObject.transform.GetChild(0).GetComponent<Collider2D>();
        ContactFilter2D cf = new ContactFilter2D();
        collider.OverlapCollider(cf.NoFilter(), allOverlappingColliders);
        foreach (Collider2D colliderOverlaping in allOverlappingColliders) {
            if (colliderOverlaping == null) { break; }
            if (colliderOverlaping.gameObject.CompareTag("Mirror")) {
                StartDrag(colliderOverlaping.gameObject);
                break;
            }
        }
    }

    void StartDrag(GameObject dragObject) {
        collidingMirror = dragObject;
        isMovingMirror = true;
        mirrorPositionDifference = transform.position - dragObject.transform.position;
        collidingMirror.GetComponent<Collider2D>().isTrigger = true;
    }

    void CancelDrag() {
        if (collidingMirror) {
            collidingMirror.GetComponent<Collider2D>().isTrigger = false;
        }
        isMovingMirror = false;
        collidingMirror = null;
    }

    public void OnStepChange() {
        CancelDrag();
    }

    void FixedUpdate(){
        float movex = Input.GetAxisRaw("Horizontal");
        float movey = Input.GetAxisRaw("Vertical");
        body.AddRelativeForce(new Vector2(movex * inputSpeed, movey * inputSpeed));

        if (isMovingMirror && collidingMirror) {
            collidingMirror.transform.position = transform.position - mirrorPositionDifference;
        }

        if (Mathf.Abs(movex) > 0.1 || Mathf.Abs(movey) > 1) { return; }
    }
}