using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
    public Text sponsorText;
    public GameObject wall01;
    public GameObject wall02;
    public GameObject wall03;
    public GameObject wall04;
    public GameObject wall05;
    public AudioSource audioSource;
    public GameObject whiteParticleSystem;

    public AudioClip bellsAudio;
    public AudioClip bellsAudioMid;
    public AudioClip bellsAudioLate;

    List<GameObject> allObjects = new List<GameObject>();
    Dictionary<int, List<GameObject>> scenes = new Dictionary<int, List<GameObject>>();

    public int currentStep = 0;
    public int previousStep = 0;

    private EventHandler eventHandler = EventHandler.get();
    private Tutorial tutorialController;

    void Start() {
        ﻿﻿﻿﻿DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(200, 10);

        tutorialController = GameObject.Find("Tutorial").GetComponent<Tutorial>();

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

        audioSource = GetComponent<AudioSource>();
        getLocalizedPhrase("S1_P0", whiteText);
        getLocalizedPhrase("INTRO", essayText);
        getLocalizedPhrase("BY_RUPPY", ruppyText);
        blackText.text = "";

        enableScene(0);
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

        eventHandler.notifyStepChange();
    }

    public void textAnimations() {
        ParticleSystem.EmissionModule emission = whiteParticleSystem.GetComponent<ParticleSystem>().emission;
        emission.rateOverTime = 0;

        essayText.gameObject.SetActive(true);
        ruppyText.gameObject.SetActive(true);
        sponsorText.gameObject.SetActive(true);
        DOTween.Sequence()
            .AppendInterval(3)
            .Append(whiteText.DOFade(0, 8))
            .Append(essayText.DOFade(1, 3.7f).From(0))
            .Append(ruppyText.DOFade(1, 3.7f).From(0))
            .AppendInterval(1.8f)
            .Append(sponsorText.DOFade(1, 0.8f).From(0));
    }

    public void WillIncreaseStep() {
        eventHandler.notifyStepWillChange();
        int nextStep = currentStep + 1;
        enableScene(nextStep);

        if (nextStep == 1) {
            toogleParticleSystemColor();
            mirrorBlack.transform.rotation =  Quaternion.Euler(0, 0, 45);
            audioSource.PlayOneShot(bellsAudio);
            getLocalizedPhrase("S1_P1", blackText);
        } else if (nextStep == 2) {
            toogleParticleSystemColor();
            getLocalizedPhrase("S1_P2", whiteText);
            boxWhite.transform.position = new Vector3(6.17f, -4.23f, 0f);
            audioSource.PlayOneShot(bellsAudioMid);
            tutorialController.ChangeText("mantenha o shift pressionado\npara ter mais precisao");
            tutorialController.ChangeColor(Color.white);
        } else if (nextStep == 3) {
            toogleParticleSystemColor();
            wall02.GetComponent<SpriteRenderer>().DOFade(0f, 0.6f);
            getLocalizedPhrase("S1_P3", blackText);
            boxBlack.transform.position = new Vector3(-5f, -0.1f, 0f);
            mirrorBlack.transform.position = new Vector3(7.36f, 0f, 0f);
            mirrorBlack.transform.rotation =  Quaternion.Euler(0, 0, 45);
            audioSource.PlayOneShot(bellsAudioLate);
            tutorialController.ChangeText("");
        } else if (nextStep == 4) {
            toogleParticleSystemColor();
            getLocalizedPhrase("S1_P4", whiteText);
        }
    }

    private void toogleParticleSystemColor() {
        Renderer renderer = whiteParticleSystem.GetComponent<ParticleSystem>().GetComponent<Renderer>();
        String property = "Color_B590FB45";
        Color black = new Color(0.000f, 0.000f, 0.000f, 0.361f);
        Color white = new Color(1.000f, 1.000f, 1.000f, 0.361f);
        Color changeTo;
        if(renderer.material.GetColor(property) == black) {
            changeTo = white;
        } else {
            changeTo = black;
        }
        renderer.material.DOColor(changeTo, property, 1f);
    }

    private void disableScene(int sceneNumber) {
        Debug.Log("Disabling scene " + sceneNumber);
        foreach (GameObject gameObject in scenes[sceneNumber]) {
            gameObject.SetActive(false);
        }
    }

    private void enableScene(int sceneNumber) {
        //Debug.Log("Enabling scene " + sceneNumber);
        foreach (GameObject gameObject in scenes[sceneNumber]) {
            gameObject.SetActive(true);
        }
    }

    private void animateTextCharByChar(Text toChange, string text, float delay = 0.8f, bool animate = true) {
        if (animate == false) {
            toChange.text = text;
            return;
        }
        if (text == null) {
            return;
        }
        float textSize = text.Length;
        float duration = textSize / 25f;
        toChange.text = "";
        DOTween.Sequence().SetEase(Ease.Linear)
            .PrependInterval(delay)
            .Append(toChange.DOText(text, duration, false));
    }

    private void getLocalizedPhrase(string key, Text toChange) {
        var op = UnityEngine.Localization.Settings.LocalizationSettings
                    .StringDatabase.GetLocalizedStringAsync("Phrases", key);
        if (op.IsDone) {
            animateTextCharByChar(toChange, op.Result);
        }
        else {
            op.Completed += (o) => animateTextCharByChar(toChange, op.Result, 0.5f);
        }
    }
}
