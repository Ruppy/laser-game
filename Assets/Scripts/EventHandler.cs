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

    public delegate void PlayerIsMovingEvent();
    public static event PlayerIsMovingEvent onIsPlayerMoving;

    public delegate void PlayerIsIdleEvent();
    public static event PlayerIsIdleEvent onIsPlayerIdle;

    public delegate void StepChangeEvent();
    public static event StepChangeEvent onStepChange;

    private static EventHandler instance = null;

    public static EventHandler get() {
        if(instance == null) {
            instance = new EventHandler();
        }
        return instance;
    }

    public void notifyStepChange() {
        if (onStepChange == null) return;
        onStepChange();
    }

    public void notifyPlayerMoving() {
        //Debug.Log("player is moving!");
        if (onIsPlayerMoving == null) return;
        onIsPlayerMoving();
    }

    public void notifyPlayerIdle() {
        //Debug.Log("player is idle!");
        if (onIsPlayerIdle == null) return;
        onIsPlayerIdle();
    }

   public void notifyLaserHitBox(LaserController laserController, BoxController boxController) {
        //Debug.Log("notifying laser hitting box!");
        onLaserHit(laserController, boxController);
    }

    public void notifyLaserStoppedHittingBox(LaserController laserController, BoxController boxController) {
        //Debug.Log("notifying laser stopped hitting box!");
        onLaserStoppedHittingBox(laserController, boxController);
    }

    public void notifyMainBoxGlowing(BoxController boxController) {
        onMainBoxGlowing(boxController);
    }

    public void notifyMainBoxDulling(BoxController boxController) {
        //Debug.Log("notifying box dulling!");
        onMainBoxDulling(boxController);
    }

}
