using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public TextMeshProUGUI currentTextElement;
    private float waitToShow = 2.0f;
    public enum TypeOfText { 
        objectivesTitle,
        objectives,
        dialog,
        info,
        newEvent
    }

    public TypeOfText typeOfText;
    public GameManagerScript.GameAction action;

    private void Start()
    {
        if (typeOfText == TypeOfText.objectivesTitle)
        {
            FindObjectOfType<TextManager>().GetTextForTitlesAndObjectives(dialogue, typeOfText, currentTextElement);
        }
        else if (typeOfText == TypeOfText.objectives)
        {
            FindObjectOfType<TextManager>().GetTextForTitlesAndObjectives(dialogue, typeOfText, currentTextElement);
        }
    }

    public void TriggerTextAction()
    {
        if (typeOfText == TypeOfText.dialog)
        {
            FindObjectOfType<TextManager>().StartDialogue(dialogue, action, currentTextElement);
        }
    }

    public void TriggerStartingDialog() {
        //StartCoroutine(WaitToShowTrigger());
        if (typeOfText == TypeOfText.dialog)
        {
            FindObjectOfType<TextManager>().StartDialogue(dialogue, action, currentTextElement);
        }
    }


    IEnumerator WaitToShowObjectives() {
        yield return new WaitForSeconds(waitToShow);
        FindObjectOfType<TextManager>().GetTextForTitlesAndObjectives(dialogue, typeOfText, currentTextElement);
    }

    IEnumerator WaitToShowTrigger() {
        yield return new WaitForSeconds(1.0f);
        if (typeOfText == TypeOfText.dialog)
        {
            FindObjectOfType<TextManager>().StartDialogue(dialogue, action, currentTextElement);
        }
    }
}
