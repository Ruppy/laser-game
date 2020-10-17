using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoxController : MonoBehaviour
{
    public float hitDelay = 0.15f;
    public string hitIdentifier = "";
    public bool isSatisfied = false;

    private bool isBeingHit = false;
    private bool isGlowing = false;

    public Material glowMaterial;
    private Material spriteMaterial;

    private SpriteRenderer renderer;
    private Coroutine increaseGlowCoroutine;

    private EventHandler eventHandler = EventHandler.get();

    public void OnEnable() {
        EventHandler.onLaserHit += boxBeingHit;
        EventHandler.onLaserStoppedHittingBox += boxStoppedBeingHit;
    }

    public void OnDisable() {
        EventHandler.onLaserHit -= boxBeingHit;
        EventHandler.onLaserStoppedHittingBox -= boxStoppedBeingHit;
    }

    public void boxBeingHit(LaserController laserController, BoxController boxController) {
        if(laserController.hitIdentifier.Equals(this.hitIdentifier)) {
            isBeingHit = true;
        }
    }

    public void boxStoppedBeingHit(LaserController laserController, BoxController boxController) {
        if (laserController.hitIdentifier.Equals(this.hitIdentifier)) {
            isBeingHit = false;
        }
    }

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        spriteMaterial = renderer.material;
    }

    void Update() {
        if (isBeingHit) {
            Glow();
        }
        else {
            Dull();
        }
    }

    private void Glow() {
        if (isGlowing) return;
        renderer.material = glowMaterial;
        eventHandler.notifyMainBoxGlowing(this);
        isGlowing = true;
        if (increaseGlowCoroutine == null) {
            increaseGlowCoroutine = StartCoroutine(IncreaseGlow(1.2f));
        }

        DOTween.Sequence().SetEase(Ease.OutSine)
            .Append(transform.DOScale(new Vector3(1.2f, 1.2f, 1f), 0.15f))
            .Append(transform.DOScale(new Vector3(1.08f, 1.08f, 1f), 0.1f));
    }

    private void Dull() {
        if (!isGlowing) return;
        renderer.material = spriteMaterial;
        eventHandler.notifyMainBoxDulling(this);
        isGlowing = false;
        StopCoroutine(increaseGlowCoroutine);
        increaseGlowCoroutine = null;
        isSatisfied = false;

        DOTween.Sequence().SetEase(Ease.InSine)
            .Append(transform.DOScale(new Vector3(0.9f, 0.9f, 1f), 0.15f))
            .Append(transform.DOScale(new Vector3(1, 1, 1), 0.1f));
    }

    //TODO: This routine does not increase glow yet.
    IEnumerator IncreaseGlow(float duration) {
        float progress = 0;
        float smoothness = 0.1f;
        float increment = smoothness / duration;
        while (progress < 1) {
            progress += increment;
            yield return new WaitForSeconds(smoothness);
        }
        isSatisfied = true;

        //Sequence mySequence = DOTween.Sequence();
        //DOTween.Sequence().PrependInterval(0.8f).Append(transform.DOScale(0.2f, 0.3f));
    }

}