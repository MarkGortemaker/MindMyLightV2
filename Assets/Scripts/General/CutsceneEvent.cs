using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneEvent : MonoBehaviour
{
    public static AnimationClip[] clips;
    public static Animator animator;

    public void StartDialogueWhileWaiting()
    {
        DialogueManager.instance.StartDialogueWhileWaiting();
    }

    public void StartDialogue()
    {
        DialogueManager.instance.StartDialogue();
    }

    public void HideDialogueWindow()
    {
        DialogueManager.instance.HideDialogueWindow();
    }

    public void EndDialogue()
    {
        DialogueManager.instance.EndDialogue(DialogueManager.IsHide);
    }

    public void SetHide(int value)
    {
        bool result;

        if (value == 1)
        {
            result = true;
        }
        else
        {
            result = false;
        }

        DialogueManager.IsHide = result;
    }

    public void SetFade(int fade)
    {
        DialogueManager.fade = (DialogueManager.Fade)Mathf.Clamp(fade, 0, 4);
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        clips = animator.runtimeAnimatorController.animationClips;
    }
}
