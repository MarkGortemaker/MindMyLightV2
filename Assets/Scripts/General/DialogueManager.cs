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
    public static bool IsHide = false;
    
    public enum Fade { None, FadeInBlack, FadeOutBlack, FadeInWhite, FadeOutWhite };
    public static Fade fade = Fade.None;

    public float typeSpeed = 50f;

    public int lineCount = 0;
    public int clipCount = 0;

    public Animator dialogueAnimator;
    public Animator fadeAnimator;
    public Animator buttonAnimator;
    public Animator autoButtonAnimator;

    public DialogueReader reader;


    /*
    TODO:
    - 1-4: Zironko moping, slide camera in
    - 1-5: Ivan and Zironko talk abt wth is going on
    - 1-6: "Dnah? you mean hand?" camera topsy turvy
    - 1-7: Zironko explains the dnah thing
    - 1-8: Zironko comments on Ivan's lantern, which replies that it can help collect the stardust back
    - 1-9: Ivan is scared of the dark, but is willing to help, and the lantern says it will light the way with the stardust
    - 1-10: Zironko warns them of Yrgna 
    - 1-11: Zironko wishes them luck as they go on to collect the dust, cut to black gameplay start

    Other cutscenes:
    - Game end cutscene where Yrgna arrives and attacks Ivan, fade white and wake up in 2D cutscene
    - Two 2D cutscenes for the real world, which are just stills so just shift them along slowly as the dialogue goes on

    * Once the models come in, swap the placeholders /w the models (if you can do it on the same gameObject it'd save time)
    * After the music system is done, music must be played during the cutscene
    */
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

        clipCount = 0;
    }

    public void StartDialogue() 
    {
        lines = reader.lines;

        IsDialogueActive = true;

        lineCount = 0;

        DisplayNextDialogueLine();
    }

    public void DisplayNextDialogueLine()
    {
        if (lineCount >= lines.Length)
        {
            EndDialogue(IsHide);
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

        if (IsAuto)
        {
            StartCoroutine(DisplayNextAuto());
        }
    }

    IEnumerator DisplayNextAuto()
    {
        yield return new WaitForSeconds(100f / typeSpeed);
        DisplayNextDialogueLine();
    }

    IEnumerator StartDialogueWhileWaiting(Animator animator, string animationName)
    {
        switch (fade)
        {
            case Fade.None:
                break;
            case Fade.FadeInBlack:
                fadeAnimator.Play("FadeInBlack");
                yield return new WaitForSeconds(fadeAnimator.runtimeAnimatorController.animationClips[0].length);
                break;
            case Fade.FadeOutBlack:
                fadeAnimator.Play("FadeOutBlack");
                yield return new WaitForSeconds(fadeAnimator.runtimeAnimatorController.animationClips[1].length);
                break;
            case Fade.FadeInWhite:
                fadeAnimator.Play("FadeInWhite");
                yield return new WaitForSeconds(fadeAnimator.runtimeAnimatorController.animationClips[2].length);
                break;
            case Fade.FadeOutWhite:
                fadeAnimator.Play("FadeOutWhite");
                yield return new WaitForSeconds(fadeAnimator.runtimeAnimatorController.animationClips[3].length);
                break;
        }

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

    IEnumerator HideDialogueWindow(Animator animator, string animationName)
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

        characterName.text = "";

        switch (fade)
        {
            case Fade.None:
                break;
            case Fade.FadeInBlack:
                fadeAnimator.Play("FadeInBlack");
                yield return new WaitForSeconds(fadeAnimator.runtimeAnimatorController.animationClips[0].length);
                break;
            case Fade.FadeOutBlack:
                fadeAnimator.Play("FadeOutBlack");
                yield return new WaitForSeconds(fadeAnimator.runtimeAnimatorController.animationClips[1].length);
                break;
            case Fade.FadeInWhite:
                fadeAnimator.Play("FadeInWhite");
                yield return new WaitForSeconds(fadeAnimator.runtimeAnimatorController.animationClips[2].length);
                break;
            case Fade.FadeOutWhite:
                fadeAnimator.Play("FadeOutWhite");
                yield return new WaitForSeconds(fadeAnimator.runtimeAnimatorController.animationClips[3].length);
                break;
        }

        if (reader.nextEntry.ToLower() != "stop")
        {
            reader.NextEntry();
        }

        clipCount++;

        if (clipCount < CutsceneEvent.clips.Length)
        {
            CutsceneEvent.animator.Play(CutsceneEvent.clips[clipCount].name);
        }

        //change to the gameplay level if there is no more cutscenes/dialogue
    }

    public void StartDialogueWhileWaiting()
    {
        StartCoroutine(StartDialogueWhileWaiting(dialogueAnimator, "DialogueEnter"));
    }

    public void HideDialogueWindow()
    {
        StartCoroutine(HideDialogueWindow(dialogueAnimator, "DialogueExit"));
    }

    public void EndDialogue(bool IsHide)
    {
        IsDialogueActive = false;

        buttonAnimator.Play("Idle");

        dialogueArea.text = "";

        if (IsHide)
        {
            HideDialogueWindow();
        }

        else
        {
            if (reader.nextEntry.ToLower() != "stop")
            {
                reader.NextEntry();
            }

            clipCount++;

            if (clipCount < CutsceneEvent.clips.Length)
            {
                CutsceneEvent.animator.Play(CutsceneEvent.clips[clipCount].name);
            }
        }
    }

    public void ToggleAuto()
    {
        IsAuto = !IsAuto;

        if (IsAuto)
        {
            autoButtonAnimator.Play("ButtonBlink");
            if (IsTypingFinished)
            {
                StartCoroutine(DisplayNextAuto());
            }
        }

        else
        {
            autoButtonAnimator.Play("Idle");
        }
    }

    public void SetHide(bool value)
    {
        IsHide = value;
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
