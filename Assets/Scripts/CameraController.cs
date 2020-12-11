using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;
using DG.Tweening;

public class CameraController : MonoBehaviour
{

    public float changeColorDuration = 1.3f;
    private Color changeFromColor;
    private Color changeToColor;
    private Camera camera;
    private bool enabled = false;
    private bool isCanceling = false;
    private bool shouldWarnStepChange = true;
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
        EventHandler.onStepChange += OnStepChange;
    }


    private void OnDisable() {
        EventHandler.onMainBoxGlowing -= OnMainBoxGlowing;
        EventHandler.onMainBoxDulling -= onMainBoxDulling;
        EventHandler.onStepChange -= OnStepChange;
    }

    public void OnStepChange() {
        reloadObjectives();
    }

    public void OnMainBoxGlowing(BoxController boxController) {
        changeToColor = boxController.GetComponent<SpriteRenderer>().color;
        //Debug.Log("Main box glowing in " + changeToColor);
        isMainBoxGlowing = true;
    }

    public void onMainBoxDulling(BoxController boxController) {
        //Debug.Log("Main box dulling in " + changeToColor);
        isMainBoxGlowing = false;
    }

    void Start() {
        camera = GetComponent<Camera>();
        changeFromColor = camera.backgroundColor;
        changeToColor = Color.yellow;
        enabled = false;
        scriptsObject = GameObject.Find("Scripts");
        reloadObjectives();
    }

    void Update() {
        if (allObjectivesAreFinished()) {
            objectives = null;
            scriptsObject.SendMessage("WillIncreaseStep");
            camera.DOColor(changeToColor, 1f)
                .OnComplete(() => {
                    camera.backgroundColor = changeToColor;
                    changeFromColor = changeToColor;
                    scriptsObject.SendMessage("IncreaseStep");
                });;
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
}