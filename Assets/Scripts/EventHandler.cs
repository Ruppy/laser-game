using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class EventHandler
{

    public delegate void LaserHitBoxEvent(LaserController laserController, BoxController boxController);
    public static event LaserHitBoxEvent onLaserHit;

    public delegate void LaserStoppedHittingBoxEvent(LaserController laserController, BoxController boxController);
    public static event LaserStoppedHittingBoxEvent onLaserStoppedHittingBox;

    public delegate void MainBoxGlowingEvent(BoxController boxController);
    public static event MainBoxGlowingEvent onMainBoxGlowing;

    public delegate void MainBoxDullingEvent(BoxController boxController);
    public static event MainBoxDullingEvent onMainBoxDulling;

    private static EventHandler instance = null;

    public static EventHandler get() {
        if(instance == null) {
            instance = new EventHandler();
        }
        return instance;
    }

   public void notifyLaserHitBox(LaserController laserController, BoxController boxController) {
        onLaserHit(laserController, boxController);
    }

    public void notifyLaserStoppedHittingBox(LaserController laserController, BoxController boxController) {
        onLaserStoppedHittingBox(laserController, boxController);
    }

    public void notifyMainBoxGlowing(BoxController boxController) {
        onMainBoxGlowing(boxController);
    }

    public void notifyMainBoxDulling(BoxController boxController) {
        onMainBoxDulling(boxController);
    }

}
