using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking.Types;
using static Card;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(Card))]
[CanEditMultipleObjects]
public class CardEditor : Editor {
    Card script;
    List<SerializedProperty> sp;
    SerializedProperty price;
    SerializedProperty content;

    void OnEnable()
    {
        script = (Card)target;
        sp = new List<SerializedProperty>();
        MemberInfo[] mi = script.GetType().GetFields();
        for (int i = 0; i < mi.Length; i++)
        {
            sp.Add(serializedObject.FindProperty(mi[i].Name));
            if (sp[i].type == "Price")
            {
                price = sp[i];
            } else if (mi[i].Name == "content")
            {
                content = sp[i];
            }
        }
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        for (int i = 0; i < sp.Count; i++)
        {
            if (sp[i] == price)
            {
                if (script.type && script.type.hasPrice)
                {
                    EditorGUILayout.PropertyField(sp[i]);
                } else
                {
                    price.FindPropertyRelative("number").intValue = 0;
                    price.FindPropertyRelative("type").enumValueIndex = 0;
                }
            } else
            {
                EditorGUILayout.PropertyField(sp[i]);
            }
        }
        content.stringValue = script.name; //Update card content to match its name
        serializedObject.ApplyModifiedProperties();
    }
}