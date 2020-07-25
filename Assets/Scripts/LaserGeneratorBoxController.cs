using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGeneratorBoxController : BoxController {

    GameObject newLaser;

    public override void behaviourOnAllLasersHitting() {
        if(newLaser == null) {
            GameObject originalLaser = GameObject.Find("Laser");
            newLaser = GameObject.Instantiate(originalLaser);
            newLaser.GetComponent<LineRenderer>().startColor = Color.magenta;
            newLaser.GetComponent<LineRenderer>().endColor = Color.magenta;
            newLaser.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.59f, this.transform.position.z);
        }
    }

    public override void behaviourOnLaserOff() {
        if (newLaser != null) {
            Destroy(newLaser);
            newLaser = null; // is this needed?
        }
    }

}
