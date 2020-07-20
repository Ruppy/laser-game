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

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        if (transform.parent == null) {
          Debug.Log("no parent");
          line.enabled = true;
        }
        else {
          Debug.Log("parent");
          line.enabled = false;
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

          //line.positionCount += 1;
          line.positionCount += 3;
          Debug.Log("Line total " + line.positionCount);
          if (line.positionCount > 20) { break; }

          //Vector3 hitPosition = new Vector3(hit.point.x, hit.point.y, 0) - initialRaycastPosition;
          Vector3 hitPosition = new Vector3(hit.point.x, hit.point.y, 0);
          //Debug.Log("Before hit " + hitPosition + " Line: " + line.positionCount);
          //line.SetPosition(line.positionCount - 1, hitPosition);
          line.SetPosition(line.positionCount - 3, transform.InverseTransformPoint(hitPosition));
          line.SetPosition(line.positionCount - 2, transform.InverseTransformPoint(hitPosition));
          line.SetPosition(line.positionCount - 1, transform.InverseTransformPoint(hitPosition));
          //line.SetPosition(line.positionCount - 1, hitPosition);

          GameObject hitObject = hit.collider.gameObject;
          //MirrorController mirror = hit.collider.gameObject.transform.parent.GetComponent<MirrorController>();
          if (hitObject.CompareTag("Mirror")) {
              GameObject mirrorGameObject = hitObject;

              //Debug.Log("its a hit mirror");

              if (lastCollideObject == gameObject) {
                reflectionAngle = Vector3.Reflect(lastCollideObject.transform.right, hit.normal);
              }
              else {
                reflectionAngle = Vector3.Reflect(reflectionAngle, hit.normal);
              }
              //Debug.Log("Angle" + reflectionAngle);

              Debug.DrawRay(hitPosition - new Vector3(0.01f, 0, 0), reflectionAngle * lineLength, Color.white);
              //break;


              int currentLayer = mirrorGameObject.layer;
              mirrorGameObject.layer = 2;

              hit = Physics2D.Raycast(hitPosition, reflectionAngle, lineLength, layerMask);
              mirrorGameObject.layer = currentLayer;
              lastCollideObject = mirrorGameObject;
          }
          else if (hitObject.CompareTag("Box")) {
            Debug.Log("Laser hit box");
            GameObject boxGameObject = hitObject;
            BoxController boxController = boxGameObject.GetComponent<BoxController>();
            boxController.Hit(hitIdentifier);
            break;
          }
          else {
            Debug.Log("its a hit wall " + line.positionCount);
            break;
          }
        }
    }
}
