using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
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


    [Header("Lists")]
    private List<string> nameList = new List<string>();
    
    private List<Question> questionList = new List<Question>();
    private List<Question> eventList = new List<Question>();
    private List<Question> wyrList = new List<Question>();
    private List<Question> truthList = new List<Question>();
    private List<Question> dareList = new List<Question>();

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
        //Global events
        Question q = new Question();
        q.type = "Evento";
        q.content = "Todo el mundo bebe y se quita una prenda";
        q.price = "--";
        eventList.Add(q);

        q = new Question();
        q.type = "Evento";
        q.content = "Quien no haya besado a nadie esta noche, bebe";
        q.price = "--";
        eventList.Add(q);

        q = new Question();
        q.type = "Evento";
        q.content = "Quien se haya besado con alguien esta noche, bebe del vaso de la otra persona";
        q.price = "--";
        eventList.Add(q);

        q = new Question();
        q.type = "Evento";
        q.content = "Los chicos beben tantos tragos como chicas haya";
        q.price = "--";
        eventList.Add(q);

        q = new Question();
        q.type = "Evento";
        q.content = "Las chicas beben tantos tragos como chicos haya";
        q.price = "--";
        eventList.Add(q);

        q = new Question();
        q.type = "Evento";
        q.content = "Los que quieran follar esta noche beben";
        q.price = "--";
        eventList.Add(q);

        q = new Question();
        q.type = "Evento";
        q.content = "Los fumadores beben";
        q.price = "--";
        eventList.Add(q);

        q = new Question();
        q.type = "Evento";
        q.content = "Quien haya bebido más esta noche termina su vaso";
        q.price = "--";
        eventList.Add(q);

        q = new Question();
        q.type = "Evento";
        q.content = "El más mayor reparte su edad en tragos";
        q.price = "--";
        eventList.Add(q);

        q = new Question();
        q.type = "Evento";
        q.content = "El más menor escoje a 3 personas para que se acaben su vaso";
        q.price = "--";
        eventList.Add(q);

        q = new Question();
        q.type = "Evento";
        q.content = "Todo el mundo bebe del vaso de su derecha";
        q.price = "--";
        eventList.Add(q);

        q = new Question();
        q.type = "Evento";
        q.content = "Todos los que se hayan acostado con alguien que haya conocido en una web de contactos beben";
        q.price = "--";
        eventList.Add(q);

        q = new Question();
        q.type = "Evento";
        q.content = "Los que hayan follado durante la regla beben";
        q.price = "--";
        eventList.Add(q);

        q = new Question();
        q.type = "Evento";
        q.content = "Los que hayan tenido una experiencia sexual con ambos géneros beben";
        q.price = "--";
        eventList.Add(q);

        q = new Question();
        q.type = "Evento";
        q.content = "La persona que se haya acostado con más gente reparte tantos tragos como gente se haya acostado";
        q.price = "--";
        eventList.Add(q);

        q = new Question();
        q.type = "Evento";
        q.content = "Bebe si tu amor platónico está hoy aquí";
        q.price = "--";
        eventList.Add(q);

        q = new Question();
        q.type = "Evento";
        q.content = "Todas la chicas se quitan una prenda o le quitan una prenda a un chico";
        q.price = "--";
        eventList.Add(q);

        q = new Question();
        q.type = "Evento";
        q.content = "La gente que le guste que le azoten durante el sexo bebe";
        q.price = "--";
        eventList.Add(q);

        q = new Question();
        q.type = "Evento";
        q.content = "El grupo elige a una persona para que se termine su vaso";
        q.price = "--";
        eventList.Add(q);

        q = new Question();
        q.type = "Evento";
        q.content = "Atención nueva formación: Chico-chica-chico-chica... A intercambiar sitios!";
        q.price = "--";
        eventList.Add(q);

        //Dare
        q = new Question();
        q.type = "Reto";
        q.content = "Escoge a una persona para estar encerrado en un armario o sitio cerrado durante 5 min";
        q.price = "(Bebe 2 chupitos)";
        dareList.Add(q);

        q = new Question();
        q.type = "Reto";
        q.content = "Intercanvia el vaso con --Name--";
        q.price = "(Bebe 3 tragos)";
        dareList.Add(q);

        q = new Question();
        q.type = "Reto";
        q.content = "Sacate una prenda de ropa";
        q.price = "(Bebe 5 tragos)";
        dareList.Add(q);

        q = new Question();
        q.type = "Reto";
        q.content = "Dale un beso a una persona donde ella quiera";
        q.price = "(Bebe 2 trago)";
        dareList.Add(q);

        q = new Question();
        q.type = "Reto";
        q.content = "Chupa los dedos de --Name-- por 30 secs";
        q.price = "(Bebe 2 tragos)";
        dareList.Add(q);

        q = new Question();
        q.type = "Reto";
        q.content = "Tienes que estar sin camiseta o sin pantalones por el resto del juego";
        q.price = "(Termina tu vaso)";
        dareList.Add(q);

        q = new Question();
        q.type = "Reto";
        q.content = "Liate con la persona con la letra primera del alfabeto";
        q.price = "(Bebe 1 trago)";
        dareList.Add(q);

        q = new Question();
        q.type = "Reto";
        q.content = "Chupa el dedo del pie de --Name--";
        q.price = "(Bebe 3 tragos)";
        dareList.Add(q);

        q = new Question();
        q.type = "Reto";
        q.content = "Liate con --Name--";
        q.price = "(Bebe 2 tragos)";
        dareList.Add(q);

        q = new Question();
        q.type = "Reto";
        q.content = "Elije a 2 personas para que se lien";
        q.price = "(Bebe 1 trago)";
        dareList.Add(q);

        q = new Question();
        q.type = "Reto";
        q.content = "Perrea con alguien o escoge a alguien que te perree";
        q.price = "(Bebe 2 tragos)";
        dareList.Add(q);

        q = new Question();
        q.type = "Reto";
        q.content = "Escoge con quien harias un trio de los presentes";
        q.price = "(Termina tu vaso)";
        dareList.Add(q);

        q = new Question();
        q.type = "Reto";
        q.content = "Grita algo sucio";
        q.price = "(Bebe 2 tragos)";
        dareList.Add(q);

        q = new Question();
        q.type = "Reto";
        q.content = "Escoge a una persona a ciegas y besate con ella";
        q.price = "(Bebe 5 tragos)";
        dareList.Add(q);

        q = new Question();
        q.type = "Reto";
        q.content = "Follar, casar y matar";
        q.price = "(Bebe 2 tragos)";
        dareList.Add(q);

        q = new Question();
        q.type = "Reto";
        q.content = "Besa a alguien sensualmente en el cuello";
        q.price = "(Bebe 3 tragos)";
        dareList.Add(q);

        q = new Question();
        q.type = "Reto";
        q.content = "Besa a --Name-- sensualmente en el cuello";
        q.price = "(Bebe 4 tragos)";
        dareList.Add(q);

        q = new Question();
        q.type = "Reto";
        q.content = "Di tu talla de miembro o de brasier";
        q.price = "(Bebe 1 trago)";
        dareList.Add(q);

        q = new Question();
        q.type = "Reto";
        q.content = "Finge un orgasmo";
        q.price = "(Bebe 2 tragos)";
        dareList.Add(q);

        q = new Question();
        q.type = "Reto";
        q.content = "Di una broma sucia";
        q.price = "(Bebe 1 trago)";
        dareList.Add(q);

        q = new Question();
        q.type = "Reto";
        q.content = "Pasale una foto tuya a quien quien te interese más del grupo";
        q.price = "(Termina tu vaso)";
        dareList.Add(q);

        q = new Question();
        q.type = "Reto";
        q.content = "Todos (menos tu) a ciegas! Escoje a alguien y dale un pico";
        q.price = "(Bebe 2 tragos)";
        dareList.Add(q);

        q = new Question();
        q.type = "Reto";
        q.content = "Frota tu cara con las partes íntimas de --Name--";
        q.price = "(Termina tu vaso)";
        dareList.Add(q);

        q = new Question();
        q.type = "Reto";
        q.content = "Muestra una parte íntima de tu cuerpo";
        q.price = "(Bebe 3 tragos)";
        dareList.Add(q);

        q = new Question();
        q.type = "Reto";
        q.content = "Muestra las últimas 5 fotos de tu móvil";
        q.price = "(Bebe 5 tragos)";
        dareList.Add(q);

        q = new Question();
        q.type = "Reto";
        q.content = "Quítale una prenda de ropa a --Name--";
        q.price = "(Bebe 3 tragos)";
        dareList.Add(q);

        q = new Question();
        q.type = "Reto";
        q.content = "Bebe 2 tragos y reparte 4";
        q.price = "(Bebe 1 trago)";
        dareList.Add(q);

        q = new Question();
        q.type = "Reto";
        q.content = "Elige una persona para que te acaricie el culo durante 5 segundos";
        q.price = "(Bebe 2 tragos)";
        dareList.Add(q);

        q = new Question();
        q.type = "Reto";
        q.content = "Haz un reto elegido por el grupo.";
        q.price = "(Bebe 3 tragos)";
        dareList.Add(q);

        q = new Question();
        q.type = "Reto";
        q.content = "Canviate de sitio con --Name--";
        q.price = "(Bebe 1 tragos)";
        dareList.Add(q);

        //Truth
        q = new Question();
        q.type = "Verdad";
        q.content = "Nombra tu fetiche sexual más oscuro";
        q.price = "(Termina tu vaso)";
        truthList.Add(q);

        q = new Question();
        q.type = "Verdad";
        q.content = "Alguna vez has probado semen?";
        q.price = "(Bebe 2 tragos)";
        truthList.Add(q);

        q = new Question();
        q.type = "Verdad";
        q.content = "Has perdido la virginidad?";
        q.price = "(Bebe 1 trago)";
        truthList.Add(q);

        q = new Question();
        q.type = "Verdad";
        q.content = "Cuál es la cosa más loca que has hecho borracho?";
        q.price = "(Bebe 4 tragos)";
        truthList.Add(q);

        q = new Question();
        q.type = "Verdad";
        q.content = "Alguna vez has tenido cibersexo?";
        q.price = "(Bebe 5 tragos)";
        truthList.Add(q);

        q = new Question();
        q.type = "Verdad";
        q.content = "Alguna vez has usado comida para masturbarte? Describe tu experiencia.";
        q.price = "(Bebe 2 tragos)";
        truthList.Add(q);

        q = new Question();
        q.type = "Verdad";
        q.content = "Quien seria el mejor novio/a de aqui?";
        q.price = "(Termina tu vaso)";
        truthList.Add(q);

        q = new Question();
        q.type = "Verdad";
        q.content = "Que es lo que más te pone de --Name--.";
        q.price = "(Bebe 5 tragos)";
        truthList.Add(q);

        q = new Question();
        q.type = "Verdad";
        q.content = "Tienes algun juguete sexual? Nombra uno";
        q.price = "(Bebe 1 trago)";
        truthList.Add(q);

        q = new Question();
        q.type = "Verdad";
        q.content = "Trio con un amigo y la pareja, aceptas?";
        q.price = "(Bebe 1 trago)";
        truthList.Add(q);

        q = new Question();
        q.type = "Verdad";
        q.content = "Alguna vez has fingido un orgasmo?";
        q.price = "(Bebe 2 tragos)";
        truthList.Add(q);

        q = new Question();
        q.type = "Verdad";
        q.content = "Quien es el más sexy del grupo?";
        q.price = "(Bebe 4 tragos)";
        truthList.Add(q);

        q = new Question();
        q.type = "Verdad";
        q.content = "Con quien tienes más ganas de follar de este grupo?";
        q.price = "(Termina tu vaso)";
        truthList.Add(q);

        q = new Question();
        q.type = "Verdad";
        q.content = "Cuántos vasos tienes que beber para tener sexo con --Name--?";
        q.price = "(Bebe 4 tragos)";
        truthList.Add(q);

        q = new Question();
        q.type = "Verdad";
        q.content = "Alguna vez te has/han metido algo por el culo?";
        q.price = "(Termina tu vaso)";
        truthList.Add(q);

        q = new Question();
        q.type = "Verdad";
        q.content = "De todos los jugadores, quién crees que la chupa mejor?";
        q.price = "(Bebe 2 tragos)";
        truthList.Add(q);

        q = new Question();
        q.type = "Verdad";
        q.content = "Has venido con intención de follar esta noche?";
        q.price = "(Bebe 4 tragos)";
        truthList.Add(q);

        q = new Question();
        q.type = "Verdad";
        q.content = "Cuál es el sitio más raro en el que has metido el pene o el objeto más raro que te has metido por la vagina?";
        q.price = "(Bebe 3 tragos)";
        truthList.Add(q);

        q = new Question();
        q.type = "Verdad";
        q.content = "Te gustaria tener una experiencia sexual con alguien de tu mismo sexo?";
        q.price = "(Bebe 2 tragos)";
        truthList.Add(q);

        q = new Question();
        q.type = "Verdad";
        q.content = "Responde a una verdad elegida por el grupo";
        q.price = "(Bebe 3 tragos)";
        truthList.Add(q);

        q = new Question();
        q.type = "Verdad";
        q.content = "Te has masturbado alguna vez con hentai o dibujos animados? Describe uno.";
        q.price = "(Bebe 4 tragos)";
        truthList.Add(q);

        q = new Question();
        q.type = "Verdad";
        q.content = "Cuál es el video más oscuro con el que te has masturbado?";
        q.price = "(Bebe 4 tragos)";
        truthList.Add(q);

        q = new Question();
        q.type = "Verdad";
        q.content = "Crees que follaras esta noche?";
        q.price = "(Bebe 3 trago)";
        truthList.Add(q);

        //Would you rather
        q = new Question();
        q.type = "Qué prefieres?";
        q.content = "Liarse o follar";
        q.price = "--";
        wyrList.Add(q);

        q = new Question();
        q.type = "Qué prefieres?";
        q.content = "Ser bueno besando o bueno en la cama";
        q.price = "--";
        wyrList.Add(q);

        q = new Question();
        q.type = "Qué prefieres?";
        q.content = "Tener solo sexo en la ducha o solo sexo en el suelo";
        q.price = "--";
        wyrList.Add(q);

        q = new Question();
        q.type = "Qué prefieres?";
        q.content = "Tener sexo durante la primera cita o a los 6 meses";
        q.price = "--";
        wyrList.Add(q);

        q = new Question();
        q.type = "Qué prefieres?";
        q.content = "Tener sexo con una persona más joven o más vieja";
        q.price = "--";
        wyrList.Add(q);

        q = new Question();
        q.type = "Qué prefieres?";
        q.content = "Tener sexo con una persona que grita o que esta callada en la cama";
        q.price = "--";
        wyrList.Add(q);

        q = new Question();
        q.type = "Qué prefieres?";
        q.content = "1 minuto de sexo increible o 10 minutos de sexo normal";
        q.price = "--";
        wyrList.Add(q);

        q = new Question();
        q.type = "Qué prefieres?";
        q.content = "Tener sexo borracho o en resaca";
        q.price = "--";
        wyrList.Add(q);

        q = new Question();
        q.type = "Qué prefieres?";
        q.content = "Recibir o dar sexo oral";
        q.price = "--";
        wyrList.Add(q);

        q = new Question();
        q.type = "Qué prefieres?";
        q.content = "Que te pillen tus padres follando o pillar a tus padres follando";
        q.price = "--";
        wyrList.Add(q);

        q = new Question();
        q.type = "Qué prefieres?";
        q.content = "Enviar nudes a tu jefe/professor o enviar nudes a tu madre";
        q.price = "--";
        wyrList.Add(q);

        q = new Question();
        q.type = "Qué prefieres?";
        q.content = "No ver o no escuchar durante el sexo";
        q.price = "--";
        wyrList.Add(q);

        q = new Question();
        q.type = "Qué prefieres?";
        q.content = "Partes con pelo o partes afeitadas";
        q.price = "--";
        wyrList.Add(q);

        q = new Question();
        q.type = "Qué prefieres?";
        q.content = "Tener sexo con tu primo o con la madre/padre de un amigo";
        q.price = "--";
        wyrList.Add(q);

        q = new Question();
        q.type = "Qué prefieres?";
        q.content = "Acostarte con el compañero de tu derecha o de tu izquierda";
        q.price = "--";
        wyrList.Add(q);

        q = new Question();
        q.type = "Qué prefieres?";
        q.content = "Dominar o ser dominado";
        q.price = "--";
        wyrList.Add(q);

        //Black card
        q = new Question();
        q.type = "BASO";
        q.content = "Haz la mezcla definitiva! Vierte en el vaso del centro un poco de la bebida de todos los jugadores hasta llenarlo. Qué aproveche!";
        q.price = "--";
        questionList.Add(q);

        //Question list
        foreach (Question question in eventList)
        {
            questionList.Add(question);
        }

        foreach (Question question in dareList)
        {
            questionList.Add(question);
        }

        foreach (Question question in truthList)
        {
            questionList.Add(question);
        }

        foreach (Question question in wyrList)
        {
            questionList.Add(question);
        }

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
                addPlayer();
            }
        });
    }
