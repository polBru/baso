using UnityEditor;
using static Card;

[CustomEditor(typeof(Card))]
[CanEditMultipleObjects]
public class CardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var card = target as Card;

        card.type = (Type)EditorGUILayout.EnumPopup("Type", card.type);
        card.content = EditorGUILayout.TextField("Conent", card.content);
        card.price.type = (PriceType)EditorGUILayout.EnumPopup("Price Type", card.price.type);
        if (card.price.type != PriceType.None)
            card.price.number = EditorGUILayout.IntField("Price", card.price.number);
        if (card.price.number < 0) 
            card.price.number = 0;
    }
}