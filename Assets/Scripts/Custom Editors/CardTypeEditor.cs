using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CardType))]
[CanEditMultipleObjects]
public class CardTypeEditor : Editor
{
    CardType script;
    List<SerializedProperty> sp;
    SerializedProperty price;

    void OnEnable()
    {
        script = (CardType)target;
        sp = new List<SerializedProperty>();
        MemberInfo[] mi = script.GetType().GetFields();
        for (int i = 0; i < mi.Length; i++)
        {
            sp.Add(serializedObject.FindProperty(mi[i].Name));
        }
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        for (int i = 0; i < sp.Count; i++)
        {
            EditorGUILayout.PropertyField(sp[i]);
        }
        serializedObject.ApplyModifiedProperties();
    }
}
