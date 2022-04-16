using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoLSDK;
using SimpleJSON;
using UnityEngine.SceneManagement;


public class GameManagerScript : MonoBehaviour
{
    //vamos a poner todo lo relacionado a los objetivos
    public Organism[] organismsForObjectives;
    public bool checkForObjectiveName = true;
    int objectivesAccomplishedCounter = 0;
    [HideInInspector]
    public bool objectivesAccomplished = false;
    [HideInInspector]
    public bool playerIsTalking = false;
    public bool levelHasObjectives = false;
    [HideInInspector]
    public bool isCheckingObjectives = false;
    public GameObject outerGate;
    public TextTrigger levelCompleteDialog;
    public TextTrigger levelNonCompletedDialog;


    public enum GameAction
    {
        none,
        start,
        checkObjective,
        levelCompleted,
        continueGame,
        objectivesNonCompleted,
        objectivesReview
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

    private void Update()
    {
        if (Input.GetKeyDown("f1"))
        {
            SceneManager.LoadScene("TentLevel1");
        }
    }

    public void checkObjectives() {

        objectivesAccomplishedCounter = 0;

        if (!objectivesAccomplished) {
            for (int i = 0; i < organismsForObjectives.Length; i++)
            {
                for (int j = 0; j < Loader.photoCollection.Length; j++)
                {
                    if (checkForObjectiveName)
                    {
                        Debug.Log("checked for objectivename");

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
                    else {                        
                        if (organismsForObjectives[i].animalType == Loader.photoCollection[j].animalType)
                        {
                            Debug.Log("give it a shot");
                            objectivesAccomplishedCounter++;
                            organismsForObjectives[i].checkedObjective = true;

                            if (objectivesAccomplishedCounter == organismsForObjectives.Length)
                            {
                                Debug.Log("objectivesAccomplishedCounter: " + objectivesAccomplishedCounter);
                                Debug.Log("organismsForObjectives: " + organismsForObjectives.Length);
                                objectivesAccomplished = true;
                                ObjectivesAccomplished();
                                return;
                            }
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

        //Dialogue endDialog = new Dialogue();
        //endDialog.sentences = new dialog[1];
        //endDialog.sentences[0] = new dialog();
        //endDialog.sentences[0].sentenceId = "ldline_objective_accomplished";

        levelCompleteDialog.TriggerTextAction();

    }

    public void ObjectivesUnaccomplished(List<Organism> organisms) {
        //levelNonCompletedDialog.TriggerTextAction();
        //Dialogue endDialog = new Dialogue();
        //endDialog.sentences = new dialog[1];
        //endDialog.sentences[0] = new dialog();
        //endDialog.sentences[0].sentenceId = "ldline_objective_failed_1";

        levelNonCompletedDialog.TriggerTextAction();
    }

    public void SetGameAction(GameAction newGameAction) {
        gameAction = newGameAction;
    }

    public void OpenGate() {
        outerGate.SetActive(false);
    }
}

