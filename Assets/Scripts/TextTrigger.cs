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
        info
    }

    public TypeOfText typeOfText;
    public GameManagerScript.GameAction action;

    public void TriggerTextAction()
    {
        if (typeOfText == TypeOfText.dialog)
        {
            FindObjectOfType<TextManager>().StartDialogue(dialogue, action, currentTextElement);
        }
    }

    private void Start()
    {
        if (typeOfText == TypeOfText.objectivesTitle)
        {
            FindObjectOfType<TextManager>().GetTextForTitlesAndObjectives(dialogue, typeOfText, currentTextElement);
        }
        else if (typeOfText == TypeOfText.objectives) {
            FindObjectOfType<TextManager>().GetTextForTitlesAndObjectives(dialogue, typeOfText, currentTextElement);
        }
    }

    IEnumerator WaitToShowObjectives() {
        yield return new WaitForSeconds(waitToShow);
        FindObjectOfType<TextManager>().GetTextForTitlesAndObjectives(dialogue, typeOfText, currentTextElement);
    }
}
