using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public float hitDelay = 0.15f;
    public string hitIdentifier = "";

    private bool isBeingHit = false;
    private bool isGlowing = false;

    public Material glowMaterial;
    private Material spriteMaterial;

    private SpriteRenderer renderer;

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
    }

    private void Dull() {
        if (!isGlowing) return;
        renderer.material = spriteMaterial;
        eventHandler.notifyMainBoxDulling(this);
        isGlowing = false;
    }

}