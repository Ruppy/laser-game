using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{

    private Material laserHitParticleMaterial;
    private GameObject laserHitParticle;
    private int particlePositionIndex = -1;

    private LineRenderer line;
    private RaycastHit2D hit;
    private float lineLength = 100f;
    private Vector3 initialRaycastPosition;
    public LayerMask layerMask;
    public float laserOffset = 0.05f;
    public string hitIdentifier = "";
    private MirrorController lastMirror;
    private EdgeCollider2D edgeCollider;

    private int totalOfLaserPoints;
    private Vector3[] laserPoints;

    private EventHandler eventHandler = EventHandler.get();
    BoxController currentBox = null;

    public void OnDisable() {
        //Debug.Log("Im being disable " + hitIdentifier);
    }

    public void OnEnable() {
        //Debug.Log("Im being enable " + hitIdentifier);
    }

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        laserHitParticle = transform.Find("HitParticle").gameObject;
        foreach (ParticleSystemRenderer p in gameObject.GetComponentsInChildren<ParticleSystemRenderer>())
         {
            Debug.Log(line);
             //p.material.shader= Shader.Find(p.material.shader.name);
             //p.material.SetColor("Color_B590FB45", line.endColor);
             p.material.SetColor("Color_B590FB45", line.endColor);
         }
        /*if (laserHitParticle) {
            ParticleSystemRenderer ps = laserHitParticle.GetComponent<ParticleSystemRenderer>();
            ps.material.SetColor("Color_B590FB45", line.endColor);
            laserHitParticle.SetActive(false);
        }*/

        laserPoints = new Vector3[30];
        totalOfLaserPoints = 0;
        edgeCollider = GetComponent<EdgeCollider2D>();
    }

    public void FadeOut() {
        StartCoroutine("FadeWidthOut");
    }

    IEnumerator FadeWidthOut() {
        float smoothness = 0.1f;
        float width = GetComponent<LineRenderer>().startWidth;
        while (width > 0.005f) {
            GetComponent<LineRenderer>().startWidth = width * 0.955f;
            width = GetComponent<LineRenderer>().startWidth;
            yield return new WaitForSeconds(smoothness);
        }
        GetComponent<LineRenderer>().startWidth = 0;
    }

    // Update is called once per frame
    void Update()
    {
        particlePositionIndex = -1;

        calculatePoints();
        renderLaserPoints();
        renderLaserCollider();
        renderLaserHitParticle();
    }

    void renderLaserHitParticle() {
        if (laserHitParticle == null) { return; }

        if (particlePositionIndex == -1) {
            laserHitParticle.SetActive(false);
            return;
        }

        laserHitParticle.SetActive(true);
        Vector3 hitPosition = transform.TransformPoint(laserPoints[particlePositionIndex]);
        laserHitParticle.transform.position = hitPosition;
    }

    void renderLaserPoints() {
        line.positionCount = (totalOfLaserPoints * 3) - 2;
        for (int index = 1; index < totalOfLaserPoints; index++) {
            Vector3 point = laserPoints[index];
            int baseIndex = ((index-1) * 3) + 1;
            line.SetPosition(baseIndex, point);
            line.SetPosition(baseIndex + 1, point);
            line.SetPosition(baseIndex + 2, point);
        }
    }

    void renderLaserCollider() {
        if (edgeCollider == null) { return; }

        Vector2[] colliderPoints = new Vector2[totalOfLaserPoints];
        for (int index = 0; index < totalOfLaserPoints; index++) {
            Vector3 point = laserPoints[index];
            colliderPoints[index] = new Vector2(point.x, point.y + 0.15f);
        }
        edgeCollider.points = colliderPoints;
    }

    void calculatePoints() {
        initialRaycastPosition = transform.position + new Vector3(0.01f, 0, 0);

        Debug.DrawRay(initialRaycastPosition, transform.right * lineLength, Color.red);
        hit = Physics2D.Raycast(initialRaycastPosition, transform.right, lineLength, layerMask);
        line.positionCount = 1;
        GameObject lastCollideObject = gameObject;
        Vector3 reflectionAngle = transform.right;

        int pointIndex = 0;
        laserPoints[pointIndex] = new Vector3(0f, 0f, 0f);

        while (hit) {

          line.positionCount += 3;
          if (line.positionCount > 30) { break; }

          Vector3 hitPosition = new Vector3(hit.point.x, hit.point.y, 0);
          pointIndex += 1;
          laserPoints[pointIndex] = transform.InverseTransformPoint(hitPosition);

          GameObject hitObject = hit.collider.gameObject;
          if (hitObject.CompareTag("Mirror") || hitObject.CompareTag("ReflectiveObject")) {
              GameObject mirrorGameObject = hitObject;

              if (lastCollideObject == gameObject) {
                reflectionAngle = Vector3.Reflect(lastCollideObject.transform.right, hit.normal);
              }
              else {
                reflectionAngle = Vector3.Reflect(reflectionAngle, hit.normal);
              }

              Debug.DrawRay(hitPosition - new Vector3(0.01f, 0, 0), reflectionAngle * lineLength, Color.white);

              int currentLayer = mirrorGameObject.layer;
              mirrorGameObject.layer = 2;

              hit = Physics2D.Raycast(hitPosition, reflectionAngle, lineLength, layerMask);
              mirrorGameObject.layer = currentLayer;
              lastCollideObject = mirrorGameObject;
          }
          else if (hitObject.CompareTag("Box")) {
                BoxController boxController = hitObject.GetComponent<BoxController>();
                if(currentBox == null) {
                    currentBox = boxController;
                } else if (currentBox != boxController) {
                    eventHandler.notifyLaserStoppedHittingBox(this, currentBox);
                    currentBox = boxController;
                }
                eventHandler.notifyLaserHitBox(this, currentBox);
                particlePositionIndex = pointIndex;
                break;
          }
          else {
                if (currentBox != null) { //beautify this
                    eventHandler.notifyLaserStoppedHittingBox(this, currentBox);
                    currentBox = null;
                }
                break;
          }
        }

        totalOfLaserPoints = pointIndex + 1;
    }
}
