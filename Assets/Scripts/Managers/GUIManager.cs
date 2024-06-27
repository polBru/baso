using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    //Constants
    private const string baso = "BASO";

    private const string lightTextColor = "#FFFFFF";
    private const string darkTextColor = "#323232";

    //Baso Constants
    private const string normalNextText = "Siguiente";
    private const string basoExitText = "Volver al menú";
    private const string basoRestartText = "Volver a jugar";

    private const int skipButtonFontSize = 60;
    private const int basoRestartFontSize = 75;

    [Header("References")]
    [SerializeField] private DebugManager debugManager;
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
    [SerializeField] private GameObject gameChoiceMenu;
    [SerializeField] private GameObject introductionMenu;
    [SerializeField] private GameObject customizeMenu;
    [SerializeField] private GameObject customizeDeckMenu;
    [SerializeField] private GameObject game;

    [Header("Animations")]
    [SerializeField] private float buttonEnableTime = .5f;

    //Private attributes
    private List<string> nameList;

    public void InitializeMainMenu(List<string> nameList)
    {
        this.nameList = nameList;
        mainMenu.SetActive(true);
        gameChoiceMenu.SetActive(false);
        introductionMenu.SetActive(false);
        customizeMenu.SetActive(false);
        customizeDeckMenu.SetActive(false);
        game.SetActive(false);
        if (nameList.Count < 2) playButton.SetActive(false);
        nameListText.text = "Jugadores: ";
    }

    public void InitializeGame()
    {
        if (nameList.Count < 2) return;

        mainMenu.SetActive(false);
        introductionMenu.SetActive(true);
        game.SetActive(false);

        nextText.text = normalNextText;
        skipText.fontSize = skipButtonFontSize;
    }

    public void StartGameUI()
    {
        mainMenu.SetActive(false);
        introductionMenu.SetActive(false);
        game.SetActive(true);
    }

    public void ChangeTextToColor(string hexadeximal)
    {
        Color color;
        ColorUtility.TryParseHtmlString(hexadeximal, out color);

        typeText.color = color;
        contentText.color = color;
        nextText.color = color;
        skipText.color = color;
    }

    public string GetNameInputText()
    {
        return nameInputText.text;
    }

    public void UpdatePlayerList()
    {
        nameListText.text = "Jugadores: ";

        for (int i = 0; i < nameList.Count - 1; i++)
        {
            nameListText.text += nameList[i] + ", ";
        }

        nameListText.text += nameList[nameList.Count - 1] + ".";
        nameInputText.text = "";
        if (nameList.Count >= 2) playButton.SetActive(true);
    }

    public void PrepareCardUI(Card c, string contentText, string skipText)
    {
        ChangeTextToColor(c.type.color == Color.black ? lightTextColor : darkTextColor);
        this.skipText.enabled = c.type.hasPrice;
        background.color = c.type.color;
        typeText.text = c.type.name;
        this.contentText.text = contentText;
        this.skipText.text = skipText;

        if (c.type.name == baso)
        {
            this.nextText.text = basoExitText;
            this.skipText.text = basoRestartText;
            this.skipText.fontSize = basoRestartFontSize;
        }

#if UNITY_EDITOR
        if (debugManager.debugEnabled)
            return;
#endif

        //Spawn animation
        this.nextText.gameObject.SetActive(false);
        this.skipText.gameObject.SetActive(false);
        StartCoroutine(EnableButtonsWithDelay());
    }

    private IEnumerator EnableButtonsWithDelay()
    {
        yield return new WaitForSeconds(buttonEnableTime);
        this.nextText.gameObject.SetActive(true);
        this.skipText.gameObject.SetActive(true);
    }
}
