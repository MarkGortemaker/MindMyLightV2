using System;
using System.Reflection;
using UnityEditor;

/// <summary>
/// Class allowing for improved referencing between a class and it's editor.
/// </summary>
public static class ImprovedRef
{
    public static object GetProperty(SerializedProperty property)
    {
        return FindProperty(property).GetValue(property.serializedObject.targetObject); //Returns the value.
    }

    public static void SetProperty(SerializedProperty property, object value)
    {
        FindProperty(property).SetValue(property.serializedObject.targetObject, value); //Sets the value to 'value'
    }

    public static FieldInfo FindProperty(SerializedProperty property)
    {
        Type parentType = property.serializedObject.targetObject.GetType(); //Gets the type of the original object the serialized property is from.
        FieldInfo fi = parentType.GetField(property.propertyPath); //Creates a complete reference to the original property serialized property referred to.
        return fi;
    }
}
