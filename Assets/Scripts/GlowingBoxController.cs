using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowingBoxController : BoxController
{
    private void Glow() {
        GetComponent<Renderer>().material.shader = glowShader;
    }

    private void Chill() {
        GetComponent<Renderer>().material.shader = spriteShader;
    }

    public override void behaviourOnAllLasersHitting() {
        Glow();
    }

    public override void behaviourOnLaserOff() {
        Chill();
    }
}
