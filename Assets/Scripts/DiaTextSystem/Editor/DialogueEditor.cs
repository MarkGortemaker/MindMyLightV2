using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// A Unity Editor Window class for writing dialogue for, or editing dialogue from a JSON file.
/// </summary>
public class DialogueEditor : EditorWindow
{
    public string titleText;  //The title of the entry being written or edited.
    public List<string> lines = new(); //All dialogue lines.
    public List<int> speakers = new(); //All speakers corresponding to dialogue lines.
    public string nextUpDialogue; //The entry which will follow the entry being written or edited.
    public static string[] allChapters; //All existing Chapters.
    public int chapter; // The selected chapter (JSONChapterLibrary Object) to which the entry being written or edited belongs.

    [Tooltip("Current speaker speaking the line in 'dialogueLine'")] 
    public int speaker;
    [Tooltip("Current dialogue line being spoken by 'speaker'.")] 
    public string dialogueLine;

    [Tooltip("List of characters.")] public List<string> characters = new();

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
        set { if (value) { SelectCharacter(""); } }
    }
#endregion Button Bools

    bool wipeOnSave = false; //Whether the editor wipes itself of all input on saving.
    bool clearOnStore = false; //Whether the line section clears itself of all input on storing a line.

    Vector2 charScrollPos; //GUI scroll position.
    Vector2 linesScrollPos; //GUI scroll position.

    #region GUILayout
    GUIContent save = new("Save", "Stores and formats full dialogue into a file with the name given in 'Entry Name'.");
    GUIContent wipe = new("Wipe", "Wipes editor clean of all input.");
    GUIContent store = new("Store", "Stores current dialogue line.");
    GUIContent clear = new("Clear", "Clears currently dialogue line.");
    GUIContent add = new("Add", "Opens character editor to add character.");
    #endregion GUILayout
    
    /// <summary>
    /// Behaviour to be called upon this window being opened. <para />
    /// Also displays the window as openable under "Window" tab.
    /// </summary>
    [MenuItem("Window/Dialogue and Text System/Dialogue to JSON")]
    public static void WindowOpen()
    {
        System.Type[] desiredDock = { typeof(JSONView), typeof(TextEditor) };
        DialogueEditor window = GetWindow<DialogueEditor>("Dialogue to JSON Editor", desiredDock);

        window.hasUnsavedChanges = true;
        window.minSize = new Vector2(400, 400);
        DialogueEditor.SortChapters();
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
        JSONChapterLibrary[] chapters = Resources.LoadAll<JSONChapterLibrary>(PlayerPrefs.GetString("JSONDir") + "\\Chapters");
        allChapters = new string[chapters.Length];
        for (int i = 0; i < chapters.Length; i++)
        { allChapters[i] = chapters[i].name; }
        Debug.Log("Sorted Chapters");
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

    /// <summary>
    /// Draws the character list.
    /// </summary>
    void DrawCharacterList()
    {
        GUILayout.BeginVertical(GUILook.vertFieldsLayout); //Character List Vertical container START 

        charScrollPos = GUILayout.BeginScrollView(charScrollPos, false, true, GUILook.vertFieldsLayout);
        GUILayout.Label("Characters", GUILook.labelLayout);

        //For every existing character, adds a button with the characters name.
        for (int i = 0; i < characters.Count; i++)
        {
            //Gives button functionality.
            if (GUILayout.Button(characters[i], GUILook.labelLayout))
            { SelectCharacter(characters[i]); }
        }

        GUILayout.EndScrollView();

        adding = GUILayout.Button(add); //Add character button.
        GUILayout.EndVertical(); //Character List Vertical container END
    }

    /// <summary>
    /// Draws the field the speaker is selected in, and the field the dialogue is written in.
    /// </summary>
    void DrawDialogueFields()
    {
        GUILayout.BeginVertical(GUILook.vertFieldsLayout); //Dialogue Fields Vertical container START
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Speaking:", GUILook.labelLayout); //Label
        speaker = EditorGUILayout.Popup(speaker, characters.ToArray(), GUILook.thinTextBox); //Speaker selection dropdown field.
        GUILayout.EndHorizontal();

        GUILayout.Label("Dialogue Text", GUILook.labelLayout); //Label
        GUILayout.BeginHorizontal();
        dialogueLine = EditorGUILayout.TextArea(dialogueLine, GUILook.bigTextBox); //Dialogue input field.
        GUILayout.EndHorizontal();

        GUILayout.EndVertical(); //Dialogue Fields Vertical container END
    }

    /// <summary>
    /// Draws all stored lines.
    /// </summary>
    void DrawStoredLines()
    {
        GUILayout.BeginVertical(GUILook.vertFieldsLayout); //Character List Vertical container START
        GUILayout.Label("Entry Name", GUILook.labelLayout);
        titleText = EditorGUILayout.TextArea(titleText, GUILook.thinTextBox); //Label

        linesScrollPos = GUILayout.BeginScrollView(linesScrollPos, false, true, GUILook.vertFieldsLayout);
        GUILayout.Label("Stored Lines", GUILook.labelLayout);

        //For each stored line, a button will be added with a small preview in the format of 'name: text'
        for (int i = 0; i < lines.Count; i++)
        {
            //Adds formatting and functionality.
            if (GUILayout.Button(characters[speakers[i]] + ":\n" + lines[i], GUILook.labelLayout))
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
        chapter = EditorGUILayout.Popup(chapter, allChapters, GUILook.thinTextBox); //Speaker selection dropdown field.

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
                entryType = EntryType.Dialogue,
                entryTitle = titleText,
                nextEntryTitle = nextUpDialogue,
                chapter = allChapters[chapter],

                characters = characters.ToArray(),
                characterOrder = speakers.ToArray(),
                dialogueLines = lines.ToArray()
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
        nextUpDialogue = "";
        chapter = -1;

        Clear();
    }

    public void StoreLine()
    {
        if (speaker < 0)
        { Debug.LogWarning("No speaker selected."); }
        else
        {
            lines.Add(dialogueLine);
            speakers.Add(speaker);
        }

        if (clearOnStore) { Clear(); } //If the user wishes to clear dialogue fields automatically after storing progress. This is off by default.
    }

    public void Clear()
    {
        dialogueLine = "";
        speaker = -1;
    }

    /// <summary>
    /// Button functionality which sends data to LineMenu and opens LineMenu's respective editor.
    /// </summary>
    /// <param name="index"></param>
    public void SelectLine(int index) 
    { 
        LineMenu lineMenu = GetWindow<LineMenu>();
        LineMenu.index = index;
        LineMenu.line = lines[index];
        LineMenu.speaker = speakers[index];
        LineMenu.characters = characters;
        LineMenu.WindowOpen();
    }

    /// <summary>
    /// Button functionality which sends data to CharacterMenu and opens CharacterMenu's respective editor.
    /// </summary>
    /// <param name="index"></param>
    public void SelectCharacter(string target)
    {
        CharacterMenu charMenu = GetWindow<CharacterMenu>();
        charMenu.character = target;
        charMenu.newChar = charMenu.character;
        CharacterMenu.WindowOpen();
    }


    #endregion Button Functions
}

public class LineMenu : EditorWindow
{
    public static string line;
    public static int speaker;
    public static List<string> characters;
    public static int index;

    bool saving
    {
        get { return saving; }
        set { if (value) { SendToDialogueEditor(); } }
    }
    bool deleting
    {
        get { return deleting; }
        set { if (value) { RemoveLine(); } }
    }

    GUIContent save = new("Save", "Sends line to the Dialogue Editor.");
    GUIContent delete = new("Delete", "Removes line from the Dialogue Editor.");

    public static void WindowOpen()
    {
        LineMenu window = GetWindow<LineMenu>("Line Options");
        window.minSize = new(100, 225);
        window.maxSize = new(750, 600);
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();
        
        speaker = EditorGUILayout.Popup(speaker, characters.ToArray(), GUILook.thinTextBox);
        line = GUILayout.TextField(line, GUILook.bigTextBox);

        saving = GUILayout.Button(save, GUILayout.MinWidth(50), GUILayout.MaxWidth(150));
        deleting = GUILayout.Button(delete, GUILayout.MinWidth(50), GUILayout.MaxWidth(150));

        GUILayout.EndVertical();
    }

    void SendToDialogueEditor() 
    {
        DialogueEditor dialogueEditor = GetWindow<DialogueEditor>();

        dialogueEditor.lines[index] = line;
        dialogueEditor.speakers[index] = speaker;

        CloseWindow();
    }

    void RemoveLine()
    {
        DialogueEditor dialogueEditor = GetWindow<DialogueEditor>();

        dialogueEditor.lines.RemoveAt(index);
        dialogueEditor.speakers.RemoveAt(index);

        CloseWindow();
    }

    void CloseWindow()
    {
        LineMenu window = GetWindow<LineMenu>("Line Options");
        window.Close();
    }
}

public class CharacterMenu : EditorWindow 
{
    char[] invalidChars = "!@#$%^*()_+-={}[]|\\:;'\"<>./".ToCharArray();
    string warningString = "";

    public string character;
    public string newChar;

    bool saving
    {
        get { return saving; }
        set { if (value) { SendToDialogueEditor(); } }
    }
    bool deleting
    {
        get { return deleting; }
        set { if (value) { RemoveCharacter(); } }
    }

    GUIContent save = new("Save", "Sends character to the Dialogue Editor.");
    GUIContent delete = new("Delete", "Removes line from the Dialogue Editor.");

    /// <summary>
    /// Behaviour to be called upon this window being opened.
    /// </summary>
    public static void WindowOpen()
    {
        CharacterMenu window = GetWindow<CharacterMenu>("Character Options");
        window.minSize = new(100, 225);
        window.maxSize = new(750, 600);
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();

        newChar = GUILayout.TextField(newChar, GUILook.thinTextBox);

        saving = GUILayout.Button(save, GUILayout.MinWidth(50), GUILayout.MaxWidth(150));
        deleting = GUILayout.Button(delete, GUILayout.MinWidth(50), GUILayout.MaxWidth(150));

        GUILayout.EndVertical();
    }

    /// <summary>
    /// Sends the data in the textfield of this menu back to the Dialogue Editor. <para />
    /// If the name in the textfield is new, it will create a new character. 
    /// Otherwise, it overwrites an existing characters name.
    /// </summary>
    void SendToDialogueEditor()
    {
        DialogueEditor target = GetWindow<DialogueEditor>();
        int index = target.characters.IndexOf(character);
        if (!CheckName(newChar)) { return; }

        if (index < 0) //If this character doesn't exist.
        { target.characters.Add(newChar); }
        else
        { target.characters[index] = newChar; }
        CloseWindow();
    }

    /// <summary>
    /// Checks a given string against the char[] "invalidChars".
    /// </summary>
    /// <param name="str">The string to check.</param>
    /// <returns>Returns false if invalid characters are detected in the given string.</returns>
    public bool CheckName(string str)
    {
        bool validString = true;
        //Checks if any of the characters in the given character name contain invalid characters.
        for (int i = 0; i < invalidChars.Length; i++)
        { if (str.Contains(invalidChars[i])) { validString = false; } }

        //Results. If all checks are passed, the character is added to the character list. Else, the system throws warnings.
        if (validString) { return true; }
        else 
        { 
            Debug.LogWarning("Invalid Character Name. Name may not contain any of the following characters: " + warningString);
            return false;
        }
    }

    /// <summary>
    /// Formats the warning string for incorrect character usage to be accurate to the invalid character list.
    /// </summary>
    void FormatWarningString()
    {
        warningString = "";
        //Goes through each invalid character in the invalidChars list and attaches corresponding grammatical syntax.
        for (int i = 0; i < invalidChars.Length; i++)
        {
            //Determines appropriate summary syntax.
            string syntax;
            if (i < invalidChars.Length - 2) { syntax = ", "; }
            else if (i == invalidChars.Length - 2) { syntax = " or "; }
            else { syntax = "."; }

            warningString += invalidChars[i].ToString() + syntax; //Adds the invalid character plus it's summary syntax to the warning string.
        }
    }

    void RemoveCharacter()
    {
        if (!EditorUtility.DisplayDialog("Delete Character?", "Deleting a character will delete all of the characters lines. Are you sure?", "Yes", "No"))
        { return; }

        DialogueEditor target = GetWindow<DialogueEditor>();

        int toDelete = target.characters.IndexOf(character); //Finds the characters index.
        for (int i = 0; i < target.speakers.Count; i++)
        {
            if (target.speakers[i] == toDelete) //Deletes the speakers and lines of any stored dialogue if the speaker matches with the deleted character.
            { 
                target.speakers.RemoveAt(i);
                target.lines.RemoveAt(i);
                i -= 1;         //Brings the iteration back 1 step to make up for changed 'speakers' and 'lines' list length due to deletion.
            }
            else if (target.speakers[i] > toDelete) //Recalibrates the array of speakers to reference an existing character.
            { target.speakers[i] -= 1; }
        }
        target.characters.Remove(character); //Removes the character
        target.speaker = -1; //For safety, deselects characters.

        CloseWindow();
    }

    void CloseWindow()
    {
        CharacterMenu window = GetWindow<CharacterMenu>("Character Options");
        window.Close();
    }
}
