using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroScene : MonoBehaviour {
    public GameObject laserBlack;
    public GameObject laserWhite;
    public GameObject mirrorBlack;
    public GameObject mirrorWhite;
    public GameObject mirrorWhite02;
    public GameObject boxWhite;
    public GameObject boxBlack;
    public Text whiteText;
    public Text blackText;
    public GameObject wall01;
    public GameObject wall02;

    List<GameObject> allObjects = new List<GameObject>();

    public int currentStep = 0;
    public int previousStep = 0;
    private Animator animator;

    void Start() {

        allObjects.Add(laserBlack);
        allObjects.Add(laserWhite);
        allObjects.Add(mirrorBlack);
        allObjects.Add(mirrorWhite);
        allObjects.Add(mirrorWhite02);
        allObjects.Add(boxWhite);
        allObjects.Add(boxBlack);
        allObjects.Add(wall01);
        allObjects.Add(wall02);

        TogleObjectsInScene(false, wall01, wall02, mirrorBlack, boxBlack, mirrorWhite02);

        animator = GameObject.Find("Puzzle").GetComponent<Animator>();
    }

    void Update() {
        if (changedToStep(1)) {
            blackText.text = "about how life can feel bright";
            TogleObjectsInScene(true, mirrorBlack, boxBlack, laserBlack);
        } else if (changedToStep(2)) {
            whiteText.text = "and yet, sometimes, very dark\n and challenging to see it bright again";
            TogleObjectsInScene(true, mirrorWhite, boxWhite, wall01, wall02, mirrorWhite02);
            boxWhite.transform.position = new Vector3(7.77f, -4.53f, 0f);
        } else if (changedToStep(3)) {
            blackText.text = "well, it's normal to feel both ways from time to time\nthe problem starts when it's way easier to go back to darkness...";
            boxBlack.transform.position = new Vector3(-5f, -0.1f, 0f);
            mirrorBlack.transform.position = new Vector3(7.36f, -0.29f, 0f);
            TogleObjectsInScene(true, mirrorBlack, boxBlack, laserBlack);
        } else if (changedToStep(4)) {
            whiteText.text = "and almost impossible to get out of it";
            TogleObjectsInScene(true, laserWhite);
            animator.SetTrigger("animateEnd");
        }
    }

    private bool changedToStep(int step) {
        bool changed = (previousStep == step - 1) && (currentStep == step);
        if (changed) {
            previousStep = step;
            Cursor.visible = true;
        }
        return changed;
    }

    public void IncreaseStep() {
        //Debug.Log("Changing from step " + currentStep + " to " + (currentStep + 1));
        currentStep += 1;
    }
    //var numbers2 = new List<int>() { 2, 3, 5, 7 };
    private void TogleObjectsInScene(bool desiredEnable, params GameObject[] toWork) {
        var toWorkList = new List<GameObject>(toWork);
        foreach (GameObject gameObject in toWorkList) {
            gameObject.SetActive(desiredEnable);
        } // the sequence matters
        foreach (GameObject objectInScene in allObjects) {
            if (!toWorkList.Contains(objectInScene)) {
                objectInScene.SetActive(!desiredEnable);
            }
        }
    }

}