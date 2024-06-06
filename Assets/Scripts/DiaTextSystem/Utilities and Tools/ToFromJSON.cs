using System.IO;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Utility for transforming Entry classes to or from JSON files
/// </summary>
public static class ToFromJSON
{
    public static string ReadJSON(string path) 
    {
        string result = "";
        using (StreamReader file = File.OpenText(path))
        { result = file.ReadToEnd(); }
        return result;
    }

    /// <summary>
    /// Transforms any entry JSON file into a usable entry class.
    /// </summary>
    /// <param name="path">The path at which the file is located.</param>
    /// <returns>BaseEntry class</returns>
    public static BaseEntry JSONToEntry(string path) 
    {
        BaseEntry entry = JsonUtility.FromJson<BaseEntry>(ReadJSON(path));
        return entry;
    }
    public static BaseEntry JSONToEntry(TextAsset JSON)
    {
        BaseEntry entry = JsonUtility.FromJson<BaseEntry>(JSON.text);
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
    public static EntryDialogue JSONToDialogue(TextAsset JSON)
    {
        EntryDialogue entry = JsonUtility.FromJson<EntryDialogue>(JSON.text);
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
    public static EntryText JSONToText(TextAsset JSON)
    {
        EntryText entry = JsonUtility.FromJson<EntryText>(JSON.text);
        return entry;
    }

#if UNITY_EDITOR
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
        string mainDir = Directory.GetCurrentDirectory() + "\\Assets\\Resources\\" + PlayerPrefs.GetString("JSONDir");

        PublishToFile(JSON, mainDir + "\\JSONS\\" + origEntry.entryTitle + ".json");

        if (CheckFile(mainDir + "\\Chapters\\" + origEntry.chapter + ".asset"))
        {
            JSONChapterLibrary target = Resources.Load<JSONChapterLibrary>(PlayerPrefs.GetString("JSONDir") + "\\Chapters\\" + origEntry.chapter);
            Object JSONFile = Resources.Load(PlayerPrefs.GetString("JSONDir") + "\\JSONS\\" + origEntry.entryTitle);
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
        AssetDatabase.Refresh();
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
#endif
}
