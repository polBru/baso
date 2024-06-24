using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/New Card Type")]
public class CardType : ScriptableObject
{
    public new string name;
    public Color color;
    public bool hasPrice;
    public bool showName;
}
