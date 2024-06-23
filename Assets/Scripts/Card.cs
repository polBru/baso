using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/New Card")]
public class Card : ScriptableObject
{
    public enum PriceType
    {
        [Description("Bebe @% tragos")]
        Sip,
        [Description("Termina @% vasos")]
        Finish,
        [Description("")]
        None
    }

    [System.Serializable]
    public struct Price
    {
        public PriceType type;
        [Min(0)]
        public int number;
    }

    public CardType type;
    public string content;
    public Price price;
}
