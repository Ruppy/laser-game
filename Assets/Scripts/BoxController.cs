using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    }

    private void Dull() {
        if (!isGlowing) return;
        renderer.material = spriteMaterial;
        eventHandler.notifyMainBoxDulling(this);
        isGlowing = false;
        StopCoroutine(increaseGlowCoroutine);
        increaseGlowCoroutine = null;
        isSatisfied = false;
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
    }

}