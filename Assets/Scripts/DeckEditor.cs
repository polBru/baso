using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Deck))]
[CanEditMultipleObjects]
public class DeckEditor : Editor
{
    Deck script;
    List<SerializedProperty> sp;
    SerializedProperty price;

    void OnEnable()
    {
        script = (Deck)target;
        sp = new List<SerializedProperty>();
        MemberInfo[] mi = script.GetType().GetFields();
        for (int i = 0; i < mi.Length; i++)
        {
            if (mi[i].IsDefined(typeof(HideInInspector)) == false)
            {
                sp.Add(serializedObject.FindProperty(mi[i].Name));
            }
        }
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        for (int i = 0; i < sp.Count; i++)
        {
            EditorGUILayout.PropertyField(sp[i]);
        }
        script.allCards.AddRange(script.eventCards);
        script.allCards.AddRange(script.truthCards);
        script.allCards.AddRange(script.dareCards);
        script.allCards.AddRange(script.wyrCards);
        script.allCards.Add(script.basoCard);
        serializedObject.ApplyModifiedProperties();
    }
}

