using UnityEngine;
using UnityEditor;

/// <summary>
/// A Unity Editor Window class for writing text for, or editing text from a JSON file.
/// </summary>
public class TextEditor : EditorWindow
{
    public string titleText; //The title of the entry being written or edited.
    public string contentText; //The content of the entry being written or edited.
    public string nextUpText; //The entry which will follow the entry being written or edited.
    public static string[] allChapters; //All existing Chapters.
    public int chapter; // The selected chapter (JSONChapterLibrary Object) to which the entry being written or edited belongs.

    private bool saving //Button boolean, when true it calls Save();
    {
        get { return saving; }
        set { if (value) { Save(); } }
    }
    private bool clearing //Button boolean, when true it calls Clear();
    {
        get { return clearing; }
        set { if (value) { Clear(); } }
    }

    private bool clearOnSave = false; //Boolean deciding if text fields are cleared after saving.

    #region GUILayout
    GUIContent save = new("Save", "Stores and formats currently written text into a file with the name given.");
    GUIContent clear = new("Clear", "Clears currently written text");
    #endregion GUILayout

    /// <summary>
    /// Behaviour to be called upon this window being opened. <para />
    /// Also displays the window as openable under "Window" tab.
    /// </summary>
    [MenuItem("Window/Dialogue and Text System/Text to JSON")]
    public static void WindowOpen()
    {
        System.Type[] desiredDock = { typeof(JSONView), typeof(DialogueEditor) };
        TextEditor window = GetWindow<TextEditor>("Text to JSON Editor", desiredDock);

        window.hasUnsavedChanges = true;
        window.minSize = new Vector2(400, 400);
        TextEditor.SortChapters();
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
        JSONChapterLibrary[] chapters = Resources.LoadAll<JSONChapterLibrary>(PlayerPrefs.GetString("JSONDir") + "Chapters");
        allChapters = new string[chapters.Length];
        for (int i = 0; i < chapters.Length; i++)
        { allChapters[i] = chapters[i].name; }
        Debug.Log("Sorted Chapters");
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();

        DrawTextFields();
        DrawToolbar();

        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// Draws the main text fields which a user can utilize for entry content creation, deletion, and alteration.
    /// </summary>
    private void DrawTextFields()
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
    private void DrawToolbar()
    {
        GUILayout.BeginVertical(GUILook.toolbarLayout); //Toolbar Vertical container START
        saving = GUILayout.Button(save); //Save Button
        clearing = GUILayout.Button(clear); //Clear Button

        GUILayout.Label("Clear on \n Save?", GUILook.labelLayout); //ClearOnSave Label
        clearOnSave = EditorGUILayout.Toggle(clearOnSave); //ClearOnSave Toggle

        GUILayout.Label("Chapter"); //Chapter Label
        chapter = EditorGUILayout.Popup(chapter, allChapters, GUILook.thinTextBox); //Speaker selection dropdown field.
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
            entryType = EntryType.Text,
            entryTitle = titleText,
            nextEntryTitle = nextUpText,
            chapter = allChapters[chapter],

            content = contentText
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
        chapter = -1;
    }
}
