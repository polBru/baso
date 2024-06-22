using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/New Card")]
public class Card : ScriptableObject
{
    public enum Type {
        [Description("Evento")]
        Event,
        [Description("Verdad")]
        Truth,
        [Description("Reto")]
        Dare,
        [Description("Qué prefieres?")]
        Prefer,
        [Description("BASO")]
        BASO
    }
    public enum PriceType
    {
        [Description("Bebe")]
        Sip,
        [Description("Termina")]
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

    public Type type;
    public string content;
    public Price price;
}
