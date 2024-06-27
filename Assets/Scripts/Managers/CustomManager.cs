using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CustomManager : MonoBehaviour
{
    [SerializeField]
    private Toggle dareToggle;
    [SerializeField]
    private Toggle truthToggle;
    [SerializeField]
    private Toggle voteToggle;
    [SerializeField]
    private Toggle wyrToggle;
    [SerializeField]
    private Toggle eventToggle;

    [SerializeField]
    private CardType dareType;
    [SerializeField]
    private CardType truthType;
    [SerializeField]
    private CardType voteType;
    [SerializeField]
    private CardType wyrType;
    [SerializeField]
    private CardType eventType;

    private List<Deck> decks = new List<Deck>();
    private GameMode actualCustomizingGameMode;

    public void AddGameMode(GameMode gameMode)
    {
        foreach(Deck deck in gameMode.decks)
        {
            if (!decks.Contains(deck)) decks.Add(deck);
        }
    }

    public void GoToCustomizeDecks(GameMode gameMode)
    {
        actualCustomizingGameMode = gameMode;
        dareToggle.isOn = CheckDeck(dareType);
        truthToggle.isOn = CheckDeck(truthType);
        voteToggle.isOn = CheckDeck(voteType);
        wyrToggle.isOn = CheckDeck(wyrType);
        eventToggle.isOn = CheckDeck(eventType);
    }

    public void OnDeckToggle(Toggle toggle, Deck deck)
    {
        if (toggle.isOn)
        {
            if (!decks.Contains(deck)) decks.Add(deck);
        } else
        {
            decks.Remove(deck);
        }
    }

    private Deck GetDeck(CardType type)
    {
        Deck deck = new Deck();
        deck.cards = new List<Card>();
        foreach (Card card in actualCustomizingGameMode.cards)
        {
            if (card.type == type) deck.cards.Add(card);
        }
        return deck;
    }

    private bool CheckDeck(CardType cardType)
    {
        foreach (Deck deck in decks)
        {
            foreach (Card dCard in deck.cards)
            {
                foreach (Card card in GetDeck(cardType).cards)
                {
                    if (dCard.content == card.content) return true;
                }
            }
        }
        return false;
    }
}
