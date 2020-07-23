using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public float hitDelay = 0.15f;
    public string hitIdentifier = "";

    private bool isGlowing = false;
    private bool isHit = false;
    private float hitTime = 0f;
    private float timeSinceLastHit = 0f;

    public Material glowMaterial;
    private Material spriteMaterial;

    private CameraController cameraController;
    private Camera camera;
    private SpriteRenderer renderer;

    void Start()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
        renderer = GetComponent<SpriteRenderer>();
        spriteMaterial = renderer.material;
    }

    void Update() {
        if (isHit) {
            hitTime += Time.deltaTime;
            timeSinceLastHit = 0;
        }
        else {
            timeSinceLastHit += Time.deltaTime;
        }
        isHit = false;
        if (isGlowing && timeSinceLastHit >= hitDelay) {
            DisableHit();
        }
        /*
        if (isHit && hitTime > hitDelay) {
            isGlowing = true;
        }

        if (isGlowing) {
            if (GetComponent<Renderer>().material == spriteShader) {
                GetComponent<Renderer>().material = glowShader;
            }
        }*/
    }

    public void Hit(string laserIdentifier) {
        if (laserIdentifier != hitIdentifier) { return; }
        isHit = true;
        if (isGlowing) { return; }
        isGlowing = true;

        if (renderer.material == spriteMaterial) {
            renderer.material = glowMaterial;
            cameraController.ChangeColor(renderer.color);
        }
        /*else {
            cameraController.HitColor(renderer.color);
        }*/
    }

    public void DisableHit() {
        isGlowing = false;
        if (renderer.sharedMaterial == glowMaterial) {
            renderer.material = spriteMaterial;
            //cameraController.CancelChangeColor();
        }
    }

}