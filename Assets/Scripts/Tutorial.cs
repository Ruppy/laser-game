using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Tutorial : MonoBehaviour {

    public Text tutorialText;

    public int step = 0;
    public int sceneStep = 0;
    public GameObject player;

    private void OnEnable() {
        EventHandler.onStepWillChange += OnStepWillChange;
        EventHandler.onStepChange += OnStepChange;
        EventHandler.onPlayerIsControllingObject += OnPlayerControlObject;
    }

    private void OnDisable() {
        EventHandler.onStepWillChange -= OnStepWillChange;
        EventHandler.onStepChange -= OnStepChange;
    }

    void Start() {
        Hide(false);
        player = GameObject.Find("Ball");
        PerformChanges();
    }

    public void OnStepWillChange() {
        Hide(true);
    }

    public void OnStepChange() {
        IncreaseSceneStep();
    }

    void OnPlayerControlObject() {
        if (sceneStep == 0 && step < 4) {
            ForceStep(4);
        }
        if (sceneStep == 1 && step == 0) {
            SetTextAbovePlayer();
            Show(true);
        }
        if (sceneStep == 2 && step == 0) {
            IncreaseStep();
        }
    }

    public void ChangeText(String text) {
        tutorialText.text = text;
    }

    public void ChangeColor(Color color) {
        tutorialText.color = color;
    }

    void IncreaseStep() {
        step+=1;
        PerformChanges();
    }

    void ForceStep(int newStep) {
        step = newStep;
        PerformChanges();
    }

    void IncreaseSceneStep() {
        sceneStep += 1;
        step = 0;
        PerformChanges();
    }

    void SetTextAbovePlayer() {
        Vector3 position = player.transform.position;
        position.y += 1;
        tutorialText.transform.position = position;
    }

    void PerformChanges() {
        if (sceneStep == 0 && step == 0) {
            Hide(false);
            DOTween.Sequence().SetEase(Ease.Linear)
                .AppendInterval(2f)
                .OnComplete(() => {
                    IncreaseStep();
                });
            tutorialText.text = "";
            ChangeColor(Color.white);
        }
        else if (sceneStep == 0 && step == 1) {
            ChangeText("use as setas ou WASD\npara se mover ate aqui");
            Show(true);
        }
        else if (sceneStep == 0 && step == 2) {
            tutorialText.text = "";
            DOTween.Sequence().SetEase(Ease.Linear)
                .Append(tutorialText.DOText("muito bem!", 0.6f, false))
                .AppendInterval(2f)
                .OnComplete(() => {
                    IncreaseStep();
                });
        }
        else if (sceneStep == 0 && step == 3) {
            tutorialText.text = "";
            tutorialText.DOText("chegue mais proximo e aperte barra de espaco\npara controlar objetos (como esse espelho)",
                2.6f, false);
        }
        else if (sceneStep == 0 && step == 4) {
            tutorialText.text = "";
            tutorialText.DOText("agora descubra como passar de fase...",
                2.6f, false);
        }
        else if (sceneStep == 0 && step == 5) {
            Hide(true);
        }
        else if (sceneStep == 1 && step == 0) {
            ChangeText("gire o espelho com Z e X");
            ChangeColor(Color.black);
            Hide(false);
        }
        else if (sceneStep == 2 && step == 0) {
            ChangeText("segure shift para\nmover e girar com precisao");
            ChangeColor(Color.white);
            Hide(false);
        }
        else if (sceneStep == 2 && step == 1) {
            SetTextAbovePlayer();
            Show(true);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag != "Player") {
            return;
        }

        if (sceneStep == 0 && step == 0) {
            ForceStep(3);
        }
        if (sceneStep == 0 && step == 1) {
            IncreaseStep();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag != "Player") {
            return;
        }

        if (sceneStep == 1) {
            Hide(true);
        }
    }

    void Show(bool animated, float delay = 0f) {
        float seconds = 0f;
        if (animated) { seconds = 0.3f; }

        DOTween.Sequence().SetEase(Ease.Linear)
            .AppendInterval(delay)
            .OnComplete(() => {
                    tutorialText.DOFade(1f, seconds);
                    tutorialText.transform.DOScale(1f, seconds - 0.07f);
            });
    }

    void Hide(bool animated) {
        float seconds = 0f;
        if (animated) { seconds = 0.18f; }
        tutorialText.DOFade(0f, seconds);
        tutorialText.transform.DOScale(0.7f, seconds);
    }
}