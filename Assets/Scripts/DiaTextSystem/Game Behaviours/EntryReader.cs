using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ToFromJSON;

public abstract class EntryReader : MonoBehaviour
{
    public JSONChapterLibrary chapter;
    public Object toRead;
    BaseEntry entry;

    public string entryName;
    public string nextEntry;

    protected virtual void Awake()
    {
        DigestBaseEntry();
    }

    /// <summary>
    /// Processes the entry from 'toRead'.
    /// </summary>
    void DigestBaseEntry()
    {
        entry = JSONToEntry(Resources.Load<TextAsset>(PlayerPrefs.GetString("JSONDir") + "\\JSONS\\" + toRead.name));
        entryName = entry.entryTitle;
        nextEntry = entry.nextEntryTitle;
    }

    /// <summary>
    /// Prompts Reader to find and digest the next entry specified in 'nextEntry'
    /// </summary>
    public abstract void NextEntry(); 
}
