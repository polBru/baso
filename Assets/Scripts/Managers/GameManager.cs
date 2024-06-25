using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Attributes
    //Constants
    const string baso = "BASO";

    const string namePlaceholder = "@p";
    const string name2Placeholder = "@p2";
    const string pricePlaceholder = "@%";

    const float minBasoTurnPercentage = .35f; //Percentatge of the minimum turn where the baso card can appear
    const float basoSoftPittyPercentage = .75f; //Percentatge of turns to calculate baso soft pitty
    const float basoHardPittyPercentage = .90f; //Percentatge of turns to calculate baso hard pitty

    private const float minBasoChance = 0.02f; //Chance of getting a baso chance before soft pitty
    private const float maxBasoChance = 1f; //Chance of getting a baso chance after hard pitty

    [Header("Debug")]
    [SerializeField] private bool debugEnabled = true;
    [SerializeField] private bool debugLogsEnabled = true;
    [SerializeField] private List<string> debugNames;

    [Header("References")]
    [SerializeField] private Image background;
    [SerializeField] private Text typeText;
    [SerializeField] private Text contentText;
    [SerializeField] private Text skipText;
    [SerializeField] private Text nextText;
    [SerializeField] private InputField nameInputText;
    [SerializeField] private Text nameListText;
    [SerializeField] private GameObject playButton;

    [Header("Menus")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject introductionMenu;
    [SerializeField] private GameObject game;

    [Header("Game Modes")]
    [SerializeField] private GameMode testingGameMode; //For debugging
    [SerializeField] private GameMode casualGameMode;
    [SerializeField] private GameMode spicyGameMode;

    [Header("Decks")]
    [SerializeField] private Deck basoDeck;

    //Private attributes
    private List<string> nameList = new List<string>();
    private List<Card> cards = new List<Card>();

    private int currentName = -1;
    private Card currentCard = null;
    private int currentTurn = 0;

    private int minBasoTurn;
    private int basoSoftPitty;
    private int basoHardPitty;
    #endregion

    #region Initialization
    private void Awake()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {

#if UNITY_EDITOR
        if (debugLogsEnabled) DebugInitialize();
#endif

        AddCards();
        mainMenu.SetActive(true);
        introductionMenu.SetActive(false);
        game.SetActive(false);
        if (nameList.Count < 2) playButton.SetActive(false);
        nameListText.text = "Jugadores";
        InitializeBasoCard();
    }

    private void InitializeBasoCard()
    {
        minBasoTurn = (int)(cards.Count * minBasoTurnPercentage);
        basoSoftPitty = (int)(cards.Count * basoSoftPittyPercentage);
        basoHardPitty = (int)(cards.Count * basoHardPittyPercentage);
    }

    private void DebugInitialize()
    {
        foreach (string s in debugNames)
        {
            nameList.Add(s);
        }
    }

    void AddCards()
    {
        //Check game mode
        //cards.AddRange(testingGameMode.cards);
        //cards.AddRange(casualGameMode.cards);
        cards.AddRange(spicyGameMode.cards);
    }

#if !UNITY_EDITOR
    void Update()
    {
        nameInputText.onEndEdit.AddListener(val =>
        {
            // TouchScreenKeyboard.Status.Done: Keyboard disappeared when something like "Done" button in mobilekeyboard
            // TouchScreenKeyboard.Status.Canceled: Keyboard disappeared when "Back" Hardware Button Pressed in Android
            // TouchScreenKeyboard.Status.LostFocus: Keyboard disappeared when some area except keyboard and input area
            if (nameInputText.touchScreenKeyboard.status == TouchScreenKeyboard.Status.Done)
            {
                AddPlayer();
            }
        });
    }
#endif
    #endregion

    #region MainMenu
    public void AddPlayer()
    {
        if (nameInputText.text == "") return;
        nameListText.text = "Jugadores: ";
        nameList.Add(nameInputText.text);

        for (int i = 0; i < nameList.Count - 1; i++)
        {
            nameListText.text += nameList[i] + ", ";
        }
        nameListText.text += nameList[nameList.Count - 1] + ".";

        nameInputText.text = "";
        if (nameList.Count >= 2) playButton.SetActive(true);
    }

    public void Play()
    {
        if (nameList.Count < 2) return;

        mainMenu.SetActive(false);
        introductionMenu.SetActive(true);
        game.SetActive(false);
    }
    #endregion

    #region IntroductionMenu
    public void StartGame()
    {
        mainMenu.SetActive(false);
        introductionMenu.SetActive(false);
        game.SetActive(true);

        Next();
#if UNITY_EDITOR
        if (debugEnabled) DebugLogs();

    }

    private void DebugLogs()
    {
        Debug.Log("Name List: " + nameList.Count);
        Debug.Log("Question List: " + cards.Count);
        Debug.Log("Min Baso Turn: " + minBasoTurn);
        Debug.Log("Baso Soft Pitty: " + basoSoftPitty);
        Debug.Log("Baso Hard Pitty: " + basoHardPitty);
    }
#endif
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

    public bool TryGetBasoCard()
    {
        float currentChance = CalculateBasoChance();

#if UNITY_EDITOR
        if (debugLogsEnabled)
        {
            Debug.Log($"Current Turn: {currentTurn} | Current Pitty Chance: {currentChance}");
        }
#endif

        System.Random random = new System.Random();
        bool isSuccess = random.NextDouble() < currentChance;

        return isSuccess;
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
        return content.Replace(pricePlaceholder, number.ToString());
    }

    private void ChangeTextToColor(string hexadeximal)
    {
        Color color;
        ColorUtility.TryParseHtmlString(hexadeximal, out color);

        typeText.color = color;
        contentText.color = color;
        nextText.color = color;
        skipText.color = color;
    }

    public string GetEnumDescription(Enum value)
    {
        FieldInfo fi = value.GetType().GetField(value.ToString());

        DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

        if (attributes != null && attributes.Any())
        {
            return attributes.First().Description;
        }

        return value.ToString();
    }

    private string GetContentText(Card c)
    {
        string content = "";
        if (c.type.singleTarget) content = $"{nameList[currentName]}: ";
        return $"{content}{ReplaceContent(c.content, c.type.singleTarget)}";
    }

    private void EndGame()
    {
        nameList.Clear();
        cards.Clear();
        currentName = -1;
        currentCard = null;
        currentTurn = 0;

        InitializeGame();
    }
    #endregion

    #region Buttons
    private void PrepareCard(Card c)
    {
        currentCard = c;
        ChangeTextToColor(c.type.color == Color.black ? "#FFFFFF" : "#323232");
        skipText.enabled = c.type.hasPrice;
        background.color = c.type.color;
        typeText.text = c.type.name;
        contentText.text = GetContentText(c);
        skipText.text = "No tengo huevos\n" + ReplacePriceText(GetEnumDescription(c.price.type), c.price.number);

        if (!c.type.singleTarget)
            cards.Remove(c);
    }

    public void Next() 
    {
        currentTurn++;

        if (currentCard?.type?.name == baso)
        {
            EndGame(); //TODO: Implement end game menu (Return to main menu / play again)
            return;
        }

        NextPlayer();
        PrepareCard(TryGetBasoCard() ? GetRandomBasoCard() : GetRandomCard());
    }

    public void Repeat()
    {
        PrepareCard(TryGetBasoCard() ? GetRandomBasoCard() : GetRandomSingleTargetCard());
    }
    #endregion
}
