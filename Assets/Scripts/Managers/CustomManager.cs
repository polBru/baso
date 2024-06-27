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

    private List<CardType> cardTypes = new List<CardType>();

    [HideInInspector] public List<Deck> decks = new List<Deck>();
    private GameMode actualCustomizingGameMode;

    private void Start()
    {
        cardTypes.Add(dareType);
        cardTypes.Add(truthType);
        cardTypes.Add(voteType);
        cardTypes.Add(wyrType);
        cardTypes.Add(eventType);
    }

    public void GoToCustomizeDecks(GameMode gameMode)
    {
        actualCustomizingGameMode = gameMode;
        dareToggle.SetIsOnWithoutNotify(CheckDeck(dareType));
        truthToggle.SetIsOnWithoutNotify(CheckDeck(truthType));
        voteToggle.SetIsOnWithoutNotify(CheckDeck(voteType));
        wyrToggle.SetIsOnWithoutNotify(CheckDeck(wyrType));
        eventToggle.SetIsOnWithoutNotify(CheckDeck(eventType));
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

    public void OnGameModeHold(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void OnGameModeHold(GameMode gameMode)
    {
        actualCustomizingGameMode = gameMode;
        foreach (Deck deck in gameMode.decks)
        {
            foreach (CardType type in cardTypes)
            {
                AddOrRemoveDeckByCardTpe(deck, type);
            }
        }
    }

    private void AddOrRemoveDeckByCardTpe(Deck deck, CardType type)
    {
        foreach (Card card in deck.cards)
        {
            if (card.type == type)
            {
                if (!CheckDeck(type))
                {
                    decks.Add(deck);
                    return;
                }
                else
                {
                    decks.Remove(deck);
                    return;
                }
            }
        }
    }

    public void OnDeckClicked(CardType type)
    {
        Deck deck = GetDeck(type);
        if (!CheckDeck(type)) decks.Add(deck);
        else decks.Remove(deck);
    }
}
