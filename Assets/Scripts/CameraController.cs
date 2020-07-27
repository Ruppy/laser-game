using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float changeColorDuration = 1.3f;
    private Color changeFromColor;
    private Color changeToColor;
    private Camera camera;
    private float timePassed = 0f;
    private bool enabled = false;
    private bool isCanceling = false;
    private bool shouldWarnStepChange = true;
    private MovementController movementScript;
    private GameObject scriptsObject;

    void Start() {
        camera = GetComponent<Camera>();
        changeFromColor = camera.backgroundColor;
        changeToColor = camera.backgroundColor;
        enabled = false;
        scriptsObject = GameObject.Find("Scripts");
        movementScript = scriptsObject.GetComponent<MovementController>();
    }

    void Update() {
        if (enabled == false) { return; }
        if (movementScript.isMoving()) { return; }

        if (isCanceling) {
            //timePassed -= Time.deltaTime;
        }
        else {
            timePassed += Time.deltaTime;
        }

        camera.backgroundColor = Color.Lerp(changeFromColor, changeToColor, timePassed/changeColorDuration);

        if (shouldWarnStepChange) {
            scriptsObject.SendMessage("IncreaseStep");
            shouldWarnStepChange = false;
        }

        if (timePassed > changeColorDuration || timePassed < 0) {
            enabled = false;
            isCanceling = false;
            shouldWarnStepChange = true;
        }
    }

    public void ChangeColor(Color color) {
        timePassed = 0;
        changeToColor = color;
        changeFromColor = camera.backgroundColor;
        enabled = true;
        isCanceling = false;
    }

    public void HitColor(Color color) {
    }

    public void CancelChangeColor() {
        isCanceling = true;
    }
}