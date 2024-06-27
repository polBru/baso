using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Attributes
    //Constants
    private const string baso = "BASO";

    private const string namePlaceholder = "@p";
    private const string name2Placeholder = "@p2";
    private const string pricePlaceholder = "@%";

    private const float minBasoTurnPercentage = .45f; //Percentatge of the minimum turn where the baso card can appear
    private const float basoSoftPittyPercentage = .65f; //Percentatge of turns to calculate baso soft pitty
    private const float basoHardPittyPercentage = .90f; //Percentatge of turns to calculate baso hard pitty

    private const float minBasoChance = 0.02f; //Chance of getting a baso chance before soft pitty
    private const float maxBasoChance = 1f; //Chance of getting a baso chance after hard pitty

    [Header("References")]
    [SerializeField] DebugManager debugManager; 
    [SerializeField] GUIManager guiManager;
    [SerializeField] CustomManager customManager;

    [Header("Decks")]
    [SerializeField] private Deck basoDeck;

    //Private attributes
    private List<string> nameList = new List<string>();
    private List<Card> cards = new List<Card>();

    private int currentName = -1;
    private Card currentCard = null;
    private GameMode currentGameMode = null;
    private int currentTurn = 0;

    private int minBasoTurn;
    private int basoSoftPitty;
    private int basoHardPitty;
    #endregion

    #region Initialization
    private void Start()
    {
        InitializeMenu();
    }

    private void InitializeMenu()
    {
#if UNITY_EDITOR
        if (debugManager.debugEnabled) DebugInitialize();
        guiManager.InitializeMainMenu(nameList);
#else
        guiManager.InitializeMainMenuBuild(nameList, AddPlayer);
#endif
    }

    private void InitializeGame()
    {
        AddCards();
        minBasoTurn = (int)(cards.Count * minBasoTurnPercentage);
        basoSoftPitty = (int)(cards.Count * basoSoftPittyPercentage);
        basoHardPitty = (int)(cards.Count * basoHardPittyPercentage);
        guiManager.InitializeGame();
    }

    private void DebugInitialize()
    {
        foreach (string s in debugManager.debugNames)
        {
            nameList.Add(s);
        }
    }

    private void AddCards()
    {
        cards.AddRange(currentGameMode.cards);
    }
#endregion

#region Game
    private Card GetRandomCard()
    {
        return cards[UnityEngine.Random.Range(0, cards.Count)];
    }

    private Card GetRandomSingleTargetCard()
    {
        Card c = GetRandomCard();
        if (!c.type.singleTarget) c = GetRandomSingleTargetCard();
        return c;
    }

    private Card GetRandomBasoCard()
    {
        return basoDeck.cards[UnityEngine.Random.Range(0, basoDeck.cards.Count)];
    }

    private string GetRandomName(List<string> excludedNames)
    {
        List<string> filteredNameList = new List<string>(nameList);

        foreach (string name in excludedNames)
        {
            if (filteredNameList.Contains(name))
                filteredNameList.Remove(name);
        }

        return filteredNameList[UnityEngine.Random.Range(0, filteredNameList.Count)];
    }

    private bool TryGetBasoCard()
    {
        float currentChance = CalculateBasoChance();

#if UNITY_EDITOR
        if (debugManager.debugLogsEnabled)
        {
            Debug.Log($"Current Turn: {currentTurn} | Current Pitty Chance: {currentChance}");
        }
#endif

        System.Random random = new System.Random();
        return random.NextDouble() < currentChance;
    }

    private float CalculateBasoChance()
    {
        if (currentTurn < minBasoTurn)
        {
            return 0;
        }
        else if (currentTurn < basoSoftPitty)
        {
            return minBasoChance;
        }
        else if (currentTurn >= basoHardPitty)
        {
            return maxBasoChance;
        }
        else
        {
            float slope = (maxBasoChance - minBasoChance) / (basoHardPitty - basoSoftPitty);
            return minBasoChance + slope * (currentTurn - basoSoftPitty);
        }
    }

    private void NextPlayer()
    {
        currentName = (currentName == nameList.Count - 1) ? 0 : currentName + 1;
    }

    private void EndGame()
    {
        cards.Clear();
        currentName = -1;
        currentCard = null;
        currentTurn = 0;

        nameList.Clear();
        InitializeMenu();
    }

    private void RestartGame()
    {
        cards.Clear();
        currentName = -1;
        currentCard = null;
        currentTurn = 0;

        InitializeGame();
        currentName = 0;
        PrepareCard(GetRandomCard());
    }
