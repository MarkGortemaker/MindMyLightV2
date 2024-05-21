using System.IO;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// A Library class for storing and sorting file and file types for editing and display purposes.
/// </summary>
[CreateAssetMenu(fileName = "JSON Chapter Library", menuName = "Dialogue and Text System/JSON Chapter Library")]
public class JSONChapterLibrary : ScriptableObject
{
    [Tooltip("List of standard text files.")]
    public List<Object> textJSONList;
    [Tooltip("List of dialogue files.")]
    public List<Object> dialogueJSONList;


    public void onListChanged(List<Object> list)
    {
        if (list.Contains(null))
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == null)
                { list.RemoveAt(i); }
            }
        }
    }
}
