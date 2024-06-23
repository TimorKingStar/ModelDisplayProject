using System.Collections;
using System.Collections.Generic;
using TriLibCore;
using TriLibCore.Extensions;
using UnityEngine;

/// <summary>
/// Example of how to use the AnimationManager
/// </summary>
public class TmpGUI : MonoBehaviour
{
    AnimationManager animationManager;
    AnimationClip activeClip;

    private void Start()
    {
        animationManager = GetComponent<AnimationManager>();
    }

    private void OnGUI()
    {
        
        if (GUILayout.Button("Load"))
        {
            
            var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
            assetLoaderOptions.AnimationType = TriLibCore.General.AnimationType.Legacy; //The current setup requires Legacy animations
            assetLoaderOptions.EnforceAnimatorWithLegacyAnimations = true;
            var assetLoaderFilePicker = AssetLoaderFilePicker.Create();
            // Replace this with download version of AssetLoader
            assetLoaderFilePicker.LoadModelFromFilePickerAsync("Select a File", OnLoad, OnMaterialLoad, OnProgress,
                OnBeginLoad, OnError, gameObject, assetLoaderOptions);
        }

        // call these on the AnimationManager
        if (GUILayout.Button("Play"))
        {
            // Make sure to wait until the loading of the model is complete before pressing play, it's loading async
            animationManager.PlayAnimation(activeClip.name); // you can change the name to something more meaningfull, if you want
        }
        if (GUILayout.Button("Pause"))
        {
            animationManager.Pause();
        }
        if (GUILayout.Button("Continue"))
        {
            animationManager.Resume(); 
        }
        if (GUILayout.Button("Stop"))
        {
            animationManager.Stop();
        }
    }

    /// <summary>
    /// This gets called automatically once the load of the fbx is complete
    /// </summary>
    /// <param name="obj"></param>
    private void OnLoad(AssetLoaderContext obj)
    {
        if (obj.RootGameObject.TryGetComponent(out Animation a))
        {
            var l_animationClips = a.GetAllAnimationClips();
            if (l_animationClips.Count > 0)
            {
                activeClip = l_animationClips[0]; // there should be only one clip per download
            }

            animationManager.StoreAnimation(activeClip.name, activeClip); //use this to store any animation by name. You can pick a more meaningful name, if you want
        }
        else
            print("WARNING: there was no animation on the loaded fbx");
        
        //Destroy(obj.RootGameObject); // WARNING Do not use the TriLibCore Unload() function, it also destroys the animation clip
    }

    private void OnBeginLoad(bool obj)
    {
    }

    private void OnError(IContextualizedError obj)
    {
    }

    private void OnProgress(AssetLoaderContext arg1, float progress)
    {
        //if you want to keep track of the loading progress, uncomment
        //int progressPercent = Mathf.RoundToInt(progress * 100f);
        //print($"Progress: {progressPercent}%");
    }


    private void OnMaterialLoad(AssetLoaderContext obj)
    {
       // We are not loading any materials
    }
}
