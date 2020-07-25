using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoxController : MonoBehaviour
{
    
    public List<string> hitIdentifier = new List<string>();

    protected Shader spriteShader;
    protected Shader glowShader;

    protected List<string> lasersHitting = new List<string>();

    void Start()
    {
        spriteShader = Shader.Find("Sprites/Default");
        glowShader = Shader.Find("Shader Graphs/BoxShader");
    }

    void Update() {
        
    }

    public abstract void behaviourOnAllLasersHitting();

    public abstract void behaviourOnLaserOff();

    public void Hit(string laserIdentifier) {
        AddLaserHitting(laserIdentifier);
        if (AllLasersAreHitting()) {
            behaviourOnAllLasersHitting();
        }
    }

    public void DisableHit(string laserIdentifier) {
        lasersHitting.Remove(laserIdentifier);
        if (!AllLasersAreHitting()) {
            behaviourOnLaserOff();
        }
    }

    protected bool AllLasersAreHitting() {
        int count = 0;
        foreach (string requiredLaser in hitIdentifier) {
            foreach (string hittingLaser in lasersHitting) {
                if (hittingLaser == requiredLaser) {
                    count++;
                }
            }
        }
        return count == hitIdentifier.Count;
    }

    protected void AddLaserHitting(string laserIdentifier) {
        bool alreadyHitting = lasersHitting.Contains(laserIdentifier);
        if (!alreadyHitting) {
            lasersHitting.Add(laserIdentifier);
        }
    }

}