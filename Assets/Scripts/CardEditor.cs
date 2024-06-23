using UnityEditor;
using UnityEngine.Networking.Types;
using static Card;

[CustomEditor(typeof(Card))]
[CanEditMultipleObjects]
public class CardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var card = target as Card;

        card.type = (CardType)EditorGUILayout.ObjectField(card.type, typeof(CardType), true);
        card.content = EditorGUILayout.TextField("Conent", card.content);
        card.price.type = (PriceType)EditorGUILayout.EnumPopup("Price Type", card.price.type);
        if (card.price.type != PriceType.None)
            card.price.number = EditorGUILayout.IntField("Price", card.price.number);
        if (card.price.number < 0) 
            card.price.number = 0;
    }
}