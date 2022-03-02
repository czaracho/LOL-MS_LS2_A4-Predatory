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
    public TextMeshProUGUI dialogText;
    public Button nextButton;
    public Color hightlightedColor;
    public Color darkerColor;
    private bool canGoNextSentence;

    private string currentTalkingCat = "";

    public SimpleSampleCharacterControl tpsController;

    JSONNode _lang;

    public GameManagerScript gameManagerScript;

    private void Start()
    {
        sentences = new Queue<dialog>();
        dialogText.text = "";
        _lang = SharedState.LanguageDefs;
        nextButton.onClick.AddListener(DisplayNextSentence);
        tpsController = FindObjectOfType<SimpleSampleCharacterControl>();
    }

    public void StartDialogue(Dialogue dialogue, GameManagerScript.GameAction gameAction) {

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
        currentTalkingCat = dialog.GetCharacterName();

        string nextLine;
        //pdline = pebbles dialog line
        //ldline = lana dialog line
        if (currentTalkingCat == "pdline")
        {
            catLanaPortrait.gameObject.SetActive(true);
            catPebblesPortrait.gameObject.SetActive(true);

            currentTalkingCat = "Pebbles";

            catLanaPortrait.DOColor(darkerColor, 0.5f);
            catPebblesPortrait.DOColor(hightlightedColor, 0.5f);

            nextLine = currentTalkingCat + ": " + dialogeLine;
        }
        else if (currentTalkingCat == "ldline")
        {
            catLanaPortrait.gameObject.SetActive(true);
            catPebblesPortrait.gameObject.SetActive(true);

            currentTalkingCat = "Lana";

            catLanaPortrait.DOColor(hightlightedColor, 0.5f);
            catPebblesPortrait.DOColor(darkerColor, 0.5f);

            nextLine = currentTalkingCat + ": " + dialogeLine;
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
        
        dialogText.text = "";

        canGoNextSentence = false;

        foreach (char character in dialogLine) {
            dialogText.text = dialogText.text + character;
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
