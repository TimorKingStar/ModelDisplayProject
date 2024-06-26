using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFixToSceen : MonoBehaviour
{
    [SerializeField]
     float widthOffset;
    [SerializeField]
    float lengthOffset;
    public float distance;
    private void Start()
    {
        widthOffset = Camera.main.pixelWidth / 10;
        lengthOffset = Camera.main.pixelHeight / 5;
    }

    void Update()
    {
        Vector3 screenUpperRight = new Vector3( Camera.main.pixelWidth- widthOffset, Camera.main.pixelHeight- lengthOffset, distance);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenUpperRight);
        transform.position = worldPosition;
        // transform.Translate(Vector3.forward * -distance);
    }
}
