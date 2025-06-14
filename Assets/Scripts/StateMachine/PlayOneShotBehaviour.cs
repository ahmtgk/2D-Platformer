using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOneShotBehaviour : StateMachineBehaviour
{
    public AudioClip soundToPlay;
    public float volume = 1f;
    public bool playOnEnter = true, playOnExit = false, playAfterDelay = false;

    public float playDelay = 0.25f;
    private float timeSinceEntered = 0;
    private bool hasDElayedSoundPlayed = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playOnEnter)
        {
            AudioSource.PlayClipAtPoint(soundToPlay, animator.gameObject.transform.position, volume);
        }

        timeSinceEntered = 0f;
        hasDElayedSoundPlayed = false;
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playAfterDelay && !hasDElayedSoundPlayed)
        {
            timeSinceEntered += Time.deltaTime;

            if (timeSinceEntered > playDelay)
            {
                AudioSource.PlayClipAtPoint(soundToPlay, animator.gameObject.transform.position, volume);
                hasDElayedSoundPlayed = true;
            }
        }
        
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
         if (playOnExit)
        {
            AudioSource.PlayClipAtPoint(soundToPlay, animator.gameObject.transform.position, volume); 
        }
    }
}
