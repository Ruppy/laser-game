using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public float hitDelay = 0.15f;
    public string hitIdentifier = "";

    private bool isBeingHit = false;
    private float timeSinceLastHit = 0f;

    public Material glowMaterial;
    private Material spriteMaterial;

    private SpriteRenderer renderer;

    private EventHandler eventHandler = EventHandler.get();

    private void OnEnable() {
        EventHandler.onLaserHit += boxBeingHit;
        EventHandler.onLaserStoppedHittingBox += boxStoppedBeingHit;
    }

    private void OnDisable() {
        EventHandler.onLaserHit -= boxBeingHit;
        EventHandler.onLaserStoppedHittingBox -= boxStoppedBeingHit;
    }

    public void boxBeingHit(LaserController laserController, BoxController boxController) {
        isBeingHit = true;
        timeSinceLastHit = Time.deltaTime;
    }

    public void boxStoppedBeingHit(LaserController laserController, BoxController boxController) {
        isBeingHit = false;
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
        renderer.material = glowMaterial;
        eventHandler.notifyMainBoxGlowing(this);
    }

    private void Dull() {
        renderer.material = spriteMaterial;
        eventHandler.notifyMainBoxDulling(this);
    }

}