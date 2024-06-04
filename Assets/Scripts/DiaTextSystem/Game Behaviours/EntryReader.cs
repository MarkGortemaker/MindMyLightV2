using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
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
        System.IO.FileInfo fileInfo = new System.IO.FileInfo(toRead.name);
        Debug.Log(fileInfo.FullName);

        entry = JSONToEntry(AssetDatabase.GetAssetPath(toRead));
        entryName = entry.entryTitle;
        nextEntry = entry.nextEntryTitle;
    }

    /// <summary>
    /// Prompts Reader to find and digest the next entry specified in 'nextEntry'
    /// </summary>
    public abstract void NextEntry(); 
}
