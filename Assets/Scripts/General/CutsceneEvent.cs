using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneEvent : MonoBehaviour
{
    public static AnimationClip[] clips;
    public void StartDialogue()
    {
        DialogueManager.instance.StartDialogueWhileWaiting();
    }

    private void Start()
    {
        clips = GetComponent<Animator>().runtimeAnimatorController.animationClips;
    }
}
