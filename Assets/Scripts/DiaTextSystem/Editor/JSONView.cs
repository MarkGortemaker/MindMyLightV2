using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// A Unity Editor Window class for sorting through existing JSON files.
/// </summary>
public class JSONView : EditorWindow
{
    public static JSONChapterLibrary[] chapters; //The JSONChapterLibrary Scriptable Objects that hold references to JSON files.
    private static string[] chaptersStrings; //
    private int chapterSelect = -1; //Chapter Selection Index. Is used for the chapter dropdown in the editor window.

    private Vector2 textScrollPos; //GUI scroll position.
    private Vector2 diaScrollPos; //GUI scroll position.

    GUIContent local = new("Localization", "Choose which localization to work in.");

    /// <summary>
    /// Behaviour to be called upon this window being opened. <para /> 
    /// Also displays the window as openable under "Window" tab.
    /// </summary>
    [MenuItem("Window/Dialogue and Text System/File Viewer")]
    public static void WindowOpen()
    {
        System.Type[] desiredDock = { typeof(TextEditor), typeof(DialogueEditor) };
        JSONView window = GetWindow<JSONView>("File Viewer", desiredDock);
        SortChapters();
    }

    private void OnEnable()
    { 
        AssemblyReloadEvents.afterAssemblyReload += onAfterReload; 
        SortChapters();
    }
    private void OnDisable()
    { AssemblyReloadEvents.afterAssemblyReload -= onAfterReload; }
    private void onAfterReload()
    {
        Debug.LogWarning("Post-load");
        SortChapters();
    }

    /// <summary>
    /// Goes through all of the JSONChapterLibrary objects and sorts out their files.
    /// </summary>
    public static void SortChapters()
    {
        chapters = Resources.LoadAll<JSONChapterLibrary>(PlayerPrefs.GetString("JSONDir") + "\\Chapters");
        chaptersStrings = new string[chapters.Length];
        for (int i = 0; i < chapters.Length; i++)
        { chaptersStrings[i] = "Chapter " + chapters[i].name; }
        TextEditor.SortChapters();
        DialogueEditor.SortChapters();
        Debug.Log("Sorted Chapters");
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        chapterSelect = EditorGUILayout.Popup(chapterSelect, chaptersStrings, GUILook.thinTextBox);
        if (GUILayout.Button(local, GUILook.labelLayout)) 
        { 
            CreateWindow<LocalizationMenu>();
            LocalizationMenu.ShowWindow();
        }
        GUILayout.EndHorizontal();

        if (chapterSelect >= 0) //If a chapter is selected, draws the environment.
        { DrawChapter(chapterSelect); }
    }

    /// <summary>
    /// Draws the editor window based off of which chapter is selected.
    /// </summary>
    /// <param name="index"></param>
    void DrawChapter(int index)
    {
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        textScrollPos = GUILayout.BeginScrollView(textScrollPos, false, true);

        GUILayout.Label("Texts", GUILook.labelLayout); //Label
        //Draws all the text file buttons.
        for (int i = 0; i < chapters[index].textJSONList.Count; i++)
        {
            //Gives button functionality.
            if (GUILayout.Button(chapters[index].textJSONList[i].name ,GUILook.thinTextBox)) { LoadOptions(chapters[index].textJSONList[i]); }
        }

        GUILayout.EndScrollView();
        GUILayout.EndVertical();


        GUILayout.BeginVertical();
        diaScrollPos = GUILayout.BeginScrollView(diaScrollPos, false, true);

        GUILayout.Label("Dialogues", GUILook.labelLayout); //Label
        //Draws all the dialogue file buttons
        for (int i = 0; i < chapters[index].dialogueJSONList.Count; i++)
        {
            //Gives button functionality.
            if (GUILayout.Button(chapters[index].dialogueJSONList[i].name, GUILook.thinTextBox)) { LoadOptions(chapters[index].dialogueJSONList[i]); }
        }

        GUILayout.EndScrollView();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// Calls the FileMenu window for further options once a file has been selected.
    /// </summary>
    /// <param name="file"></param>
    void LoadOptions(Object file)
    {
        CreateWindow<FileMenu>();
        FileMenu.file = file;
        FileMenu.fileDir = AssetDatabase.GetAssetPath(file);
        FileMenu.chapter = chapters[chapterSelect];
        FileMenu.ShowWindow();
    }
}

public class FileMenu : EditorWindow
{
    public static Object file;
    public static string fileDir;
    public static JSONChapterLibrary chapter;

