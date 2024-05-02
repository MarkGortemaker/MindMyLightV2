using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class JSONView : EditorWindow
{
    static JSONChapterLibrary[] chapters;
    static string[] chaptersStrings;
    int chapterSelect = -1;

    Vector2 textScrollPos;
    Vector2 diaScrollPos;


    [MenuItem("Window/Dialogue and Text System/File Viewer")]
    public static void ShowWindow()
    {
        System.Type[] desiredDock = { typeof(TextEditor), typeof(DialogueEditor) };
        JSONView window = GetWindow<JSONView>("File Viewer", desiredDock);
        SortFiles();
    }

    private void OnEnable()
    {
        SortFiles();        
    }

    static void SortFiles()
    {
        chapters = Resources.LoadAll<JSONChapterLibrary>("DiaTextSystem\\Chapters");
        chaptersStrings = new string[chapters.Length];
        for (int i = 0; i < chapters.Length; i++)
        {
            chaptersStrings[i] = "Chapter " + chapters[i].name;
            Debug.Log(chapters[i]);
        }
    }

    private void OnGUI()
    {
        chapterSelect = EditorGUILayout.Popup(chapterSelect, chaptersStrings, GUILook.thinTextBox);

        if (chapterSelect >= 0)
        { DrawChapter(chapterSelect); }
    }

    void DrawChapter(int index)
    {
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        textScrollPos = GUILayout.BeginScrollView(textScrollPos, false, true);

        GUILayout.Label("Texts", GUILook.labelLayout);
        for (int i = 0; i < chapters[index].textJSONList.Count; i++)
        {
            if (GUILayout.Button(chapters[index].textJSONList[i].name ,GUILook.thinTextBox)) { LoadOptions(chapters[index].textJSONList[i]); }
        }

        GUILayout.EndScrollView();
        GUILayout.EndVertical();


        GUILayout.BeginVertical();
        diaScrollPos = GUILayout.BeginScrollView(diaScrollPos, false, true);

        GUILayout.Label("Dialogues", GUILook.labelLayout);
        for (int i = 0; i < chapters[index].dialogueJSONList.Count; i++)
        {
            if (GUILayout.Button(chapters[index].dialogueJSONList[i].name, GUILook.thinTextBox)) { LoadOptions(chapters[index].dialogueJSONList[i]); }
        }

        GUILayout.EndScrollView();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }

    void LoadOptions(Object file)
    {
        CreateWindow<FileMenu>();
        FileMenu.file = file;
        FileMenu.fileDir = AssetDatabase.GetAssetPath(file);
        FileMenu.ShowWindow();
    }
}

public class FileMenu : EditorWindow
{
    public static bool isDialogue;
    public static Object file;
    public static string fileDir;

    public static string rawContent;

    public static void ShowWindow()
    {
        FileMenu window = GetWindow<FileMenu>("File Options");
        window.minSize = new(100, 225);
        window.maxSize = new(750, 600);

        using (StreamReader file = File.OpenText(fileDir))
        {
            rawContent = file.ReadToEnd();
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();

        GUILayout.Label(file.name, GUILook.thinTextBox);
        GUILayout.Box(rawContent, GUILook.leftAlignedText, GUILook.bigTextBox);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Edit", GUILook.labelLayout)) { EditFile(); }
        if (GUILayout.Button("Delete", GUILook.labelLayout)) { DeleteFile(); }

        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }

    void DeleteFile()
    {
        //using (StreamReader file = File.Delete(fileDir)) ;
    }

    void EditFile()
    {
        //UnJSON the file, and send it to it's respective editor.
    }
}
