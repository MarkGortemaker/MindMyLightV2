using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "JSON Chapter Library", menuName = "Dialogue and Text System/JSON Chapter Library")]
public class JSONChapterLibrary : ScriptableObject
{
    public List<Object> textJSONList;
    public List<Object> dialogueJSONList;
}
