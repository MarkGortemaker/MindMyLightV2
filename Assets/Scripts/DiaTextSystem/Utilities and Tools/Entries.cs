using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Enumerator for determining Entry Type.
public enum EntryType
{
    Text,
    Dialogue
}

/// <summary>
/// <para>
/// The base class for any entry type.
/// </para>
/// <para>
/// It is not recommended that the user uses this class for storing information, 
/// but instead uses one of it’s children classes or makes a new child class based off of this one.
/// </para>
/// </summary>
public class BaseEntry
{
    public EntryType entryType;
    public string entryTitle; //The name of this entry
    public string nextEntryTitle; //The name of the next entry. If left empty, it will autofill it's own name to loop back on itself.
    public string chapter; //The chapter this entry belongs to. if 0, null, or invalid, will be sorted into "Chapterless"
}

/// <summary>
/// Child class of BaseEntry, handles basic text.
/// </summary>
public class EntryText : BaseEntry
{
    public string content; //The content of this entry.
}

/// <summary>
/// Child class of BaseEntry, handles dialogue between 2 or more characters.
/// </summary>
public class EntryDialogue : BaseEntry
{
    new const int entryType = 2;
    public string[] characters; //Array of all characters in this dialogue.
    public int[] characterOrder; //Determines what name is seen on screen. this array contains indexes for the string[array] of 'characters' seen above.
    public string[] dialogueLines; //All the dialogue lines of this entry.
}

/// <summary>
/// Line for storing a singular Dialogueline, which can then be added into a EntryDialogue along with other DialogueLine class instances.
/// </summary>
[Obsolete]
public class DialogueLine
{
    public string character; //The character that's speaking
    public string content; //The content of the speech
}