    public static string rawContent;

    /// <summary>
    /// Behaviour to be called upon this window being opened.
    /// </summary>
    public static void ShowWindow()
    {
        //Calls windows and sets sizes.
        FileMenu window = GetWindow<FileMenu>("File Options");
        window.minSize = new(100, 225);
        window.maxSize = new(750, 600);

        //Opens the file provided by the JSONView window at the path of "fileDir".
        using (StreamReader file = File.OpenText(fileDir))
        {
            rawContent = file.ReadToEnd();
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();

        GUILayout.Label(file.name, GUILook.thinTextBox); //File name label
        GUILayout.Box(rawContent, GUILook.leftAlignedText, GUILook.bigTextBox); //File content box.

        GUILayout.BeginHorizontal();
        
        //Draws Edit and Delete buttons.
        if (GUILayout.Button("Edit", GUILook.labelLayout)) { EditFile(); }
        if (GUILayout.Button("Delete", GUILook.labelLayout)) { DeleteFile(); }

        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }

    /// <summary>
    /// Deletes the file located at path "fileDir".
    /// </summary>
    void DeleteFile()
    {
        if (!EditorUtility.DisplayDialog("Delete file?","This action cannot be reversed. Are you sure?", "Yes", "No"))
        { return; }

        //Converts the file located at path "fileDir" from a JSON file into a BaseEntry class
        //to find it's corresponding list and prevent Missing type references.
        BaseEntry bE = ToFromJSON.JSONToEntry(fileDir);
        switch (bE.entryType) //Checks for the entry type.
        {
            case EntryType.Text: chapter.textJSONList.Remove(file);
                break;
            case EntryType.Dialogue: chapter.dialogueJSONList.Remove(file);
                break;
        }
        
        File.Delete(fileDir);
        EditorUtility.SetDirty(chapter);
    }

    void EditFile()
    {
        //Converts the file located at path "fileDir" from a JSON file into a BaseEntry class.
        BaseEntry bE = ToFromJSON.JSONToEntry(fileDir); 
        switch (bE.entryType) //Checks for the entry type.
        {
            case EntryType.Text: OpenWithTextEditor();
                break;
            case EntryType.Dialogue: OpenWithDialogueEditor();
                break;
        }
    }

    /// <summary>
    /// Opens the file located at the path "fileDir" with the Text Editor.
    /// </summary>
    void OpenWithTextEditor()
    {
        EntryText entry = ToFromJSON.JSONToText(fileDir);
        TextEditor target = GetWindow<TextEditor>();
        target.titleText = entry.entryTitle;
        target.nextUpText = entry.nextEntryTitle;
        target.chapter = System.Array.IndexOf(TextEditor.allChapters, entry.chapter);

        target.contentText = entry.content;
    }


    /// <summary>
    /// Opens the file located at the path "fileDir" with the Dialogue Editor.
    /// </summary>
    void OpenWithDialogueEditor()
    {
        EntryDialogue entry = ToFromJSON.JSONToDialogue(fileDir);
        DialogueEditor target = GetWindow<DialogueEditor>();
        target.titleText = entry.entryTitle;
        target.nextUpDialogue = entry.nextEntryTitle;
        target.chapter = System.Array.IndexOf(DialogueEditor.allChapters, entry.chapter);

        target.characters = new(entry.characters);
        target.speakers = new(entry.characterOrder);
        target.lines = new(entry.dialogueLines);
    }
}
