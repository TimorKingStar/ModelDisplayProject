using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMessageFromIOS : MonoBehaviour
{
    public GameObject cube;
    private void Awake()
    {
        cube = transform.GetChild(0).gameObject;
    }

    //��Ϸ�������ƣ�GetMessageFromIOS  �������ƺͲ������£�

    public void SetModelStateBool(bool state)
    {
        cube.gameObject.SetActive(state);
    }
    public void SetModelStateString(string state)
    {
        if (state=="active")
        {
            cube.gameObject.SetActive(true);
        }
        if (state == "close")
        {
            cube.gameObject.SetActive(false);
        }
    }
    public void SetModelStateFloat(float state)
    {
        if (state==1f)
        {
            cube.gameObject.SetActive(true);
        }
        if (state == 0)
        {
            cube.gameObject.SetActive(false);
        }
    }
    public void SetModelStateVector2(Vector2 state)
    {
        if (state.x >0f && state.y>0f)
        { 
            cube.gameObject.SetActive(true);
        }
        if (state.x <= 0f && state.y <= 0f)
        {
            cube.gameObject.SetActive(false);
        }
    }

}