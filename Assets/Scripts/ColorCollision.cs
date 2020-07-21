using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCollision : MonoBehaviour
{
    private Collider2D collider;
    private Camera camera;
    private SpriteRenderer renderer;

    void Start()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        collider = GetComponent<BoxCollider2D>();
        renderer = GetComponent<SpriteRenderer>();
    }

    void Update() {
        //Debug.Log(camera.backgroundColor);
        //Debug.Log(renderer.color);

        float redDiff = Mathf.Abs(camera.backgroundColor.r - renderer.color.r);
        float greenDiff = Mathf.Abs(camera.backgroundColor.g - renderer.color.g);
        float blueDiff = Mathf.Abs(camera.backgroundColor.b - renderer.color.b);
        float colorDiff = redDiff + greenDiff + blueDiff;

        //bool enableCollision = camera.backgroundColor != renderer.color;
        bool enableCollision = colorDiff > 0.3;
        collider.enabled = enableCollision;
        renderer.enabled = collider.enabled;
    }
}