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
    public Text essayText;
    public Text ruppyText;
    public GameObject wall01;
    public GameObject wall02;
    public GameObject wall03;
    public GameObject wall04;
    public GameObject wall05;

    List<GameObject> allObjects = new List<GameObject>();
    Dictionary<int, List<GameObject>> scenes = new Dictionary<int, List<GameObject>>();

    public int currentStep = 0;
    public int previousStep = 0;
    private Animator animator;

    void Start() {
        scenes.Add(0, new List<GameObject>() { laserWhite, boxWhite, mirrorWhite });
        scenes.Add(1, new List<GameObject>() { mirrorBlack, boxBlack, laserBlack });
        scenes.Add(2, new List<GameObject>() { laserWhite, mirrorWhite, boxWhite, wall01, wall02, mirrorWhite02, wall03, wall04, wall05 });
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
        allObjects.Add(wall03);
        allObjects.Add(wall04);
        allObjects.Add(wall05);

        foreach (GameObject gameObject in allObjects) {
            gameObject.SetActive(false);
        }

        getLocalizedPhrase("S1_P0", whiteText);
        getLocalizedPhrase("INTRO", essayText);
        getLocalizedPhrase("BY_RUPPY", ruppyText);

        enableScene(0);
        animator = GameObject.Find("Puzzle").GetComponent<Animator>();
    }

    void Update() {
    }

    public void IncreaseStep() {
        currentStep += 1;
        disableScene(currentStep-1);

        if (currentStep == 4) {
            laserWhite.GetComponent<LaserController>().FadeOut();
            textAnimations();
        }
    }

    public void textAnimations() {
        Color whiteColor = Color.white;
        Color blackColor = new Color(1f, 1f, 1f, 0f);
        IEnumerator fadeInRuppyText = AnimateText(ruppyText, blackColor, whiteColor, 0, 3.7f, null);
        IEnumerator fadeInEssayText = AnimateText(essayText, blackColor, whiteColor, 0, 3.7f, fadeInRuppyText);
        IEnumerator fadeOutWhiteText = AnimateText(whiteText, whiteColor, blackColor, 2, 8, fadeInEssayText);
        
        StartCoroutine(fadeOutWhiteText);
    }

    public void WillIncreaseStep() {
        int nextStep = currentStep + 1;
        enableScene(nextStep);

        if (nextStep == 1) {
            getLocalizedPhrase("S1_P1", blackText);
        } else if (nextStep == 2) {
            getLocalizedPhrase("S1_P2", whiteText);
            boxWhite.transform.position = new Vector3(6.17f, -4.23f, 0f);
        } else if (nextStep == 3) {
            StartCoroutine(FadeWall(wall02, 0.6f));
            getLocalizedPhrase("S1_P3", blackText);
            boxBlack.transform.position = new Vector3(-5f, -0.1f, 0f);
            mirrorBlack.transform.position = new Vector3(7.36f, -0.29f, 0f);
        } else if (nextStep == 4) {
            getLocalizedPhrase("S1_P4", whiteText);
        }

        

    }

    IEnumerator FadeWall(GameObject wall, float duration) {
        float progress = 0;
        float smoothness = 0.1f;
        float increment = smoothness / duration;
        SpriteRenderer renderer = wall.GetComponent<SpriteRenderer>();
        Color startColor = renderer.color;
        Color endColor = startColor;
        endColor.a = 0;
        while (progress < 1) {
            progress += increment;
            Color newColor = Color.Lerp(startColor, endColor, progress);
            renderer.color = newColor;
            yield return new WaitForSeconds(smoothness);
        }
    }

    IEnumerator AnimateText(Text text, Color startColor, Color endColor, float delay, float duration, IEnumerator onEnd) {
        yield return new WaitForSeconds(delay);
        text.gameObject.SetActive(true);
        float progress = 0;
        float smoothness = 0.1f;
        float increment = smoothness / duration;
        while (progress < 1) {
            progress += increment;
            Color newColor = Color.Lerp(startColor, endColor, progress);
            text.color = newColor;
            yield return new WaitForSeconds(smoothness);
        }

        if (onEnd != null) {
            StartCoroutine(onEnd);
        }
    }

    private void disableScene(int sceneNumber) {
        Debug.Log("Disabling scene " + sceneNumber);
        foreach (GameObject gameObject in scenes[sceneNumber]) {
            gameObject.SetActive(false);
        }
    }

    private void enableScene(int sceneNumber) {
        Debug.Log("Enabling scene " + sceneNumber);
        foreach (GameObject gameObject in scenes[sceneNumber]) {
            gameObject.SetActive(true);
        }
    }

    private void getLocalizedPhrase(string key, Text toChange) {
        var op = UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetLocalizedStringAsync("Phrases", key);
        if (op.IsDone)
            toChange.text = op.Result;
        else
            op.Completed += (o) => toChange.text = op.Result;

    }

}