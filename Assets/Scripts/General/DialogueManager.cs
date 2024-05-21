using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;

    private Queue<DialogueText> lines;

    public bool IsDialogueActive = false;
    public bool IsTypingFinished = false;
    public bool IsContinueButtonClicked = false;
    public bool IsAuto = false;

    public float typeSpeed = 50f;

    public Animator dialogueAnimator;
    public Animator buttonAnimator;
    public Animator autoButtonAnimator;

    public Dialogue dialogue;


    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        lines = new Queue<DialogueText>();

        StartCoroutine(StartDialogueWhileWaiting(dialogueAnimator, "DialogueEnter"));
    }

    private void Update()
    {
       if (IsAuto && IsTypingFinished)
       {
            StartCoroutine(DisplayNextAuto());
       }
    }

    public void StartDialogue(Dialogue dialogue) 
    {
        IsDialogueActive = true;

        lines.Clear();

        foreach (DialogueText line in dialogue.dialogueLines)
        {
            lines.Enqueue(line);
        }

        DisplayNextDialogueLine();
    }

    public void DisplayNextDialogueLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueText currentLine = lines.Dequeue();

        characterName.text = currentLine.character.name;

        StopAllCoroutines();

        StartCoroutine(TypeSentence(currentLine));
    }

    IEnumerator TypeSentence(DialogueText dialogue) 
    {
        buttonAnimator.Play("Idle");

        IsTypingFinished = false;

        dialogueArea.text = "";
        foreach (char letter in dialogue.line)
        {
            dialogueArea.text += letter;

            if (!IsContinueButtonClicked)
            {
                yield return new WaitForSeconds(1f / typeSpeed);
            }
        }

        IsTypingFinished = true;
        IsContinueButtonClicked = false;

        if (lines.Count != 0)
        {
            buttonAnimator.Play("ButtonBob");
        }
    }

    IEnumerator DisplayNextAuto()
    {
        yield return new WaitForSeconds(50f / typeSpeed);
        DisplayNextDialogueLine();
    }

    IEnumerator StartDialogueWhileWaiting(Animator animator, string animationName)
    {
        AnimationClip currentClip = null;

        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == animationName)
            {
                currentClip = clip;
                break;
            }
        }

        animator.Play(animationName);

        yield return new WaitForSeconds(currentClip.length);

        StartDialogue(dialogue);
    }

    public void EndDialogue()
    {
        IsDialogueActive = false;

        buttonAnimator.Play("Idle");
        dialogueAnimator.Play("DialogueExit");
    }

    public void ToggleAuto()
    {
        IsAuto = !IsAuto;

        if (IsAuto)
        {
            autoButtonAnimator.Play("ButtonBlink");
        }

        else
        {
            autoButtonAnimator.Play("Idle");
        }
    }
    public void OnContinueButtonClick()
    {
        if (!IsTypingFinished)
        {
            IsContinueButtonClicked = true;
        }

        else
        {
            DisplayNextDialogueLine();
        }
    }
}
