using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEntry
{
    public string entryTitle; //The name of this entry
    public string nextEntryTitle; //The name of the next entry. If left empty, it will autofill it's own name to loop back on itself.
    public string chapter; //The chapter this entry belongs to. if 0, null, or invalid, will be sorted into "Chapterless"
}

public class EntryText : BaseEntry
{
    public string content; //The content of this entry.
}

public class EntryDialogue : BaseEntry
{
    public string[] characterOrder; //Determines what name is seen on screen. All strings in this array correspond with a string in "dialogueLines".
    public string[] dialogueLines; //All the dialogue lines of this entry.
}

public class DialogueLine
{
    public string character; //The character that's speaking
    public string content; //The content of the speech
}
