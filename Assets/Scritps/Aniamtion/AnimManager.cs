using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class AnimManager : MonoBehaviour
{
    Animator  anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();

        var con =  anim.runtimeAnimatorController as AnimatorController;

    }

    public AnimationClip clip;

    void AddAnmation(AnimationClip  clip)
    {
      
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddAnmation(clip); 
        }
    }

}
