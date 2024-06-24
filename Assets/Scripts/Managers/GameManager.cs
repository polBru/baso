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

    [Header("Decks")]
    [SerializeField] private GameMode ogGameMode;

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


        // IDEES ----------------------------------------------------------------------------------------------------------------------------------------------------

        //Nou mode = casual
        //OG = hoy se folla
        //Mixte = a lo loco

        
        
        /*q = new Question();
        q.type = "Reto";
        q.content = "Entra a la ducha / piscina con ropa";
        q.price = "(Termina tu vaso)";
        dareList.Add(q);

        q = new Question();
        q.type = "Reto";
        q.content = "Reordena formando un círculo el grupo de más feo a más guapo sin revelar quiénes són los extremos. Todo el mundo a especular!!!";
        q.price = "(Termina tu vaso)";
        dareList.Add(q);

        q = new Question();
        q.type = "Reto";
        q.content = "Cierra los ojos, el resto del grupo te pondra un objeto en tus manos, adivina que es! Si fallas, bebe un trago.";
        q.price = "(Bebe 3 tragos)";
        dareList.Add(q);

        q = new Question();
        q.type = "Reto";
        q.content = "Cierra los ojos, el resto del grupo te dara una bebida, adivina cual es! Si fallas, bebe un trago.";
        q.price = "(Bebe 5 tragos)";
        dareList.Add(q);

        q = new Question();
        q.type = "Verdad";
        q.content = "Crees que eres más guap@ que --Name--?";
        q.price = "(Bebe 5 tragos)";
        truthList.Add(q);

        q = new Question();
        q.type = "Verdad";
        q.content = "Crees que eres más inteligente que --Name--?";
        q.price = "(Bebe 5 tragos)";
        truthList.Add(q);

        q = new Question();
        q.type = "Verdad";
        q.content = "Cuenta un secreto tuyo que no sepa nadie de aquí";
        q.price = "(Bebe 2 tragos)";
        truthList.Add(q);

        q = new Question();
        q.type = "Evento";
        q.content = "--Name--, bebe tantos tragos como jugadores haya en la mesa";
        q.price = "--";
        eventList.Add(q);

        q = new Question();
        q.type = "Evento";
        q.content = "--Name--, reparte tantos tragos como jugadores hay en la mesa";
        q.price = "--";
        eventList.Add(q);

        q = new Question();
        q.type = "Evento";
        q.content = "Todo el mundo excepto --Name-- cierra los ojos. --Name--, hora de hacer alquimia, añade alcohol al vaso de un jugador a tu elección";
        q.price = "--";
        eventList.Add(q);

        q = new Question();
        q.type = "Evento";
        q.content = "--Name--, ahora eres un mago, puedes redirigir un castigo de una persona a otra de tu elección (tu incluido)";
        q.price = "--";
        eventList.Add(q);

        q = new Question();
        q.type = "Evento";
        q.content = "--Name--, la próxima vez que bebas, todos los jugadores beben esa cantidad en tu lugar";
        q.price = "--";
        eventList.Add(q);

        q = new Question();
        q.type = "Evento";
        q.content = "--Name-- y --Name2-- ahora estais enamorados, cuando bebe uno bebe el otro";
        q.price = "--";
        eventList.Add(q);

        q = new Question();
        q.type = "Evento";
        q.content = "Castigo para los cobardes! Todos los que se hayan cagado con alguna verdad o reto esta noche beben";
        q.price = "--";
        eventList.Add(q);

        q = new Question();
        q.type = "Juego";
        q.content = "--Name--, reparte tantos tragos como jugadores hay en la mesa";
        q.price = "--";
        eventList.Add(q);

        // He fet un nou mode de joc que és votación que bàsicament és que qui surt més votat beu, però nse si és molt sema pq pot portar mal rollos
        // Tot i que si treiem les més semes rollo tonto o feo i deixem altres com borracho o introvertido pot estar bé I guess

        q = new Question();
        q.type = "Votación";
        q.content = "El que vaya borracho más bebe";
        q.price = "--";
        eventList.Add(q);

        q = new Question();
        q.type = "Votación";
        q.content = "El más guapo bebe";
        q.price = "--";
        eventList.Add(q);

        q = new Question();
        q.type = "Votación";
        q.content = "El más inteligente bebe";
        q.price = "--";
        eventList.Add(q);

        q = new Question();
        q.type = "Votación";
        q.content = "El peor conductor bebe";
        q.price = "--";
        eventList.Add(q);

        q = new Question();
        q.type = "Votación";
        q.content = "El más introvertido bebe";
        q.price = "--";
        eventList.Add(q);

        q = new Question();
        q.type = "Votación";
        //POL HALP
        q.content = @"El más ""fácil"" a la hora de ligar bebe";
        q.price = "--";
        eventList.Add(q);*/

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
        cards.AddRange(ogGameMode.cards);
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
        Debug.Log("Question List: " + ogGameMode.cards.Count);
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
