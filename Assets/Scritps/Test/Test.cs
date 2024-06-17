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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            fromIOS.SetHeadLayerShow(message + "+" + state.ToString());
            fromIOS.SetAlphaState(alpha.ToString());
            fromIOS.SetOutlineState(outLine.ToString());  
        }
    }
}
