using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public TextMeshProUGUI currentTextElement;
    private float waitToShow = 2.0f;
    public enum TypeOfText { 
        objectivesTitle,
        objectives,
        dialog
    }

    public TypeOfText typeOfText;
    public GameManagerScript.GameAction action;

    public void TriggerDialogue()
    {
        if (typeOfText == TypeOfText.dialog) {
            FindObjectOfType<DialogManager>().StartDialogue(dialogue, action, currentTextElement);
        }
    }

    private void Start()
    {
        if (typeOfText == TypeOfText.objectivesTitle)
        {
            FindObjectOfType<DialogManager>().GetTextForTitlesAndObjectives(dialogue, typeOfText, currentTextElement);
        }
        else if (typeOfText == TypeOfText.objectives) {
            //StartCoroutine(WaitToShowObjectives());
            FindObjectOfType<DialogManager>().GetTextForTitlesAndObjectives(dialogue, typeOfText, currentTextElement);

        }
    }

    IEnumerator WaitToShowObjectives() {
        yield return new WaitForSeconds(waitToShow);
        FindObjectOfType<DialogManager>().GetTextForTitlesAndObjectives(dialogue, typeOfText, currentTextElement);
    }
}
