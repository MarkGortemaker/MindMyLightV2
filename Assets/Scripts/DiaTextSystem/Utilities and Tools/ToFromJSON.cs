using System.IO;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Utility for transforming Entry classes to or from JSON's.
/// </summary>
public static class ToFromJSON
{
    /// <summary>
    /// Transforms any entry into a JSON string, which is then published into a file.
    /// </summary>
    /// <param name="entry">The entry which to transform</param>
    public static void EntryToJSON(EntryText entry)
    {
        string json = JsonUtility.ToJson(entry, true);
        Debug.Log(json);
        FindFileDestination(json, entry);
    }
    /// <summary>
    /// Transforms any entry into a JSON string, which is then published into a file.
    /// </summary>
    /// <param name="entry">The entry which to transform</param>
    public static void EntryToJSON(EntryDialogue entry)
    {
        string json = JsonUtility.ToJson(entry, true);
        Debug.Log(json);
        FindFileDestination(json, entry);
    }

    /// <summary>
    /// Transforms any dialogue JSON file into a usable entry class.
    /// </summary>
    /// <param name="path">The path at which the file is located.</param>
    /// <returns>EntryDialogue class</returns>
    public static EntryDialogue JSONtoDialogue(string path) 
    { 
        EntryDialogue entry = new();
        return entry;
    }

    /// <summary>
    /// Transforms any text JSON file into a usable entry class.
    /// </summary>
    /// <param name="path">The path at which the file is located.</param>
    /// <returns>EntryText class</returns>
    public static EntryText JSONtoText(string path)
    {
        EntryText entry = new();
        return entry;
    }

#region File Management
    private static void FindFileDestination(string JSON, EntryText origEntry)
    {
        //Sorts out where the file will be stored and adjusts directory "dir" accordingly.
        string mainDir = Directory.GetCurrentDirectory() + "\\Assets\\Resources\\DiaTextSystem\\";

        PublishToFile(JSON, mainDir + "JSONS\\" + origEntry.entryTitle + ".json");

        if (CheckFile(mainDir + "Chapters\\" + origEntry.chapter + ".asset"))
        {
            JSONChapterLibrary target = Resources.Load<JSONChapterLibrary>("DiaTextSystem\\Chapters\\" + origEntry.chapter);
            Object JSONFile = Resources.Load("DiaTextSystem\\JSONS\\" + origEntry.entryTitle);
            target.textJSONList.Add(JSONFile);

            EditorUtility.SetDirty(target);
        }
    }
    private static void FindFileDestination(string JSON, EntryDialogue origEntry)
    {
        //Sorts out where the file will be stored and adjusts directory "dir" accordingly.
        string mainDir = Directory.GetCurrentDirectory() + "\\Assets\\Resources\\DiaTextSystem\\";

        PublishToFile(JSON, mainDir + "JSONS\\" + origEntry.entryTitle + ".json");

        if (CheckFile(mainDir + "Chapters\\" + origEntry.chapter + ".asset"))
        {
            JSONChapterLibrary target = Resources.Load<JSONChapterLibrary>("DiaTextSystem\\Chapters\\" + origEntry.chapter);
            Object JSONFile = Resources.Load("DiaTextSystem\\JSONS\\" + origEntry.entryTitle);
            target.dialogueJSONList.Add(JSONFile);

            EditorUtility.SetDirty(target);
        }
    }

    /// <summary>
    /// Tuns given JSON string into a file, stored at the directory location given in 'dir'
    /// </summary>
    /// <param name="JSON">The JSON string to be stored.</param>
    /// <param name="dir">The directory at which the JSON file will be stored</param>
    private static void PublishToFile(string JSON, string dir) 
    {
        using (StreamWriter file = File.CreateText(@dir))
        {
            file.Write(JSON);
            file.Close();
        }
    }

    /// <summary>
    /// Checks if the given path leads to a valid directory.
    /// </summary>
    /// <param name="path">Total directory</param>
    /// <param name="chapter">Supposed folder name</param>
    /// <returns></returns>
    private static bool CheckFile(string path)
    {
        if (File.Exists(path))
        { return true; }
        else 
        {
            Debug.LogWarning("Invalid path.");
            return false; 
        }
    }
#endregion File Management
}
