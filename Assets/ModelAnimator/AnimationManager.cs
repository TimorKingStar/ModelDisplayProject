using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    /// <summary>
    /// The animation attached to the radical character (easiest if you put the character in the scene and drag it here)
    /// </summary>
    public Animation m_Animation;
    public RectTransform playbackProcess; // You can choose any object, but new Unity UI is best, because it's independent of camera location
    /// <summary>
    /// Dictionary that stores the downloaded animations by name, so you can easily access them at any point
    /// </summary>
    Dictionary<string, AnimationClip> m_Animations = new Dictionary<string, AnimationClip>();
    float lengthOfClip;

    /// <summary>
    /// Call this once the AssetLoaderContext has finished loading, see TmpGUI.cs
    /// </summary>
    /// <param name="animationName">Choose any name, it does not have to match the clip's name</param>
    /// <param name="clip">Any AnimationClip, usually the clip that was attached to the animation of the RootGameObject of the AssetLoaderContext</param>
    public void StoreAnimation(string animationName, AnimationClip clip)
    {
        if (!m_Animations.ContainsKey(animationName))
        {
            clip.legacy = true;
            m_Animations.Add(animationName, clip);
        }
        else
        {
            print("WARNING: Dictionary already contains a clip named " + animationName);
        }
    }

    private void Update()
    {
        if (m_Animation.isPlaying && lengthOfClip != 0)
        {
            // Update the progress bar
            foreach (AnimationState state in m_Animation)
            {
                float currentTime = state.time;
                if (currentTime > 0)
                {
                    float scale = currentTime / lengthOfClip;
                    playbackProcess.localScale = new Vector3(scale, 1, 1);
                }
            }
        }
    }

    /// <summary>
    /// Play back an animation by name (the one you chose when storing it, not necessarily the name of the clip)
    /// </summary>
    /// <param name="animationName"></param>
    public void PlayAnimation(string animationName)
    {
        if (!m_Animations.ContainsKey(animationName))
        {
            print("WARNING: Dictionary does not contain a clip named " + animationName);
            return;
        }
        AnimationClip activeAnimation = m_Animations[animationName];
        
        m_Animation.AddClip(activeAnimation, activeAnimation.name); //the names of AddClip and clip must match, 
        m_Animation.clip = activeAnimation;
        m_Animation.Play();
        lengthOfClip = activeAnimation.length;
        playbackProcess.localScale = new Vector3(0, 1, 1);
    }

    /// <summary>
    /// Pause the current animation
    /// </summary>
    public void Pause()
    {
        if (m_Animation.isPlaying)
        {
            foreach (AnimationState state in m_Animation) // Animators are more elegant, but they are also more complicated to set up
            {
                if (state.time > 0)
                {
                    state.speed = 0;
                }
            }
        }
    }

    /// <summary>
    /// Resume at the time, the animation was paused
    /// </summary>
    public void Resume()
    {
        foreach (AnimationState state in m_Animation)
        {
            if (state.speed == 0)
            {
                state.speed = 1;
            }
        }
    }

    /// <summary>
    /// Stop and rewind the animation
    /// </summary>
    public void Stop()
    {
        AnimationClip currentClip = m_Animation.clip;
        if (currentClip != null)
        {
            m_Animation.Stop();
            playbackProcess.localScale = new Vector3(0, 1, 1);
        }
    }
}

