using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    const string namePlaceholder = "@p";
    const string name2Placeholder = "@p2";
    const string pricePlaceholder = "@%";

    [Header("Debug")]
    [SerializeField] private bool debugEnabled = true;
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

    private int currentName = -1;

    [Header("Menus")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject introductionMenu;
    [SerializeField] private GameObject game;

    [Header("Game Modes")]
    [SerializeField] private GameMode testingGameMode; //For debugging
    [SerializeField] private GameMode casualGameMode;
    [SerializeField] private GameMode spicyGameMode;

    private List<Card> cards = new List<Card>();

    [Header("Lists")]
    private List<string> nameList = new List<string>();

    private void DebugInitialize()
    {
        foreach (string s in debugNames)
        {
            nameList.Add(s);
        }
    }

    void Awake()
    {

#if UNITY_EDITOR
        if (debugEnabled) DebugInitialize();
#endif

        AddCards();
    }

    void Start()
    {
        mainMenu.SetActive(true);
        introductionMenu.SetActive(false);
        game.SetActive(false);
        if (nameList.Count < 2) playButton.SetActive(false);
        nameListText.text = "Jugadores";
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

    void AddCards()
    {
        //Check game mode
        cards.AddRange(testingGameMode.cards);
        //cards.AddRange(casualGameMode.cards);
        //cards.AddRange(spicyGameMode.cards);
    }

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
        if (debugEnabled) DebugLists();

    }

    private void DebugLists()
    {
        Debug.Log("Name List: " + nameList.Count);
        Debug.Log("Question List: " + spicyGameMode.cards.Count);
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

    private string GetRandomName()
    {
        string name = nameList[UnityEngine.Random.Range(0, nameList.Count)];
        if (name == nameList[currentName]) return GetRandomName();

        return name;
    }

    private void NextPlayer()
    {
        currentName = (currentName == nameList.Count - 1) ? 0 : currentName + 1;
    }

    private string ReplaceContent(string content)
    {
        string newContent = content;
        if (content.Contains(namePlaceholder)) newContent = content.Replace(namePlaceholder, GetRandomName());

        return newContent;
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
        return $"{content}{ReplaceContent(c.content)}";
    }

    //Buttons
    private void PrepareCard(Card c)
    {
        ChangeTextToColor(c.type.color == Color.black ? "#FFFFFF" : "#323232");
        skipText.enabled = c.type.hasPrice;
        background.color = c.type.color;
        typeText.text = c.type.name;
        contentText.text = GetContentText(c);
        skipText.text = "No tengo huevos\n" + ReplacePriceText(GetEnumDescription(c.price.type), c.price.number);
    }

    public void Next() 
    {
        NextPlayer();
        PrepareCard(GetRandomCard());
    }

    public void Repeat()
    {
        PrepareCard(GetRandomSingleTargetCard());
    }
    #endregion

}
