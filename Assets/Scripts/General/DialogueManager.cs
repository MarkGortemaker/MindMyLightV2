using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    
    public enum Fade { None, FadeInBlack, FadeOutBlack, FadeInWhite, FadeOutWhite }; //enum for fade out/fade in options
    public static Fade fade = Fade.None;

    public float typeSpeed = 50f; //the speed at which the dialogue gets written on the text box

    public int lineCount = 0; //index of dialogue lines
    public int clipCount = 0; //index of animation clips

    public Animator dialogueAnimator;
    public Animator fadeAnimator;
    public Animator buttonAnimator;
    public Animator autoButtonAnimator;

    public GameObject winScreen;

    public DialogueReader reader;


    /*
    TODO:

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
        if (instance == null) //Singleton
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

    /// <summary>
    /// Displays the next dialogue line if there is a line to be displayed, ends current dialogue if not.
    /// </summary>
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

    /// <summary>
    /// Types out sentence in the text box letter by letter. If the player interacts with the text box, displays the entire text instantly instead of letter by letter. 
    /// If Auto button is on, calls DisplayNextAuto.
    /// </summary>
    /// <param name="dialogue">Dialogue to be typed in the textbox.</param>
    /// <returns></returns>
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

    /// <summary>
    /// Waits for 100 / typeSpeed seconds to display the next dialogue.
    /// </summary>
    /// <returns></returns>
    IEnumerator DisplayNextAuto()
    {
        yield return new WaitForSeconds(100f / typeSpeed);
        DisplayNextDialogueLine();
    }

    /// <summary>
    /// Plays the given animation, and plays the corresponding fade out/fade in animation if the fade enum is not 0.
    /// Starts dialogue after waiting for the animations to finish. 
    /// </summary>
    /// <param name="animator">The animator to be used for the animation.</param>
    /// <param name="animationName">The name of the animation to be played.</param>
    /// <returns></returns>
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

    /// <summary>
    /// Hides the dialogue window after playing the given animation and waiting for it to finish.
    /// Plays the corresponding fade out/fade in animation to the fade enum if it is not 0.
    /// If there is a next entry, gets the reader to the next entry and plays the next animation in the cutscene. If there is not, loads the next scene.
    /// </summary>
    /// <param name="animator">The animator to be used for the animation.</param>
    /// <param name="animationName">The name of the animation to be played.</param>
    /// <returns></returns>
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
            clipCount++;
        }

        else {
            if (SceneManager.GetActiveScene().name.Contains("End"))
            {
                winScreen.SetActive(true);
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        if (clipCount < CutsceneEvent.clips.Length)
        {
            CutsceneEvent.animator.Play(CutsceneEvent.clips[clipCount].name);
        }
    }

    public void StartDialogueWhileWaiting()
    {
        StartCoroutine(StartDialogueWhileWaiting(dialogueAnimator, "DialogueEnter"));
    }

    public void HideDialogueWindow()
    {
        StartCoroutine(HideDialogueWindow(dialogueAnimator, "DialogueExit"));
    }

    /// <summary>
    /// Ends current dialogue. If IsHide is true, calls HideDialogueWindow. Otherwise, gets the reader to the next entry and plays the next animation.
    /// If there is no next entry, loads the next scene.
    /// </summary>
    /// <param name="IsHide"></param>
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
                clipCount++;
            }

            else
            {
                if (SceneManager.GetActiveScene().name.Contains("End"))
                {
                    winScreen.SetActive(true);
                }
                else
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
            }

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

        else if (IsDialogueActive)
        {
            DisplayNextDialogueLine();
        }
    }
}
