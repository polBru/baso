using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameMode")]
public class GameMode : ScriptableObject
{
    public List<Deck> decks;
    [HideInInspector] public List<Card> cards;
}

