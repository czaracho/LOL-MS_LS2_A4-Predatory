using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoLSDK;
using SimpleJSON;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class DialogManager : MonoBehaviour
{
    //private Queue<string> sentences;
    private Queue<dialog> sentences;
    private Queue<dialog> objectivesAccomplishedSentences;
    private Queue<dialog> objectivesUnacomplishedSentences;

    public DialogTrigger objectivesDialogue;

    public Image catLanaPortrait;
    public Image catPebblesPortrait;
    public GameObject dialogBox;
    public TextMeshProUGUI dialogText;
    public Button nextButton;
    public Color hightlightedColor;
    public Color darkerColor;

    private string currentTalkingCat = "";

    JSONNode _lang;

    public GameManagerScript gameManagerScript;

    private void Start()
    {
        sentences = new Queue<dialog>();
        dialogText.text = "";
        _lang = SharedState.LanguageDefs;
        nextButton.onClick.AddListener(DisplayNextSentence);
    }

    public void StartDialogue(Dialogue dialogue, GameManagerScript.GameAction gameAction) {
        
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
        
        if (sentences.Count == 0) {
            EndDialogue();
            return;
        }

        dialog dialog = sentences.Dequeue();
        string dialogeLine = _lang[dialog.sentenceId];
        currentTalkingCat = dialog.GetCharacterName();

        string nextLine = "";
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

        CheckEndDialogAction();        
    }

    IEnumerator TypeWritter(string dialogLine) {

        dialogText.text = "";

        foreach (char character in dialogLine) {
            dialogText.text = dialogText.text + character;
            yield return new WaitForSeconds(0.01f / 2);
        }
    }

    private void CheckEndDialogAction() {

        switch (gameManagerScript.gameAction) {
            case GameManagerScript.GameAction.levelCompleted:
                Debug.Log("level completed bro");
                break;
            case GameManagerScript.GameAction.checkObjective:
                Debug.Log("chequeando objectivo bro");
                gameManagerScript.checkObjectives();
                break;
            case GameManagerScript.GameAction.objectivesNonCompleted:
                Debug.Log("objetivos no completados broski");
                objectivesDialogue.TriggerDialogue();
                break;
            default:
                break;        
        }
    }
}
