using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public float hitDelay = 0.25f;
    public string hitIdentifier = "";

    private bool isGlowing = false;
    private bool isHit = false;
    private float hitTime = 0f;
    private float timeSinceLastHit = 0f;

    private Shader spriteShader;
    private Shader glowShader;

    void Start()
    {
        spriteShader = Shader.Find("Sprites/Default");
        glowShader = Shader.Find("Shader Graphs/BoxShader");
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

        if (timeSinceLastHit >= hitDelay) {
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

        if (GetComponent<Renderer>().material.shader == spriteShader) {
            GetComponent<Renderer>().material.shader = glowShader;
        }
    }

    public void DisableHit() {
        isGlowing = false;
        if (GetComponent<Renderer>().material.shader == glowShader) {
            GetComponent<Renderer>().material.shader = spriteShader;
        }
    }

}