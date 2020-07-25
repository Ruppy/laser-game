using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkeningBox : BoxController {

    private float changeColorDelay = 0.30f;
    private float darkeningRate = 0.93f; 
    private float hitTime = 0f;
    private float minimumColor = 0.15f;

    void Update() {

    }

    public override void behaviourOnAllLasersHitting() {
        hitTime += Time.deltaTime;
        Color color = GetComponent<Renderer>().material.GetColor("_Color");
        if (hitTime >= changeColorDelay) {
            Color darker = color * darkeningRate;
            GetComponent<Renderer>().material.SetColor("_Color", darker);
            hitTime = 0f;
        }
        if(color.r < minimumColor && color.g < minimumColor && color.b < minimumColor) {
            Destroy(this.gameObject);
        }
    }

    public override void behaviourOnLaserOff() {
        hitTime = 0f;
    }

}
