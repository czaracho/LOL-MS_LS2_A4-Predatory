using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoLSDK;
using SimpleJSON;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Supercyan.AnimalPeopleSample;
using UnityEngine.SceneManagement;

public class TextManager : MonoBehaviour
{
    private Queue<dialog> sentences;

    public TextTrigger objectivesDialogue;
    public TextTrigger objectivesNonCompletedDialogue;

    public Image catLanaPortrait;
    public Image catPebblesPortrait;
    public GameObject dialogBox;
    private TextMeshProUGUI currentTMPElement;
    public Button nextButton;
    public Color hightlightedColor;
    public Color darkerColor;
    private bool canGoNextSentence;

    private dialog.NameOfCat nameOfCat;
    private string currentTalkingCatText = "";
    private string currentObjectivesText = "";
    public SimpleSampleCharacterControl tpsController;

    JSONNode _lang;

    public GameManagerScript gameManagerScript;

    private void Start()
    {
        sentences = new Queue<dialog>();
        _lang = SharedState.LanguageDefs;
        nextButton.onClick.AddListener(DisplayNextSentence);
        tpsController = FindObjectOfType<SimpleSampleCharacterControl>();
    }

    public void StartDialogue(Dialogue dialogue, GameManagerScript.GameAction gameAction, TextMeshProUGUI currentTextElement) {

        currentTMPElement = currentTextElement;
        tpsController.playerCanMoveTps = false;
        EventManager.instance.OnShowIngameUI(false);

        if (sentences == null)
        {
            Debug.Log("Cargamos el queue");
            sentences = new Queue<dialog>();
        }

        canGoNextSentence = true;
        gameManagerScript.gameAction = gameAction;
        catLanaPortrait.gameObject.SetActive(false);
        catPebblesPortrait.gameObject.SetActive(false);
        dialogBox.SetActive(true);

        sentences.Clear();

        foreach (dialog sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void GetTextForTitlesAndObjectives(Dialogue dialogue, TextTrigger.TypeOfText typeOfText, TextMeshProUGUI currentTextElement) {

        currentTMPElement = currentTextElement;

        if (sentences == null)
        {
            sentences = new Queue<dialog>();
        }

        sentences.Clear();

        foreach (dialog sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentenceSimple(typeOfText);
    }

    public void DisplayNextSentence() {

        //Debug.Log("sentences left: " + sentences.Count);

        if (!canGoNextSentence)
            return;

        if (sentences.Count == 0) {
            //Debug.Log("papu los sentences son 0");
            EndDialogue(true);
            return;
        }


        dialog dialog = sentences.Dequeue();
        //Debug.Log("Hacemos dequeue a esto");

        if (_lang == null)
        {
            _lang = SharedState.LanguageDefs;
        }

        string dialogeLine = _lang[dialog.sentenceId];
        nameOfCat = dialog.nameOfCat;

        string nextLine;

        if (nameOfCat == dialog.NameOfCat.Pebbles)
        {
            catLanaPortrait.gameObject.SetActive(true);
            catPebblesPortrait.gameObject.SetActive(true);
            currentTalkingCatText = nameOfCat.ToString();
            catLanaPortrait.DOColor(darkerColor, 0.5f);
            catPebblesPortrait.DOColor(hightlightedColor, 0.5f);

            nextLine = currentTalkingCatText + ": " + dialogeLine;
        }
        else if (nameOfCat == dialog.NameOfCat.Lana)
        {
            catLanaPortrait.gameObject.SetActive(true);
            catPebblesPortrait.gameObject.SetActive(true);
            currentTalkingCatText = nameOfCat.ToString();
            catLanaPortrait.DOColor(hightlightedColor, 0.5f);
            catPebblesPortrait.DOColor(darkerColor, 0.5f);

            nextLine = currentTalkingCatText + ": " + dialogeLine;
        }
        else {
            catLanaPortrait.gameObject.SetActive(false);
            catPebblesPortrait.gameObject.SetActive(false);

            catLanaPortrait.DOColor(hightlightedColor, 0.5f);
            catPebblesPortrait.DOColor(darkerColor, 0.5f);

            nextLine = " - " + dialogeLine;
        }


        StartCoroutine(TypeWritter(nextLine)); 
    }

    public string DisplayInfo(string infoId) {

        if (_lang == null){
            _lang = SharedState.LanguageDefs;
        }

        string infoText = _lang[infoId];

        return infoText;

    }

    private void DisplayNextSentenceSimple(TextTrigger.TypeOfText typeOfText) {
                        
        if (sentences.Count == 0){
            currentObjectivesText = "";
            EndDialogue(false);
            return;
        }


        dialog dialog = sentences.Dequeue();


        if (_lang == null){
            _lang = SharedState.LanguageDefs;
        }

        string currentLine = _lang[dialog.sentenceId];
        string nextLine;
        nextLine = currentLine;
        currentObjectivesText = currentObjectivesText + "-" + nextLine;

        if (typeOfText == TextTrigger.TypeOfText.objectivesTitle) {
            currentTMPElement.text = nextLine;
        }
        else if (typeOfText == TextTrigger.TypeOfText.objectives) {
            currentTMPElement.text = currentObjectivesText ;
        }

        DisplayNextSentenceSimple(typeOfText);
    }

    void EndDialogue(bool isConversation) {
        
        dialogBox.SetActive(false);
        catPebblesPortrait.gameObject.SetActive(false);
        catLanaPortrait.gameObject.SetActive(false);

        if (isConversation) {
            EventManager.instance.OnShowPromptActionUI(true);
        }

        gameManagerScript.playerIsTalking = false;
        tpsController.playerCanMoveTps = true;
        CheckEndDialogAction();        
    }

    IEnumerator TypeWritter(string dialogLine) {

        currentTMPElement.text = "";

        canGoNextSentence = false;

        foreach (char character in dialogLine) {
            currentTMPElement.text = currentTMPElement.text + character;
            yield return new WaitForSeconds(0.01f / 2);
        }

        canGoNextSentence = true;
    }

    private void CheckEndDialogAction() {

        switch (gameManagerScript.gameAction) {
            case GameManagerScript.GameAction.start:
                gameManagerScript.OpenGate();
                EventManager.instance.OnShowIngameUI(true);
                break;
            case GameManagerScript.GameAction.continueGame:
                EventManager.instance.OnShowIngameUI(true);
                break;
            case GameManagerScript.GameAction.levelCompleted:
                SceneManager.LoadScene(gameManagerScript.nextLevel);
                break;
            case GameManagerScript.GameAction.checkObjective:
                gameManagerScript.checkObjectives();
                break;
            case GameManagerScript.GameAction.objectivesNonCompleted:
                objectivesNonCompletedDialogue.TriggerTextAction();
                break;
            case GameManagerScript.GameAction.objectivesReview:
                objectivesNonCompletedDialogue.TriggerTextAction();
                break;
            case GameManagerScript.GameAction.switchToBoard:
                EventManager.instance.OnGoToBoard();
                break;
            case GameManagerScript.GameAction.nextDialogue:
                EventManager.instance.OnShowIngameUI(true);
                break;
            case GameManagerScript.GameAction.none:
                EventManager.instance.OnShowIngameUI(true);
                break;
            default:
                break;        
        }
    }
}
