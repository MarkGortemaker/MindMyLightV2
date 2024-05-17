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
    public bool IsAuto = false;

    public float typeSpeed = 50f;

    public Animator dialogueAnimator;
    public Animator buttonAnimator;

    public Dialogue dialogue;

    /*
     * TODO:
     * - Auto button should work even when the text has finished displaying
     * - Auto button should have an animation that shows it is currently on
     * - The next button should stop blinking or disappear when it is the last line on the dialogue
     * - Animations
     */

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        lines = new Queue<DialogueText>();

        StartDialogue(dialogue);
    }

    public void StartDialogue(Dialogue dialogue) 
    {
        IsDialogueActive = true;

        dialogueAnimator.Play("DialogueEnter");

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

        dialogueArea.text = "";
        foreach (char letter in dialogue.line)
        {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(1f / typeSpeed);
        }

        buttonAnimator.Play("ButtonBlink");

        if (IsAuto)
        {
            yield return new WaitForSeconds(50f / typeSpeed);
            DisplayNextDialogueLine();
        }
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
    }
}
