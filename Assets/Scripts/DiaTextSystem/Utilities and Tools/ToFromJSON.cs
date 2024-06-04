using System.IO;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Utility for transforming Entry classes to or from JSON files
/// </summary>
public static class ToFromJSON
{
    /// <summary>
    /// Transforms any entry into a JSON string, which is then published into a file.
    /// </summary>
    /// <param name="entry">The entry which to transform</param>
    public static void EntryToJSON(BaseEntry entry)
    {
        string json = JsonUtility.ToJson(entry, true);
        Debug.Log(json);
        FindFileDestination(json, entry);
    }

    public static string ReadJSON(string path) 
    {
        string json = "";
        using (StreamReader file = File.OpenText(path))
        { json = file.ReadToEnd(); }
        return json;
    }

    /// <summary>
    /// Transforms any entry JSON file into a usable entry class.
    /// </summary>
    /// <param name="path">The path at which the file is located.</param>
    /// <returns>BaseEntry class</returns>
    public static BaseEntry JSONToEntry(string path) 
    {
        BaseEntry entry = JsonUtility.FromJson<EntryDialogue>(ReadJSON(path));
        return entry;
    }


    /// <summary>
    /// Transforms any dialogue JSON file into a usable entry class.
    /// </summary>
    /// <param name="path">The path at which the file is located.</param>
    /// <returns>EntryDialogue class</returns>
    public static EntryDialogue JSONToDialogue(string path) 
    {
        EntryDialogue entry = JsonUtility.FromJson<EntryDialogue>(ReadJSON(path));
        return entry;
    }

    /// <summary>
    /// Transforms any text JSON file into a usable entry class.
    /// </summary>
    /// <param name="path">The path at which the file is located.</param>
    /// <returns>EntryText class</returns>
    public static EntryText JSONToText(string path)
    {
        EntryText entry = JsonUtility.FromJson<EntryText>(ReadJSON(path));
        return entry;
    }

#region File Management
    /// <summary>
    /// Finds the final destination of the file using the "chapter" variable in the Entry, the type of file, 
    /// and it's name in "entryTitle", and then publishes the file with PublishToFile.
    /// </summary>
    /// <param name="JSON"></param>
    /// <param name="origEntry"></param>
    private static void FindFileDestination(string JSON, BaseEntry origEntry)
    {
        //Sorts out where the file will be stored and adjusts directory "dir" accordingly.
        string mainDir = Directory.GetCurrentDirectory() + "\\Assets\\Resources\\DiaTextSystem\\";

        PublishToFile(JSON, mainDir + "JSONS\\" + origEntry.entryTitle + ".json");

        if (CheckFile(mainDir + "Chapters\\" + origEntry.chapter + ".asset"))
        {
            JSONChapterLibrary target = Resources.Load<JSONChapterLibrary>("DiaTextSystem\\Chapters\\" + origEntry.chapter);
            Object JSONFile = Resources.Load("DiaTextSystem\\JSONS\\" + origEntry.entryTitle);
            switch (origEntry.entryType)
            {
                case EntryType.Text:
                    if (target.textJSONList.Contains(JSONFile)) { return; } //If this list already contains a reference to the file "JSONFile", returns.
                    target.textJSONList.Add(JSONFile);
                    break;
                case EntryType.Dialogue:
                    if (target.dialogueJSONList.Contains(JSONFile)) { return; } //If this list already contains a reference to the file "JSONFile", returns.
                    target.dialogueJSONList.Add(JSONFile);
                    break;
            }
#if UNITY_EDITOR
            EditorUtility.SetDirty(target);
#endif
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
#if  UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
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