#endif

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

        Next(true);
#if UNITY_EDITOR
        if (debugEnabled) DebugLists();

    }

    private void DebugLists()
    {
        Debug.Log("Name List: " + nameList.Count);
        Debug.Log("Question List: " + questionList.Count);
        Debug.Log("Event List: " + eventList.Count);
        Debug.Log("Dare List: " + dareList.Count);
        Debug.Log("Truth List: " + truthList.Count);
        Debug.Log("WyR List: " + wyrList.Count);
    }
#endif

    #endregion

    #region Game
    private Question GetRandomQuestion()
    {
        return questionList[Random.Range(0, questionList.Count)];
    }

    private string GetRandomName()
    {
        string name = nameList[Random.Range(0, nameList.Count)];
        if (name == nameList[currentName]) return GetRandomName();

        return name;
    }

    private void NextPlayer()
    {
        currentName = (currentName == nameList.Count - 1) ? 0 : currentName + 1;
    }

    private string ReplaceContent(Question q)
    {
        string content;
        if (q.content.Contains("--Name--")) content = q.content.Replace("--Name--", GetRandomName());
        else content = q.content;

        return content;
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

    //Buttons
    public void Next(bool skipPlayer)
    {
        Question q = GetRandomQuestion();
        ChangeTextToColor("#323232"); //black
        skipText.enabled = true;

        //Skip event and wyd in case they don't have balls
        if (!skipPlayer)
        {
            if (q.type == "Evento" || q.type == "Qué prefieres?")
            {
                Next(false);
                return;
            }
        }

        //Show card
        if (q.type == "Evento")
        {
            background.color = Color.cyan;
            typeText.text = q.type;
            ReplaceContent(q);
            contentText.text = ReplaceContent(q);
            skipText.enabled = false;
        }
        else if (q.type == "Reto")
        {
            if (skipPlayer) NextPlayer();
            Color red;
            ColorUtility.TryParseHtmlString("#FF2828", out red);
            background.color = red;
            typeText.text = q.type;
            contentText.text = nameList[currentName] + ": " + ReplaceContent(q);
            skipText.text = "No tengo huevos\n" + q.price;
        }
        else if (q.type == "Verdad")
        {
            if (skipPlayer) NextPlayer();
            background.color = Color.green;
            typeText.text = q.type;
            contentText.text = nameList[currentName] + ": " + ReplaceContent(q);
            skipText.text = "No tengo huevos\n" + q.price;
        }
        else if (q.type == "Qué prefieres?")
        {
            background.color = Color.yellow;
            typeText.text = q.type;
            contentText.text = ReplaceContent(q);
            skipText.enabled = false;
        }
        else if (q.type == "BASO")
        {
            if (skipPlayer) NextPlayer();
            background.color = Color.black;
            ChangeTextToColor("#FFFFFF"); //whtie
            typeText.text = q.type;
            contentText.text = nameList[currentName] + ": " + ReplaceContent(q);
            skipText.enabled = false;
            questionList.Remove(q);
        }
        else
        {
            Debug.LogError("Something went wrong, non existant question type");
        }

    }
    #endregion

}
