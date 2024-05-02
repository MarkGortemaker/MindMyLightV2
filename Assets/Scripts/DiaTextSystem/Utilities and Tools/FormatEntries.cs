using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FormatEntries
{
    static List<DialogueLine> dialogueLines = new();

    public static void FormatFullDialogue()
    {
        EntryDialogue entry = new();
        //entry.entryTitle =
        entry.dialogueLines = new string[dialogueLines.Count];
        entry.characterOrder = new string[dialogueLines.Count];
        for (int i = 0; i < dialogueLines.Count; i++)
        {
            entry.dialogueLines[i] = dialogueLines[i].content;
        }
        ToFromJSON.EntryToJSON(entry);
        dialogueLines.Clear();
    }

    public static void FormatDialogueLine()
    {
        DialogueLine line = new();
        //line.character =
        //line.content = 
        dialogueLines.Add(line);
    }
}
