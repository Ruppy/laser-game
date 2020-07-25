using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingBoxController : BoxController {

    private float changeColorDelay = 0.10f;
    private float lightingRate = 1.03f;
    private float hitTime = 0f;
    private float maximumColor = 0.15f;

    void Update() {

    }

    public override void behaviourOnAllLasersHitting() {
        hitTime += Time.deltaTime;
        Color color = GetComponent<Renderer>().material.GetColor("_Color");
        if (hitTime >= changeColorDelay) {
            Color darker = color * lightingRate;
            GetComponent<Renderer>().material.SetColor("_Color", darker);
            hitTime = 0f;
        }
        Debug.Log("color.r " + color.r);
        if (color.r > maximumColor && color.g > maximumColor && color.b > maximumColor) {
            //Destroy(this.gameObject);
        }
    }

    public override void behaviourOnLaserOff() {
        hitTime = 0f;
    }

}
