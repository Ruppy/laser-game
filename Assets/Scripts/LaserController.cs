using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{

    private LineRenderer line;
    private RaycastHit2D hit;
    private float lineLength = 100f;
    private Vector3 initialRaycastPosition;
    public LayerMask layerMask;
    public float laserOffset = 0.05f;
    public string hitIdentifier = "";
    private MirrorController lastMirror;

    private EventHandler eventHandler = EventHandler.get();
    BoxController currentBox = null;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        if (transform.parent == null) {
          //line.enabled = true;
        }
        else {
          //line.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        initialRaycastPosition = transform.position + new Vector3(0.01f, 0, 0);

        Debug.DrawRay(initialRaycastPosition, transform.right * lineLength, Color.red);
        hit = Physics2D.Raycast(initialRaycastPosition, transform.right, lineLength, layerMask);
        line.positionCount = 1;

        GameObject lastCollideObject = gameObject;
        Vector3 reflectionAngle = transform.right;

        while (hit) {

          line.positionCount += 3;
          if (line.positionCount > 20) { break; }

          Vector3 hitPosition = new Vector3(hit.point.x, hit.point.y, 0);
          line.SetPosition(line.positionCount - 3, transform.InverseTransformPoint(hitPosition));
          line.SetPosition(line.positionCount - 2, transform.InverseTransformPoint(hitPosition));
          line.SetPosition(line.positionCount - 1, transform.InverseTransformPoint(hitPosition));

          GameObject hitObject = hit.collider.gameObject;
          if (hitObject.CompareTag("Mirror")) {
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
                break;
          }
          else {
                if (currentBox != null) { //beautify this
                    currentBox = null;
                    eventHandler.notifyLaserStoppedHittingBox(this, currentBox);
                }
                break;
          }
        }
    }
}
