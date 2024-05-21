using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static ToFromJSON;

public class TextReader : EntryReader
{
    public string text;

    protected override void Awake()
    {
        base.Awake();
        DigestText();
    }


    /// <summary>
    /// Digests current Text Entry at 'toRead', located in the base class EntryReader. 
    /// Spits it out as a Debug.Log.
    /// </summary>
    void DigestText()
    {
        EntryText entry = JSONToText(AssetDatabase.GetAssetPath(toRead));
        text = entry.content;
        Debug.Log(text);
    }

    public override void NextEntry()
    {
        toRead = chapter.textJSONList.Find(x => x.name == nextEntry);
        Debug.Log(toRead.name);
        Awake();
    }
}
