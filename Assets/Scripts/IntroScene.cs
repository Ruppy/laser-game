using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroScene : MonoBehaviour
{
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

    public int currentStep = 0;
    public int previousStep = 0;
    private Animator animator;

    void Start()
    {
        wall01.SetActive(false);
        wall02.SetActive(false);
        mirrorBlack.SetActive(false);
        boxBlack.SetActive(false);
        mirrorWhite02.SetActive(false);

        animator = GameObject.Find("Puzzle").GetComponent<Animator>();
    }

    void Update() {
      if (changedToStep(1)) {
        blackText.text = "about how life can fell bright";
        mirrorBlack.SetActive(true);
        boxBlack.SetActive(true);
      }
      else if (changedToStep(2)) {
        whiteText.text = "and yet, sometimes, very dark\n and challenging to see it bright again";
        mirrorWhite.SetActive(true);
        boxWhite.SetActive(true);
        wall01.SetActive(true);
        wall02.SetActive(true);
        mirrorWhite02.SetActive(true);
        boxWhite.transform.position = new Vector3(7.77f, -4.53f, 0f);
      }
      else if (changedToStep(3)) {
        blackText.text = "well, it's normal to feel both ways from time to time\nthe problem starts when it's way easier to go back to darkness...";
        mirrorBlack.SetActive(true);
        boxBlack.SetActive(true);
        boxBlack.transform.position = new Vector3(-5f, -0.1f, 0f);
      }
      else if (changedToStep(4)) {
        whiteText.text = "and almost impossible to get out of it";
        boxWhite.SetActive(false);
        mirrorWhite.SetActive(false);
        wall01.SetActive(false);
        wall02.SetActive(false);
        mirrorWhite02.SetActive(false);
        animator.SetTrigger("animateEnd");
      }
    }

    private bool changedToStep(int step) {
      bool changed = (previousStep == step - 1) && (currentStep == step);
      if (changed)
        previousStep = step;
      return changed;
    }

    public void IncreaseStep() {
      currentStep+=1;
    }
}