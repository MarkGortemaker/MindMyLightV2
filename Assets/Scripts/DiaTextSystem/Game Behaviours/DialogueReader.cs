using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static ToFromJSON;

public class DialogueReader : EntryReader
{
    public string[] characters;

    public int[] characterOrder;
    public string[] lines;

    protected override void Awake()
    {
        base.Awake();
        DigestDialogue();
    }

    /// <summary>
    /// Digests current DialogueEntry at 'toRead', located in the base class EntryReader.
    /// Spits it out as several Debug.Log.
    /// </summary>
    public void DigestDialogue()
    {
        EntryDialogue entry = JSONToDialogue(AssetDatabase.GetAssetPath(toRead));
        characters = entry.characters;
        characterOrder = entry.characterOrder;
        lines = entry.dialogueLines;

        for (int i = 0; i < lines.Length; i++)
        { Debug.Log(characters[characterOrder[i]] + ": " + lines[i]); }
    }

    public override void NextEntry()
    {
        toRead = chapter.textJSONList.Find(x => x.name == nextEntry);
        Debug.Log(toRead.name);
        Awake();
    }
}
