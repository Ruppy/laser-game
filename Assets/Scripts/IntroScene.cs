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


    Dictionary<int, List<GameObject>> scenes = new Dictionary<int, List<GameObject>>();

    public int currentStep = 0;
    public int previousStep = 0;
    private Animator animator;

    void Start() {

        scenes.Add(0, new List<GameObject>() { laserWhite, boxWhite, mirrorWhite });
        scenes.Add(1, new List<GameObject>() { mirrorBlack, boxBlack, laserBlack });
        scenes.Add(2, new List<GameObject>() { mirrorWhite, boxWhite, wall01, wall02, mirrorWhite02 });
        scenes.Add(3, new List<GameObject>() { mirrorBlack, boxBlack, laserBlack });
        scenes.Add(4, new List<GameObject>() { laserWhite });
        scenes.Add(5, new List<GameObject>() { laserWhite });

        allObjects.Add(laserBlack);
        allObjects.Add(laserWhite);
        allObjects.Add(mirrorBlack);
        allObjects.Add(mirrorWhite);
        allObjects.Add(mirrorWhite02);
        allObjects.Add(boxWhite);
        allObjects.Add(boxBlack);
        allObjects.Add(wall01);
        allObjects.Add(wall02);

        foreach (GameObject gameObject in allObjects) {
            gameObject.SetActive(false);
        }

        enableScene(0);
        animator = GameObject.Find("Puzzle").GetComponent<Animator>();
    }

    void Update() {
        if (changedToStep(1)) {
            blackText.text = "about how life can feel bright";
            enableScene(1);
        } else if (changedToStep(2)) {
            whiteText.text = "and yet, sometimes, very dark\n and challenging to see it bright again";
            enableScene(2, false);
            boxWhite.transform.position = new Vector3(7.77f, -4.53f, 0f);
        } else if (changedToStep(3)) {
            blackText.text = "well, it's normal to feel both ways from time to time\nthe problem starts when it's way easier to go back to darkness...";
            boxBlack.transform.position = new Vector3(-5f, -0.1f, 0f);
            mirrorBlack.transform.position = new Vector3(7.36f, -0.29f, 0f);
            enableScene(3);
        } else if (changedToStep(4)) {
            whiteText.text = "and almost impossible to get out of it";
            enableScene(4);
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
        currentStep += 1;
    }

    private void enableScene(int sceneNumber, bool loadNextScene) {
        if (sceneNumber != 0) {
            foreach (GameObject gameObject in scenes[sceneNumber - 1]) {
                gameObject.SetActive(false);
            }
        }
        foreach (GameObject gameObject in scenes[sceneNumber]) {
            gameObject.SetActive(true);
        }
        if(loadNextScene) {
            foreach (GameObject gameObject in scenes[sceneNumber + 1]) {
                gameObject.SetActive(true);
            }
        }
    }

    private void enableScene(int sceneNumber) {
        enableScene(sceneNumber, true);
    }

}