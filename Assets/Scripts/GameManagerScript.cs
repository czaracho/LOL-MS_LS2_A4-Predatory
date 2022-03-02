using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoLSDK;
using SimpleJSON;

public class GameManagerScript : MonoBehaviour
{
    //vamos a poner todo lo relacionado a los objetivos
    public Organism[] organismsForObjectives;
    int objectivesAccomplishedCounter = 0;
    [HideInInspector]
    public bool objectivesAccomplished = false;
    [HideInInspector]
    public bool playerIsTalking = false;
    public bool levelHasObjectives = false;
    [HideInInspector]
    public bool isCheckingObjectives = false;
    public GameObject outerGate;

    public enum GameAction
    {
        none,
        start,
        checkObjective,
        levelCompleted,
        continueGame,
        objectivesNonCompleted
    }

    [HideInInspector]
    public GameAction gameAction = GameAction.none;


    private void Start()
    {

        Loader.photoCollection = new Photo[15];

        for (int i = 0; i < Loader.photoCollection.Length; i++)
        {
            Loader.photoCollection[i] = new Photo();
            Loader.photoCollection[i].photoIsSaved = false;
            Loader.photoCollection[i].animalName = Organism.AnimalName.typeGeneric;
        }

        if (levelHasObjectives) { 
        
        }
    }

    public void checkObjectives() {

        objectivesAccomplishedCounter = 0;

        if (!objectivesAccomplished) {
            for (int i = 0; i < organismsForObjectives.Length; i++)
            {
                for (int j = 0; j < Loader.photoCollection.Length; j++)
                {

                    if (organismsForObjectives[i].animalName == Loader.photoCollection[j].animalName)
                    {
                        objectivesAccomplishedCounter++;

                        organismsForObjectives[i].checkedObjective = true;

                        if (objectivesAccomplishedCounter == organismsForObjectives.Length)
                        {
                            objectivesAccomplished = true;
                            ObjectivesAccomplished();
                            return;
                        }
                    }
                }
            }

            int uncheckedObjectiveCounter = 0;
            List<Organism> organismList = new List<Organism>();

            for (int i = 0; i < organismsForObjectives.Length; i++) {

                if (!organismsForObjectives[i].checkedObjective) {
                    organismList.Add(organismsForObjectives[i]);
                    uncheckedObjectiveCounter++;
                }

                organismsForObjectives[i].checkedObjective = false;
            }

            ObjectivesUnaccomplished(organismList);
        }
    }

    public void ObjectivesAccomplished() {

        Dialogue endDialog = new Dialogue();
        endDialog.sentences = new dialog[1];
        endDialog.sentences[0] = new dialog();
        endDialog.sentences[0].sentenceId = "ldline_objective_accomplished";

        FindObjectOfType<DialogManager>().StartDialogue(endDialog, GameAction.levelCompleted);

    }

    public void ObjectivesUnaccomplished(List<Organism> organisms) {
        
        Debug.Log("Objectives unnacomplished");


        Dialogue endDialog = new Dialogue();
        endDialog.sentences = new dialog[1];
        endDialog.sentences[0] = new dialog();
        endDialog.sentences[0].sentenceId = "ldline_objective_failed_1";

        FindObjectOfType<DialogManager>().StartDialogue(endDialog, GameAction.objectivesNonCompleted);
    }

    public void SetGameAction(GameAction newGameAction) {
        gameAction = newGameAction;
    }

    public void OpenGate() {
        outerGate.SetActive(false);
    }
}