#endregion

#region StringManagement
    private string GetContentText(Card c)
    {
        string content = "";
        if (c.type.singleTarget) content = $"{nameList[currentName]}: ";
        return $"{content}{ReplaceContent(c.content, c.type.singleTarget)}";
    }

    private string ReplaceContent(string content, bool excludeCurrentPlayer)
    {
        List<string> excludedNames = new List<string>();
        string name;

        if (excludeCurrentPlayer)
            excludedNames.Add(nameList[currentName]);

        if (content.Contains(name2Placeholder))
        {
            //Replace @p2
            name = GetRandomName(excludedNames);
            excludedNames.Add(name);
            content = content.Replace(name2Placeholder, name);
        }

        if (content.Contains(namePlaceholder))
        {
            //Replace @p
            name = GetRandomName(excludedNames);
            content = content.Replace(namePlaceholder, name);
        }

        return content;
    }

    private string ReplacePriceText(string content, int number)
    {
        string numString = number == 0 ? "?" : number.ToString();
        string newText = content.Replace(pricePlaceholder, numString);
        if (number == 1) newText = newText.Remove(newText.Length-1);
        return newText;
    }

    private string GetEnumDescription(Enum value)
    {
        FieldInfo fi = value.GetType().GetField(value.ToString());

        DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

        if (attributes != null && attributes.Any())
        {
            return attributes.First().Description;
        }

        return value.ToString();
    }
#endregion

#region Buttons
    public void StartGame()
    {
        guiManager.StartGameUI();
        Next();

#if UNITY_EDITOR
        if (debugManager.debugLogsEnabled) DebugLogs();
#endif

    }

#if UNITY_EDITOR
    private void DebugLogs()
    {
        Debug.Log("Name List: " + nameList.Count);
        Debug.Log("Question List: " + cards.Count);
        Debug.Log("Min Baso Turn: " + minBasoTurn);
        Debug.Log("Baso Soft Pitty: " + basoSoftPitty);
        Debug.Log("Baso Hard Pitty: " + basoHardPitty);
    }
#endif

    public void PlayWithGameMode(GameMode gameMode)
    {
        currentGameMode = gameMode;

#if UNITY_EDITOR
        if (debugManager.debugGameModeEnabled)
            currentGameMode = debugManager.debugGameMode;
#endif

        InitializeGame();
    }

    public void PlayWithCustomDecks()
    {
        GameMode gameMode = new GameMode();
        gameMode.decks = new List<Deck>();
        gameMode.decks.AddRange(customManager.decks);
        currentGameMode = gameMode;
        InitializeGame();
    }

    public void AddPlayer()
    {
        string name = guiManager.GetNameInputText();
        if (name == "") return;
        nameList.Add(name);
        guiManager.UpdatePlayerList();
    }

    private void PrepareCard(Card c)
    {
        currentCard = c;
        string content = GetContentText(c);
        string skip = "No tengo huevos\n" + ReplacePriceText(GetEnumDescription(c.price.type), c.price.number);
        guiManager.PrepareCardUI(c, content, skip);

        if (!c.type.singleTarget)
            cards.Remove(c);
    }

    public void Next() 
    {
        currentTurn++;

        if (currentCard?.type?.name == baso)
        {
            EndGame();
            return;
        }

        NextPlayer();
        PrepareCard(TryGetBasoCard() ? GetRandomBasoCard() : GetRandomCard());
    }

    public void Repeat()
    {
        if (currentCard?.type?.name == baso)
        {
            RestartGame();
            return;
        }

        PrepareCard(TryGetBasoCard() ? GetRandomBasoCard() : GetRandomSingleTargetCard());
    }
#endregion
}
