using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DialogueEditor : EditorWindow
{
    char[] invalidChars = "!@#$%^*()_+-={}[]|\\:;'\"<>./".ToCharArray();
    string warningString = "";

    string titleText;
    public List<string> lines = new();
    public List<string> speakers = new();
    string nextUpDialogue;
    string chapter;

    public int speaker;
    public string dialogueLine;

    string character;
    public List<string> characters = new();

#region Button Bools
    bool saving
    {
        get { return saving; }
        set { if (value) { Save(); } }
    }
    bool wiping
    {
        get { return wiping; }
        set { if (value) { Wipe(); } }
    }
    bool storing
    {
        get { return storing; }
        set { if (value) { StoreLine(); } }
    }
    bool clearing
    {
        get { return clearing; }
        set { if (value) { Clear(); } }
    }
    bool adding
    {
        get { return adding; }
        set { if (value) { AddCharacter(); } }
    }
#endregion Button Bools

    bool wipeOnSave = false;
    bool clearOnStore = false;

    Vector2 charScrollPos;
    Vector2 linesScrollPos;

    #region GUILayout
    GUIContent save = new("Save", "Stores and formats full dialogue into a file with the name given in 'Entry Name'.");
    GUIContent wipe = new("Wipe", "Wipes editor clean of all input.");
    GUIContent store = new("Store", "Stores current dialogue line.");
    GUIContent clear = new("Clear", "Clears currently dialogue line.");
    GUIContent add = new("Add", "Adds current character name to the character list.");
    #endregion GUILayout

    [MenuItem("Window/Dialogue and Text System/Dialogue to JSON")]
    public static void ShowWindow()
    {
        System.Type[] desiredDock = { typeof(JSONView), typeof(TextEditor) };
        DialogueEditor window = GetWindow<DialogueEditor>("Dialogue to JSON Editor", desiredDock);

        window.hasUnsavedChanges = true;
        window.minSize = new Vector2(400, 400);

        window.FormatWarningString();
    }

    void FormatWarningString()
    {
        //Formats the warning string for incorrect character usage to be accurate to the invalid character list.
        for (int i = 0; i < invalidChars.Length; i++)
        {
            string summarySign;
            if (i < invalidChars.Length - 2) { summarySign = ", "; }
            else if (i == invalidChars.Length - 2) { summarySign = " or "; }
            else { summarySign = "."; }
            warningString += invalidChars[i].ToString() + summarySign;
        }
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();

        DrawCharacterList();
        GUILayout.Space(10);
        DrawDialogueFields();
        GUILayout.Space(10);
        DrawStoredLines();
        DrawToolbar();

        GUILayout.EndHorizontal();
    }

    void DrawCharacterList()
    {
        GUILayout.BeginVertical(GUILook.vertFieldsLayout); //Character List Vertical container START 
        GUILayout.Label("Character", GUILook.labelLayout);
        character = EditorGUILayout.TextField(character, GUILook.thinTextBox);

        charScrollPos = GUILayout.BeginScrollView(charScrollPos, false, true, GUILook.vertFieldsLayout);
        GUILayout.Label("Characters", GUILook.labelLayout);

        for (int i = 0; i < characters.Count; i++)
        {
            if (GUILayout.Button(characters[i], GUILook.labelLayout))
            { SelectCharacter(characters[i]); }
        }

        GUILayout.EndScrollView();

        adding = GUILayout.Button(add);
        GUILayout.EndVertical(); //Character List Vertical container END
    }

    void DrawDialogueFields()
    {
        GUILayout.BeginVertical(GUILook.vertFieldsLayout); //Dialogue Fields Vertical container START
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Speaking:", GUILook.labelLayout);
        speaker = EditorGUILayout.Popup(speaker, characters.ToArray(), GUILook.thinTextBox);
        GUILayout.EndHorizontal();

        GUILayout.Label("Dialogue Text", GUILook.labelLayout);
        GUILayout.BeginHorizontal();
        dialogueLine = EditorGUILayout.TextArea(dialogueLine, GUILook.bigTextBox);
        GUILayout.EndHorizontal();

        GUILayout.EndVertical(); //Dialogue Fields Vertical container END
    }

    void DrawStoredLines()
    {
        GUILayout.BeginVertical(GUILook.vertFieldsLayout); //Character List Vertical container START
        GUILayout.Label("Entry Name", GUILook.labelLayout);
        titleText = EditorGUILayout.TextArea(titleText, GUILook.thinTextBox);

        linesScrollPos = GUILayout.BeginScrollView(linesScrollPos, false, true, GUILook.vertFieldsLayout);
        GUILayout.Label("Stored Lines", GUILook.labelLayout);

        for (int i = 0; i < lines.Count; i++)
        {
            if (GUILayout.Button(characters[i] + ":\n" + lines[i], GUILook.labelLayout))
            { SelectLine(i); }
        }

        GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }

    /// <summary>
    /// Draws the toolbar with which a user can save or clear their work, as well as all fields with relevant information and options.
    /// </summary>
    void DrawToolbar()
    {
        GUILayout.BeginVertical(GUILook.toolbarLayout); //Toolbar Vertical container START

    // -- Dialogue Tools -- //
        GUILayout.Label("Dialogue\nTools");
        storing = GUILayout.Button(store); //Store Button
        clearing = GUILayout.Button(clear); //Clear Button

        GUILayout.Label("Clear on \nStore?", GUILook.labelLayout); //ClearOnSave Label
        clearOnStore = EditorGUILayout.Toggle(clearOnStore); //ClearOnSave Toggle

        GUILayout.Space(50);

    // -- File Tools -- //
        GUILayout.Label("File Tools");
        saving = GUILayout.Button(save); //Save Button
        wiping = GUILayout.Button(wipe); //Wipe Button

        GUILayout.Label("Wipe on \nSave?", GUILook.labelLayout); //ClearOnSave Label
        wipeOnSave = EditorGUILayout.Toggle(wipeOnSave); //ClearOnSave Toggle

        GUILayout.Label("Chapter"); //Chapter Label
        chapter = GUILayout.TextField(chapter); //Chapter Input Field

        GUILayout.EndVertical(); //Toolbar Vertical container END
    }

#region Button Functions
    /// <summary>
    /// Stores the information the user has interacted with into an entry, which is then sent off to be turned into a JSON file.
    /// </summary>
    public void Save()
    {
        Debug.Log("Saving");

        //Preps variables and appplies them to a new EntryDialogue class which is then sent off for JSON conversion.
        if (nextUpDialogue == "") { nextUpDialogue = titleText; }
        if (titleText == "") { Debug.LogWarning("No entry name given."); }
        else
        {
            EntryDialogue entry = new()
            {
                entryTitle = titleText,
                dialogueLines = lines.ToArray(),
                characterOrder = speakers.ToArray(),
                nextEntryTitle = nextUpDialogue,
                chapter = chapter
            };
            //Sending file to ToFromJSON for JSON conversion.
            ToFromJSON.EntryToJSON(entry);
        }

        if (wipeOnSave) { Wipe(); } //If the user wishes to wipe all fields automatically after saving file. This is off by default.
    }

    /// <summary>
    /// Clears all input fields.
    /// </summary>
    public void Wipe()
    {
        Debug.Log("Wiping");
        titleText = "";
        lines.Clear();
        speakers.Clear();
        characters.Clear();
        character = "";
        nextUpDialogue = "";
        chapter = "";

        Clear();
    }

    public void StoreLine()
    {
        if (speaker < 0)
        { Debug.LogWarning("No speaker selected."); }
        else
        {
            lines.Add(dialogueLine);
            speakers.Add(characters[speaker]);
        }


        if (clearOnStore) { Clear(); } //If the user wishes to clear dialogue fields automatically after storing progress. This is off by default.
    }

    public void Clear()
    {
        dialogueLine = "";
        speaker = -1;
    }
    public void AddCharacter() 
    {
        bool validString = true;
        //Checks if any of the characters in the given character name contain invalid characters.
        for (int i = 0; i < invalidChars.Length; i++)
        { if (character.Contains(invalidChars[i])) { validString = false; } }

        //Results. If all checks are passed, the character is added to the character list. Else, the system throws warnings.
        if (validString && !characters.Contains(character)) { characters.Add(character); }
        else if (!validString) { Debug.LogWarning("Invalid Character Name. Name may not contain any of the following characters: " + warningString); }
        else if (characters.Contains(character)) { Debug.LogWarning("This name already exists."); }
    }

    public void SelectCharacter(string target)
    {
        CharacterMenu charMenu = CreateWindow<CharacterMenu>();
        character = target;
    }


    public void SelectLine(int index) 
    { 
        LineMenu lineMenu = CreateWindow<LineMenu>();
        lineMenu.line = lines[index];
        lineMenu.character = speakers[index];
        LineMenu.ShowWindow();
    }

    #endregion Button Functions
}

