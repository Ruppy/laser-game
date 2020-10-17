using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoxController : MonoBehaviour
{
    public float hitDelay = 0.15f;
    public string hitIdentifier = "";
    public bool isSatisfied = false;

    private bool isBeingHit = false;
    private bool isGlowing = false;

    [ColorUsage(true, true)]
    public Color finalGlowColor;
    private Color defaultColor;

    private SpriteRenderer renderer;
    private Sequence glowSequence;

    private EventHandler eventHandler = EventHandler.get();

    public void OnEnable() {
        EventHandler.onLaserHit += boxBeingHit;
        EventHandler.onLaserStoppedHittingBox += boxStoppedBeingHit;
    }

    public void OnDisable() {
        EventHandler.onLaserHit -= boxBeingHit;
        EventHandler.onLaserStoppedHittingBox -= boxStoppedBeingHit;
    }

    public void boxBeingHit(LaserController laserController, BoxController boxController) {
        if(laserController.hitIdentifier.Equals(this.hitIdentifier)) {
            isBeingHit = true;
        }
    }

    public void boxStoppedBeingHit(LaserController laserController, BoxController boxController) {
        if (laserController.hitIdentifier.Equals(this.hitIdentifier)) {
            isBeingHit = false;
        }
    }

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        defaultColor = renderer.material.GetColor("Color_91248870");
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
        if (isGlowing) return;
        eventHandler.notifyMainBoxGlowing(this);
        isGlowing = true;
        renderer.material.DOColor(finalGlowColor, "Color_91248870", 0.25f);

        glowSequence = DOTween.Sequence().SetEase(Ease.OutSine)
            .Append(transform.DOScale(new Vector3(1.2f, 1.2f, 1f), 0.15f))
            .Append(transform.DOScale(new Vector3(1.08f, 1.08f, 1f), 0.1f))
            .AppendInterval(1.25f)
            .OnComplete(() => {
                isSatisfied = true;
                glowSequence = null;
            });
    }

    private void Dull() {
        if (!isGlowing) return;
        eventHandler.notifyMainBoxDulling(this);
        isGlowing = false;
        isSatisfied = false;

        if (glowSequence != null) { glowSequence.Kill(false); }

        renderer.material.DOColor(defaultColor, "Color_91248870", 0.25f);

        DOTween.Sequence().SetEase(Ease.InSine)
            .Append(transform.DOScale(new Vector3(0.9f, 0.9f, 1f), 0.15f))
            .Append(transform.DOScale(new Vector3(1, 1, 1), 0.1f));
    }
}