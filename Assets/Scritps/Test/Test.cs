using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GetMessageFromIOS fromIOS;
    public Vector3 rotate;

    private void Update()
    {
        fromIOS.SetLightMoveDirX(rotate.x.ToString());
        fromIOS.SetLightMoveDirY(rotate.y.ToString());
        fromIOS.SetLightMoveDirZ(rotate.z.ToString());
    }
}
