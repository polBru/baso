using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Deck/New Deck")]
public class Deck : ScriptableObject {
    public Card basoCard;
    public List<Card> eventCards;
    public List<Card> dareCards;
    public List<Card> truthCards;
    public List<Card> wyrCards;
    [HideInInspector] public List<Card> allCards;
}
