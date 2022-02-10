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
        //sentences = new Queue<string>();
        sentences = new Queue<dialog>();
        dialogText.text = "";
        _lang = SharedState.LanguageDefs;
        nextButton.onClick.AddListener(DisplayNextSentence);


    }

    public void StartDialogue(Dialogue dialogue) {

        catLanaPortrait.gameObject.SetActive(true);
        catPebblesPortrait.gameObject.SetActive(true);
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

        //pdline = pebbles dialog line
        //ldline = lana dialog line
        if (currentTalkingCat == "pdline")
        {
            currentTalkingCat = "Pebbles";
            catLanaPortrait.color = darkerColor;
            catPebblesPortrait.color = hightlightedColor;

            catLanaPortrait.DOColor(darkerColor, 1f);
            catPebblesPortrait.DOColor(hightlightedColor, 1f);


        }
        else if (currentTalkingCat == "ldline") {
            currentTalkingCat = "Lana";
            catLanaPortrait.color = hightlightedColor;
            catPebblesPortrait.color = darkerColor;

            catLanaPortrait.DOColor(hightlightedColor, 1f);
            catPebblesPortrait.DOColor(darkerColor, 1f);
        }
        
        string nextLine = currentTalkingCat + ": " + dialogeLine;
        StartCoroutine(TypeWritter(nextLine)); 
    }


    void EndDialogue() {
        dialogBox.SetActive(false);
        catPebblesPortrait.gameObject.SetActive(false);
        catLanaPortrait.gameObject.SetActive(false);
        EventManager.instance.OnShowPromptActionUI(true);
        gameManagerScript.playerIsTalking = false;
    }

    IEnumerator TypeWritter(string dialogLine) {

        dialogText.text = "";

        foreach (char character in dialogLine) {
            dialogText.text = dialogText.text + character;
            yield return new WaitForSeconds(0.01f / 2);
        }
    }
}
