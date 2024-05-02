using UnityEngine;
using UnityEditor;

public class TextEditor : EditorWindow
{
    string titleText;
    string contentText;
    string nextUpText;
    string chapter;

    bool saving
    {
        get { return saving; }
        set { if (value) { Save(); } }
    }
    bool clearing
    {
        get { return clearing; }
        set { if (value) { Clear(); } }
    }

    bool clearOnSave = false;

    #region GUILayout
    GUIContent save = new("Save", "Stores and formats currently written text into a file with the name given.");
    GUIContent clear = new("Clear", "Clears currently written text");
    #endregion GUILayout

    [MenuItem("Window/Dialogue and Text System/Text to JSON")]
    public static void ShowWindow()
    {
        System.Type[] desiredDock = { typeof(JSONView), typeof(DialogueEditor) };
        TextEditor window = GetWindow<TextEditor>("Text to JSON Editor", desiredDock);

        window.hasUnsavedChanges = true;
        window.minSize = new Vector2(400, 400);
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();

        DrawTextFields();
        DrawToolbar();

        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// Draws the main text fields which a user can utilize for entry content creation, deletion, and alteration.
    /// </summary>
    void DrawTextFields()
    {
        GUILayout.BeginVertical(GUILook.vertFieldsLayout); //Input Fields Vertical container START
        GUILayout.BeginHorizontal();
        GUILayout.Label("Entry Name", GUILook.labelLayout);
        titleText = EditorGUILayout.TextField(titleText, GUILook.thinTextBox);
        GUILayout.EndHorizontal();

        GUILayout.Label("Content Text", GUILook.labelLayout);
        GUILayout.BeginHorizontal();
        contentText = EditorGUILayout.TextArea(contentText, GUILook.bigTextBox);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Following Entry", GUILook.labelLayout);
        nextUpText = EditorGUILayout.TextField(nextUpText, GUILook.thinTextBox);
        GUILayout.EndHorizontal();

        GUILayout.EndVertical(); //Input Fields Vertical container END
    }

    /// <summary>
    /// Draws the toolbar with which a user can save or clear their work, as well as all fields with relevant information and options.
    /// </summary>
    void DrawToolbar()
    {
        GUILayout.BeginVertical(GUILook.toolbarLayout); //Toolbar Vertical container START
        saving = GUILayout.Button(save); //Save Button
        clearing = GUILayout.Button(clear); //Clear Button

        GUILayout.Label("Clear on \n Save?", GUILook.labelLayout); //ClearOnSave Label
        clearOnSave = EditorGUILayout.Toggle(clearOnSave); //ClearOnSave Toggle

        GUILayout.Label("Chapter"); //Chapter Label
        chapter = GUILayout.TextField(chapter); //Chapter Input Field
        GUILayout.EndVertical(); //Toolbar Vertical container END
    }


    /// <summary>
    /// Stores the information the user has interacted with into an entry, which is then sent off to be turned into a JSON file.
    /// </summary>
    public void Save()
    {
        Debug.Log("Saving");

        //Preps variables and appplies them to a new EntryText class which is then sent off for JSON conversion.
        if (nextUpText == "") { nextUpText = titleText; }
        EntryText entry = new()
        {
            entryTitle = titleText,
            content = contentText,
            nextEntryTitle = nextUpText,
            chapter = chapter
        };

        //Sending file to ToFromJSON for JSON conversion.
        ToFromJSON.EntryToJSON(entry);

        if (clearOnSave) { Clear(); } //If the user wishes to clear the fields automatically after saving progress. This is off by default.
    }

    /// <summary>
    /// Clears all input fields.
    /// </summary>
    public void Clear()
    {
        Debug.Log("Clearing");
        titleText = "";
        contentText = "";
        nextUpText = "";
        chapter = "";
    }
}
