using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GetMessageFromIOS fromIOS;
    public string message;
    public bool state;

    public float alpha;
    public float outLine;
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    fromIOS.TurnOnModelRotateState(message);
        //}

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            fromIOS.ResetCameraRotate();
        }
    }
}
