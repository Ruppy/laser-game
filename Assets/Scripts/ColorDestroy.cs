using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorDestroy : MonoBehaviour
{
    private Camera camera;
    private SpriteRenderer renderer;
    private GameObject scriptsObject;

    void Start()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        renderer = GetComponent<SpriteRenderer>();
        scriptsObject = GameObject.Find("Scripts");
    }

    void Update() {
        float redDiff = Mathf.Abs(camera.backgroundColor.r - renderer.color.r);
        float greenDiff = Mathf.Abs(camera.backgroundColor.g - renderer.color.g);
        float blueDiff = Mathf.Abs(camera.backgroundColor.b - renderer.color.b);
        float colorDiff = redDiff + greenDiff + blueDiff;

        bool shouldDestroy = colorDiff < 0.2;
        if(shouldDestroy) {
            gameObject.SetActive(false);
        }
    }
}