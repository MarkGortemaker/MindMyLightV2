using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static ImprovedRef;

[CustomEditor(typeof(TextReader))]
public class TextReaderEditor : Editor
{
    JSONChapterLibrary[] chapters; //All JSONChapterLibrarys.
    string[] chaptersString; //All from "chapters", but in string format.
    SerializedProperty chapter; //Reference to EntryReader "chapter"
    private int _chapterChoice = -1; //Current chapter chosen.
    int chapterChoice //If this value is updated, calls several functions.
    {
        get { return _chapterChoice; }
        set 
        {
            if (value == _chapterChoice) { return; }
            _chapterChoice = value;
            SetProperty(chapter, chapters[value]);
            UpdateFiles();
        }
    }

    Object[] files; //All files referenced in the chosen JSONChapterLibrary.
    string[] filesString; //All from "files", but in string format.
    SerializedProperty file; //Reference to EntryReader "toRead"
    private int _fileChoice = -1; //Current file chosen.
    int fileChoice //If this value is updated, calls several functions.
    {
        get { return _fileChoice; }
        set
        {
            if (value == _fileChoice) { return; }
            _fileChoice = value;
            SetProperty(file, files[value]);
        }
    }

    private void OnEnable()
    {
        chapters = Resources.LoadAll<JSONChapterLibrary>("DiaTextSystem\\Chapters"); //Finds all JSONChapterLibrary objects in the given path.
        chaptersString = new string[chapters.Length]; 
        for (int i = 0; i < chapters.Length; i++) { chaptersString[i] = "Chapter " + chapters[i].name; } //Creates strings for displaying names for the dropdown field.

        chapter = serializedObject.FindProperty("chapter"); //Finds the serialized property for the JSONChapterLibrary
        file = serializedObject.FindProperty("toRead"); //Finds the serialized property for the JSON file to be read.

        chapterChoice = System.Array.IndexOf(chapters, GetProperty(chapter)); //Gets the index of the JSONChapterLibrary object in TextReader in the 'chapters' list
        fileChoice = System.Array.IndexOf(files, GetProperty(file)); //Gets the index of the JSON file in TextReader in the 'files' list.
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        chapterChoice = EditorGUILayout.Popup("Chapter:", chapterChoice, chaptersString, GUILook.thinTextBox); //Dropdown for chapter selection
        
        if (chapterChoice < 0) //Ends GUI if a valid chapter is not selected.
        {
            serializedObject.ApplyModifiedProperties();
            return;
        }

        fileChoice = EditorGUILayout.Popup("Entry:", fileChoice, filesString, GUILook.thinTextBox); //Dropdown for file selection 

        serializedObject.ApplyModifiedProperties();
    }

    public void UpdateFiles()
    {
        JSONChapterLibrary _ = (JSONChapterLibrary)GetProperty(chapter);
        files = _.textJSONList.ToArray();
        filesString = new string[files.Length];
        for (int i = 0; i < files.Length; i++) { filesString[i] = files[i].name; }
    }
}