public class LineMenu : EditorWindow
{
    public string line;
    public string character;

    bool editing
    {
        get { return editing; }
        set { if (value) { SendToDialogueEditor(); } }
    }
    bool deleting
    {
        get { return deleting; }
        set { if (value) { RemoveLine(); } }
    }

    GUIContent edit = new("Edit", "Sends line to the Dialogue Editor.");
    GUIContent delete = new("Delete", "Removes line from the Dialogue Editor.");

    public static void ShowWindow()
    {
        LineMenu window = GetWindow<LineMenu>("Line Menu");
        window.minSize = new(100, 225);
        window.maxSize = new(750, 600);
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();

        GUILayout.Box(character, GUILook.thinTextBox);
        GUILayout.Box(line, GUILook.bigTextBox);

        editing = GUILayout.Button(edit, GUILayout.MinWidth(50), GUILayout.MaxWidth(150));
        deleting = GUILayout.Button(delete, GUILayout.MinWidth(50), GUILayout.MaxWidth(150));

        GUILayout.EndVertical();
    }

    void SendToDialogueEditor() 
    {
        DialogueEditor dialogueEditor = GetWindow<DialogueEditor>();

        dialogueEditor.dialogueLine = line;
        dialogueEditor.speaker = dialogueEditor.characters.IndexOf(character);

        CloseWindow();
    }

    void RemoveLine()
    {
        DialogueEditor dialogueEditor = GetWindow<DialogueEditor>();

        dialogueEditor.lines.Remove(line);
        dialogueEditor.speakers.Remove(character);

        CloseWindow();
    }

    void CloseWindow()
    {
        LineMenu window = GetWindow<LineMenu>("Line Menu");
        window.Close();
    }
}

public class CharacterMenu : EditorWindow 
{
    public string character;
}
