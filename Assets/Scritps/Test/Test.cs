using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    Animation anim;
    bool state;

    public GameObject current;
    public GameObject game;

    private void Awake()
    {
        anim = GetComponent<Animation>();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("����"))
        {
            state = !state;
            if (state)
            {
                if (anim.isPlaying)
                {
                    foreach (AnimationState state in anim)
                    {
                        state.speed = 1;
                    }
                }
                else
                {
                    anim.Play();
                }
            }
            else
            {
                if (anim.isPlaying)
                {
                    foreach (AnimationState state in anim)
                    {
                        if (state.time > 0)
                        {
                            state.speed = 0;
                        }
                    }
                }
            }
        }

        if (GUILayout.Button("��ͣ"))
        {
            Debug.LogError(Vector3.Distance(current.transform.position,game.transform.position));
        }
        if (GUILayout.Button("����"))
        {
            foreach (AnimationState state in anim)
            {
                state.speed = 1;
            }
            Debug.Log("IsPlaying: "+ anim.isPlaying);
        }
    }
}
