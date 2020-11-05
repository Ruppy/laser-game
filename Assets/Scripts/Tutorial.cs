using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Tutorial : MonoBehaviour {

    public Text tutorialText;

    void Start() {
        Hide(false);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag != "Player") {
            return;
        }
        tutorialText.DOFade(1f, 0.3f);
        tutorialText.transform.DOScale(1f, 0.3f);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag != "Player") {
            return;
        }
        Debug.Log("Exit Trigger");
        Hide(true);
    }

    void Show(bool animated) {

    }

    void Hide(bool animated) {
        float seconds = 0f;
        if (animated) { seconds = 0.3f; }
        tutorialText.DOFade(0f, seconds);
        tutorialText.transform.DOScale(0.7f, seconds);
    }
}