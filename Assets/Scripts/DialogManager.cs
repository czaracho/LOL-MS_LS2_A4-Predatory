using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoLSDK;
using SimpleJSON;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Supercyan.AnimalPeopleSample;

public class DialogManager : MonoBehaviour
{
    //private Queue<string> sentences;
    private Queue<dialog> sentences;

    public DialogTrigger objectivesDialogue;

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
            sentences = new Queue<dialog>();
        }

        canGoNextSentence = true;
        gameManagerScript.gameAction = gameAction;
        catLanaPortrait.gameObject.SetActive(false);
        catPebblesPortrait.gameObject.SetActive(false);
        catLanaPortrait.DOColor(darkerColor, 1f);
        catPebblesPortrait.DOColor(darkerColor, 1f);
        dialogBox.SetActive(true);

        sentences.Clear();

        foreach (dialog sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void GetTextForTitlesAndObjectives(Dialogue dialogue, DialogTrigger.TypeOfText typeOfText, TextMeshProUGUI currentTextElement) {

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


        if (!canGoNextSentence)
            return;

        if (sentences.Count == 0) {
            EndDialogue();
            return;
        }


        dialog dialog = sentences.Dequeue();


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


    private void DisplayNextSentenceSimple(DialogTrigger.TypeOfText typeOfText) {
        
                
        if (sentences.Count == 0)
        {
            currentObjectivesText = "";
            EndDialogue();
            return;
        }


        dialog dialog = sentences.Dequeue();


        if (_lang == null)
        {
            _lang = SharedState.LanguageDefs;
        }

        string currentLine = _lang[dialog.sentenceId];
        string nextLine;
        nextLine = currentLine;
        currentObjectivesText = currentObjectivesText + nextLine;

        if (typeOfText == DialogTrigger.TypeOfText.objectivesTitle) {
            currentTMPElement.text = nextLine;
        }
        else if (typeOfText == DialogTrigger.TypeOfText.objectives) {
            currentTMPElement.text = currentObjectivesText ;

        }

        DisplayNextSentenceSimple(typeOfText);
    }


    void EndDialogue() {
        dialogBox.SetActive(false);
        catPebblesPortrait.gameObject.SetActive(false);
        catLanaPortrait.gameObject.SetActive(false);
        EventManager.instance.OnShowPromptActionUI(true);
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
                break;
            case GameManagerScript.GameAction.checkObjective:
                gameManagerScript.checkObjectives();
                break;
            case GameManagerScript.GameAction.objectivesNonCompleted:
                objectivesDialogue.TriggerDialogue();
                break;
            case GameManagerScript.GameAction.none:
                EventManager.instance.OnShowIngameUI(true);
                break;
            default:
                break;        
        }
    }
}
