using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BallController : MonoBehaviour
{
    public float maxMoveSpeed = 50f;
    public float shiftMoveSpeed = 20f;
    private float inputSpeed = 0;

    private float rotateSpeed = 0;
    private float shiftRotateSpeed = 0.3f;
    private float maxRotateSpeed = 1.5f;

    private Rigidbody2D body = null;
    private Rigidbody2D mirrorBody = null;

    public GameObject collidingMirror;
    public bool isMovingMirror = false;
    public Vector3 mirrorPositionDifference;

    private EventHandler eventHandler = EventHandler.get();

    private void OnEnable() {
        EventHandler.onStepChange += OnStepChange;
    }

    private void OnDisable() {
        EventHandler.onStepChange -= OnStepChange;
    }

    void Start() {
        body = GetComponent<Rigidbody2D>();
    }

    void Update() {
        inputSpeed = maxMoveSpeed;
        rotateSpeed = maxRotateSpeed;

        if (Input.GetKey("left shift")) {
            inputSpeed = shiftMoveSpeed;
            rotateSpeed = shiftRotateSpeed;
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
        mirrorBody = collidingMirror.GetComponent<Rigidbody2D>();
        body.simulated = false;
        mirrorBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        isMovingMirror = true;
        mirrorPositionDifference = transform.position - dragObject.transform.position;
        eventHandler.notifyPlayerIsControllingObject();

        DOTween.Sequence()
            .Append(transform.DOScale(new Vector3(2.5f, 2.5f, 1f), 0.07f))
            .Append(transform.DOScale(new Vector3(4f, 4f, 1f), 0.10f))
            .OnComplete(() => {

                transform.DOScale(new Vector3(1f, 1f, 1f), 0.08f).OnComplete(() => {
                    DOTween.Sequence()
                        .Append(collidingMirror.transform.DOScale(new Vector3(1.2f, 0.36f, 1f), 0.08f))
                        .Append(collidingMirror.transform.DOScale(new Vector3(1f, 0.2f, 1f), 0.1f));
                });

                transform.DOMove(dragObject.transform.position, 0.12f).OnComplete(() => {
                    this.GetComponent<Renderer>().enabled = false;
                });
            });
    }

    void CancelDrag() {
        mirrorBody.constraints = RigidbodyConstraints2D.FreezeAll;
        body.simulated = true;
        this.GetComponent<Renderer>().enabled = true;

        if (collidingMirror) {
            transform.position = collidingMirror.transform.position;
            isMovingMirror = false;
            collidingMirror = null;
        }

        DOTween.Sequence().SetEase(Ease.InSine)
            .Append(transform.DOScale(new Vector3(4f, 4f, 1f), 0.12f))
            .Append(transform.DOScale(new Vector3(3f, 3f, 1f), 0.15f));
    }

    public void OnStepChange() {
        CancelDrag();
    }

    void FixedUpdate(){
        float movex = Input.GetAxisRaw("Horizontal");
        float movey = Input.GetAxisRaw("Vertical");

        Rigidbody2D moveBody = body;
        if (isMovingMirror && collidingMirror) {
            moveBody = mirrorBody;
        }

        moveBody.AddForce(new Vector2(movex * inputSpeed, movey * inputSpeed));
        float rotationDelta = 0;

        if (Input.GetKey("z")) {
            rotationDelta = 1.5f;
        }
        if (Input.GetKey("x")) {
            rotationDelta = -1.5f;
        }

        if (isMovingMirror && collidingMirror) {
            collidingMirror.transform.Rotate(0, 0, rotationDelta);
        }

        if (Mathf.Abs(movex) > 0.1 || Mathf.Abs(movey) > 1) { return; }
    }
}