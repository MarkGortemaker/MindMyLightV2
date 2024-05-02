using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GUILook
{
    #region GUILayoutOptions
    public static GUILayoutOption[] bigTextBox = new GUILayoutOption[]
    {
        GUILayout.MinWidth(250),
        GUILayout.MaxWidth(1200),
        GUILayout.MinHeight(150),
        GUILayout.ExpandHeight(true)
    };

    public static GUILayoutOption[] thinTextBox = new GUILayoutOption[]
    {
        GUILayout.MinWidth(50),
        GUILayout.MaxWidth(1200),
        GUILayout.ExpandHeight(false)
    };

    public static GUILayoutOption[] labelLayout = new GUILayoutOption[]
    {GUILayout.MinWidth(50), GUILayout.MaxWidth(300)};

    public static GUILayoutOption[] vertFieldsLayout = new GUILayoutOption[]
    {GUILayout.MinWidth(50), GUILayout.MaxWidth(1200)};

    public static GUILayoutOption[] toolbarLayout = new GUILayoutOption[] { GUILayout.Width(55) };
    #endregion GUILayoutOptions

    static RectOffset rectOffset = new(1, 1, 1, 1);
    public static GUIStyle leftAlignedText = new GUIStyle { alignment = TextAnchor.UpperLeft, border = rectOffset};
}
