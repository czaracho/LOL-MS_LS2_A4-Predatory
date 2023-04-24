using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoLSDK;
using SimpleJSON;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using static Delegates;

public class GameManagerScript : MonoBehaviour
{
    //vamos a poner todo lo relacionado a los objetivos
    int objectivesAccomplishedCounter = 0;
    public bool checkForObjectiveName = true;

    [HideInInspector]
    public bool objectivesAccomplished = false;
    [HideInInspector]
    public bool playerIsTalking = false;
    public bool isNormalLevel = false;
    [HideInInspector]
    public bool isCheckingObjectives = false;

    public OrganismObject[] organismsForObjectives;
    public GameObject outerGate;
    public GameObject CatHeadSilhouette;
    public TextTrigger levelCompleteDialog;
    public TextTrigger levelNonCompletedDialog;

    public static GameManagerScript instance;

    public string nextLevel = "TentLevel1";

    //public delegate void EventHandler();
    //public event EventHandler OnSuchEvent;


    public enum GameAction
    {
        none,
        start,
        checkObjective,
        levelCompleted,
        continueGame,
        objectivesNonCompleted,
        objectivesReview,
        switchToBoard,
        nextDialogue
    }

    [HideInInspector]
    public GameAction gameAction = GameAction.none;

    private void Awake()
    {
        if (instance != null){
            return;
        }
        else{
            instance = this;
        }
    }

    private void Start()
    {
        if (isNormalLevel) {
            
            Loader.photoCollection = new Photo[Constants.TOTAL_PHOTO_SLOTS];

            for (int i = 0; i < Loader.photoCollection.Length; i++)
            {
                Loader.photoCollection[i] = new Photo();
                Loader.photoCollection[i].photoIsSaved = false;
                Loader.photoCollection[i].photoAnimalName = OrganismObject.AnimalName.typeGeneric;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown("f1"))
        {
            SceneManager.LoadScene(nextLevel);
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

                        if (organismsForObjectives[i].animalName == Loader.photoCollection[j].photoAnimalName)
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
                        if (organismsForObjectives[i].animalType == Loader.photoCollection[j].photoAnimalType)
                        {
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
            List<OrganismObject> organismList = new List<OrganismObject>();

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

        levelCompleteDialog.TriggerTextAction();

    }

    public void ObjectivesUnaccomplished(List<OrganismObject> organisms) {

        levelNonCompletedDialog.TriggerTextAction();
    }

    public void SetGameAction(GameAction newGameAction) {
        gameAction = newGameAction;
    }

    public void OpenGate() {
        outerGate.SetActive(false);
    }

    public void StartBoardConversation() {        
        NewActionVoidDelegate newDelegate = EventManager.instance.OnStartBoardInitialConversation; //Assignated event to start the first dialog in the Tent Scene
        RemoveBlackCatSilhouetteForeground(newDelegate);
    }

    public void SetBlackCatSilhouetteForeground(NewActionVoidDelegate newActionVoidDelegate)
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(CatHeadSilhouette.transform.DOScale(new Vector3(6f, 6f, 1), Constants.SET_DURATION)).OnComplete(() => { newActionVoidDelegate();});
    }

    public void RemoveBlackCatSilhouetteForeground(NewActionVoidDelegate newActionVoidDelegate) {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(CatHeadSilhouette.transform.DOScale(new Vector3(0, 0, 0), Constants.REMOVE_DURATION)).OnComplete(()=> { newActionVoidDelegate();});
    }

}

public static class Constants
{
    public const int TOTAL_PHOTO_SLOTS = 15;     //Total amount of the photo slots available
    public const float REMOVE_DURATION = 1.5f;
    public const float SET_DURATION = 1f;
}

public static class Delegates {
    public delegate void NewActionVoidDelegate();
}


