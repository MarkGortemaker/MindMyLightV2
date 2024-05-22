using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(DialogueReader))]
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;

    private string[] lines;

    public bool IsDialogueActive = false;
    public bool IsTypingFinished = false;
    public bool IsContinueButtonClicked = false;
    public bool IsAuto = false;

    public float typeSpeed = 50f;

    public int lineCount = 0;

    public Animator dialogueAnimator;
    public Animator buttonAnimator;
    public Animator autoButtonAnimator;

    public DialogueReader reader;

    private void Awake()
    {
        reader = GetComponent<DialogueReader>();
    }

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        lines = reader.lines;

        StartCoroutine(StartDialogueWhileWaiting(dialogueAnimator, "DialogueEnter"));
    }

    private void Update()
    {
       if (IsAuto && IsTypingFinished)
       {
            StartCoroutine(DisplayNextAuto());
       }
    }

    public void StartDialogue() 
    {
        IsDialogueActive = true;

        lineCount = 0;

        DisplayNextDialogueLine();
    }

    public void DisplayNextDialogueLine()
    {
        Debug.Log("LineCount: " + lineCount + "\nLineLength: " + lines.Length);
        if (lineCount >= lines.Length)
        {
            EndDialogue();
            return;
        }

        characterName.text = reader.characters[reader.characterOrder[lineCount]];

        StopAllCoroutines();

        StartCoroutine(TypeSentence(reader.lines[lineCount]));

        lineCount++;
    }

    IEnumerator TypeSentence(string dialogue) 
    {
        buttonAnimator.Play("Idle");

        IsTypingFinished = false;

        dialogueArea.text = "";
        foreach (char letter in dialogue)
        {
            dialogueArea.text += letter;

            if (!IsContinueButtonClicked)
            {
                yield return new WaitForSeconds(1f / typeSpeed);
            }
        }

        IsTypingFinished = true;
        IsContinueButtonClicked = false;

        if (lines.Length != 0)
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

        StartDialogue();
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
