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

public class GameManagerScript : MonoBehaviour {
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

    //public OrganismObject[] organismsForObjectivesOld;
    public GameObject outerGate;
    public GameObject catHeadSilouette;
    public TextTrigger levelCompleteDialog;
    public TextTrigger levelNonCompletedDialog;

    public static GameManagerScript instance;

    public string nextLevel = "TentLevel1";

    public GameObject player;
    public Transform playerStartingPosition;

    public OrganismIdentifier[] objectivesOrganisms;

    public TextTrigger helpConversation;


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
        nextDialogue,
        genericAction
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
        catHeadSilouette.SetActive(true);
        RemoveBlackCatSilhouetteForeground(null);

        if (isNormalLevel) {

            Loader.photoCollection = new Photo[Constants.TOTAL_PHOTO_SLOTS];

            for (int i = 0; i < Loader.photoCollection.Length; i++) {
                Loader.photoCollection[i] = new Photo();
                Loader.photoCollection[i].photoIsSaved = false;
                Loader.photoCollection[i].photoAnimalName = OrganismObject.AnimalName.typeGeneric;
            }
        }        
    }

    private void Update()
    {
        //if (Input.GetKeyDown("f1"))
        //{
        //    SceneManager.LoadScene(nextLevel);
        //}
    }

    //public void checkObjectives() {

    //    objectivesAccomplishedCounter = 0;

    //    if (!objectivesAccomplished) {
    //        for (int i = 0; i < organismsForObjectivesOld.Length; i++)
    //        {
    //            for (int j = 0; j < Loader.photoCollection.Length; j++)
    //            {
    //                if (checkForObjectiveName)
    //                {
    //                    Debug.Log("checked for objectivename");

    //                    if (organismsForObjectivesOld[i].animalName == Loader.photoCollection[j].photoAnimalName)
    //                    {
    //                        objectivesAccomplishedCounter++;
    //                        organismsForObjectivesOld[i].checkedObjective = true;

    //                        if (objectivesAccomplishedCounter == organismsForObjectivesOld.Length)
    //                        {
    //                            objectivesAccomplished = true;
    //                            ObjectivesAccomplished();
    //                            return;
    //                        }
    //                    }
    //                }
    //                else {                        
    //                    if (organismsForObjectivesOld[i].animalType == Loader.photoCollection[j].photoAnimalType)
    //                    {
    //                        objectivesAccomplishedCounter++;
    //                        organismsForObjectivesOld[i].checkedObjective = true;

    //                        if (objectivesAccomplishedCounter == organismsForObjectivesOld.Length)
    //                        {
    //                            Debug.Log("objectivesAccomplishedCounter: " + objectivesAccomplishedCounter);
    //                            Debug.Log("organismsForObjectives: " + organismsForObjectivesOld.Length);
    //                            objectivesAccomplished = true;
    //                            ObjectivesAccomplished();
    //                            return;
    //                        }
    //                    }
    //                }

    //            }
    //        }

    //        int uncheckedObjectiveCounter = 0;
    //        List<OrganismObject> organismList = new List<OrganismObject>();

    //        for (int i = 0; i < organismsForObjectivesOld.Length; i++) {

    //            if (!organismsForObjectivesOld[i].checkedObjective) {
    //                organismList.Add(organismsForObjectivesOld[i]);
    //                uncheckedObjectiveCounter++;
    //            }

    //            organismsForObjectivesOld[i].checkedObjective = false;
    //        }

    //        ObjectivesUnaccomplished(organismList);
    //    }
    //}    

    public void checkObjectives() {

        objectivesAccomplishedCounter = 0;
        Debug.Log("Total objectives organisms: " + objectivesOrganisms.Length);

        for (int i = 0; i < objectivesOrganisms.Length; i++) {
            for (int j = 0; j < Loader.photoCollection.Length; j++) {

                if (Loader.photoCollection[j].checkedForReview)
                    continue;

                //if (objectivesOrganisms[i].isChecked)
                //    continue;

                if (objectivesOrganisms[i].checkByName) {

                    if (objectivesOrganisms[i].animalName == Loader.photoCollection[j].photoAnimalName) {

                        objectivesAccomplishedCounter++;
                        //objectivesOrganisms[i].isChecked = true;
                        Loader.photoCollection[j].checkedForReview = true;

                        Debug.Log("###########CHECKEDBYNAME###############");
                        Debug.Log("Matched the organism by name");
                        Debug.Log("objectivesOrganisms[i].animalName: " + objectivesOrganisms[i].animalName);
                        Debug.Log("Loader.photoCollection[j].photoAnimalName: " + Loader.photoCollection[j].photoAnimalName);
                        Debug.Log("Current counter: " + objectivesAccomplishedCounter);

                        if (objectivesAccomplishedCounter == objectivesOrganisms.Length) {
                            objectivesAccomplished = true;
                            ObjectivesAccomplished();
                            return;
                        }
                    }
                }
                else {

                    if (objectivesOrganisms[i].animalType == Loader.photoCollection[j].photoAnimalType) {

                        objectivesAccomplishedCounter++;
                        //objectivesOrganisms[i].isChecked = true;
                        Loader.photoCollection[j].checkedForReview = true;

                        Debug.Log("###########CHECKEDBYTYPE###############");
                        Debug.Log("Matched the organism type");
                        Debug.Log("objectivesOrganisms[i].animalType: " + objectivesOrganisms[i].animalType);
                        Debug.Log("Loader.photoCollection[j].photoAnimalType: " + Loader.photoCollection[j].photoAnimalType);
                        Debug.Log("Current counter: " + objectivesAccomplishedCounter);

                        if (objectivesAccomplishedCounter == objectivesOrganisms.Length) {
                            objectivesAccomplished = true;
                            ObjectivesAccomplished();
                            return;
                        }
                    }
                }                

            }
        }

        ObjectivesUnaccomplished();
    }

    public void ObjectivesAccomplished() {
        Debug.Log("Correct objectives after review: " + objectivesAccomplishedCounter);
        levelCompleteDialog.TriggerTextAction();

    }

    //public void ObjectivesUnaccomplished(List<OrganismObject> organisms) {

    //    levelNonCompletedDialog.TriggerTextAction();
    //}

    public void ObjectivesUnaccomplished() {

        ResetPhotoCheckedStatus();
        levelNonCompletedDialog.TriggerTextAction();
    }

    public void SetGameAction(GameAction newGameAction) {
        gameAction = newGameAction;
    }

    private void ResetPhotoCheckedStatus() {
        for (int i = 0; i < objectivesOrganisms.Length; i++) { 
            objectivesOrganisms[i].isChecked = false;
        }
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
        mySequence.Append(catHeadSilouette.transform.DOScale(new Vector3(6f, 6f, 1), Constants.SET_DURATION)).OnComplete(() => { newActionVoidDelegate();});
    }

    public void RemoveBlackCatSilhouetteForeground(NewActionVoidDelegate newActionVoidDelegate) {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(catHeadSilouette.transform.DOScale(new Vector3(0, 0, 0), Constants.REMOVE_DURATION)).OnComplete(()=> { newActionVoidDelegate();});
    }

    public void MovePlayerAwayFromNPC() {
        player.transform.position = playerStartingPosition.transform.position;
    }

    public void HelpConversation() {
        playerIsTalking = true;
        helpConversation.TriggerTextAction();                
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


