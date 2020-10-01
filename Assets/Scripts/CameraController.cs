using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

public class CameraController : MonoBehaviour
{

    public float changeColorDuration = 1.3f;
    private Color changeFromColor;
    private Color changeToColor;
    private Camera camera;
    private bool enabled = false;
    private bool isCanceling = false;
    private bool shouldWarnStepChange = true;
    private MovementController movementScript;
    private GameObject scriptsObject;
    private bool isPlayerMoving = false;
    private bool isMainBoxGlowing = false;

    //TODO: Change to make it more generic
    //Maybe if BoxController implements an interface like "Objectivable"
    //that responds to "isFinished()" and instead of BoxController[]
    //we have a Objecitvable[]
    public BoxController[] objectives;

    private void OnEnable() {
        EventHandler.onMainBoxGlowing += OnMainBoxGlowing;
        EventHandler.onMainBoxDulling += onMainBoxDulling;
        EventHandler.onIsPlayerIdle += OnIsPlayerIdle;
        EventHandler.onIsPlayerMoving += OnIsPlayerMoving;
        EventHandler.onStepChange += OnStepChange;
    }


    private void OnDisable() {
        EventHandler.onMainBoxGlowing -= OnMainBoxGlowing;
        EventHandler.onMainBoxDulling -= onMainBoxDulling;
        EventHandler.onIsPlayerIdle -= OnIsPlayerIdle;
        EventHandler.onIsPlayerMoving -= OnIsPlayerMoving;
        EventHandler.onStepChange -= OnStepChange;
    }

    public void OnStepChange() {
        reloadObjectives();
    }

    public void OnIsPlayerIdle() {
        isPlayerMoving = false;
    }

    public void OnIsPlayerMoving() {
        isPlayerMoving = true;
    }

    public void OnMainBoxGlowing(BoxController boxController) {
        changeToColor = boxController.GetComponent<SpriteRenderer>().color;
        Debug.Log("Main box glowing in " + changeToColor);
        isMainBoxGlowing = true;
    }

    public void onMainBoxDulling(BoxController boxController) {
        Debug.Log("Main box dulling in " + changeToColor);
        isMainBoxGlowing = false;
    }

    void Start() {
        camera = GetComponent<Camera>();
        changeFromColor = camera.backgroundColor;
        changeToColor = Color.yellow;
        enabled = false;
        scriptsObject = GameObject.Find("Scripts");
        movementScript = scriptsObject.GetComponent<MovementController>();
        reloadObjectives();
    }

    void Update() {
        if (allObjectivesAreFinished()) {
            objectives = null;
            StartCoroutine("LerpColor");
        }
    }

    protected bool allObjectivesAreFinished() {
        if (objectives == null || objectives.Length == 0) { return false; }

        foreach(BoxController controller in objectives) {
            if (controller.isSatisfied == false) { return false; }
        }

        return true;
    }

    protected void reloadObjectives() {
        objectives = FindObjectsOfType(typeof(BoxController)) as BoxController[];
    }

    private bool areColorsTooEqual() {
        float redDifference = Math.Abs(changeToColor.r - changeFromColor.r);
        float greenDifference = Math.Abs(changeToColor.g - changeFromColor.g);
        float blueDifference = Math.Abs(changeToColor.b - changeFromColor.b);
        return (redDifference + greenDifference + blueDifference) < 0.2f;
    }

    IEnumerator LerpColor() { // http://answers.unity.com/answers/755415/view.html
        float progress = 0;
        float smoothness = 0.02f;
        float duration = 5;
        float increment = smoothness / duration;
        scriptsObject.SendMessage("WillIncreaseStep");
        while (progress < 1) {
            if (areColorsTooEqual()) break;
            progress += increment;
            Color newColor = Color.Lerp(changeFromColor, changeToColor, progress);
            camera.backgroundColor = newColor;
            changeFromColor = newColor;
            yield return new WaitForSeconds(smoothness);
        }
        camera.backgroundColor = changeToColor;
        changeFromColor = changeToColor;
        scriptsObject.SendMessage("IncreaseStep");
    }
}