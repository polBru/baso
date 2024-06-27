using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameMode))]
[CanEditMultipleObjects]
public class GameModeEditor : Editor
{
    GameMode script;
    List<SerializedProperty> sp;

    void OnEnable()
    {
        script = (GameMode)target;
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
        if (script.cards == null)
            script.cards = new List<Card>();

        if (script.decks == null)
            script.decks = new List<Deck>();

        serializedObject.Update();
        script.cards.Clear();
        for (int i = 0; i < sp.Count; i++)
        {
            EditorGUILayout.PropertyField(sp[i]);
        }

        foreach (Deck deck in script.decks)
        {
            if (deck != null)
                script.cards.AddRange(deck.cards);
        }
        serializedObject.ApplyModifiedProperties();
    }
}
