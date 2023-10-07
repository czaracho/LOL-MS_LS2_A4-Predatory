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
    public bool isFieldLevel = true;

    [HideInInspector]
    public bool objectivesAccomplished = false;
    [HideInInspector]
    public bool playerIsTalking = false;
    [HideInInspector]
    public bool isCheckingObjectives = false;

    public GameObject outerGate;
    public GameObject catHeadSilouette;
    public TextTrigger levelCompleteDialog;
    public TextTrigger levelNonCompletedDialog;

    public static GameManagerScript instance;

    public string currentLevel = "";
    public string nextLevel = "TentLevel1";

    public GameObject player;
    public Transform playerStartingPosition;

    public OrganismIdentifier[] objectivesOrganisms;

    public TextTrigger helpConversation;

    public ObjectiveField[] objectives;

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
        genericAction,
        incorrectBoard,
        gameCompleted,
        showTakingPhotoRules
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
        InitUI();

        if (isFieldLevel)
            ResetPhotoCollection();

        Loader.PROGRESS_DATA.currentLevel = currentLevel;
        Loader.SaveData();

    }

    private void Update()
    {
        if (Input.GetKeyDown("f1")) {
            SceneManager.LoadScene(nextLevel);
        }
    }

    private void InitUI() {
        catHeadSilouette.SetActive(true);
        RemoveBlackCatSilhouetteForeground(null);
    }

    private void ResetPhotoCollection() {

        SnapshotController.photosTakenQuantity = 0;

        //Loader.photoCollection = new Photo[Constants.TOTAL_PHOTO_SLOTS];
        //Debug.Log("ProgressData test: " + Loader.PROGRESS_DATA.photoCollection.Length);
        Loader.PROGRESS_DATA.photoCollection = new Photo[Constants.TOTAL_PHOTO_SLOTS];

        for (int i = 0; i < Loader.PROGRESS_DATA.photoCollection.Length; i++) {
            Loader.PROGRESS_DATA.photoCollection[i] = new Photo();
            Loader.PROGRESS_DATA.photoCollection[i].photoIsSaved = false;
            Loader.PROGRESS_DATA.photoCollection[i].photoAnimalName = OrganismObject.AnimalName.typeGeneric;
            Loader.PROGRESS_DATA.photoCollection[i].photoAnimalType = OrganismObject.AnimalType.typeGeneric;
            Loader.PROGRESS_DATA.photoCollection[i].picture = null;
            //Loader.PROGRESS_DATA.photoCollection[i].byteArrayPicture = null;
        }
    }

    public void checkObjectives() {

        int totalObjectivesAccomplished = 0;

        for (int i = 0; i < objectives.Length; i++) {

            if (objectives[i].objectivesAccomplished) {
                totalObjectivesAccomplished++;
            }

        }

        if (totalObjectivesAccomplished == objectives.Length) {
            ObjectivesAccomplished();
        }
        else {
            ObjectivesUnaccomplished(); 
        }

    }

    private void checkObjectivesByOrganism() {

        UncheckReviewedPhotos();

        objectivesAccomplishedCounter = 0;
        Debug.Log("Total objectives organisms: " + objectivesOrganisms.Length);
        Debug.Log("Total en el Loader.photoCollection.Length: " + Loader.PROGRESS_DATA.photoCollection.Length);


        for (int i = 0; i < objectivesOrganisms.Length; i++) {
            for (int j = 0; j < Loader.PROGRESS_DATA.photoCollection.Length; j++) {

                if (Loader.PROGRESS_DATA.photoCollection[j].checkedForReview)
                    continue;


                if (objectivesOrganisms[i].checkByName) {

                    if (objectivesOrganisms[i].animalName == Loader.PROGRESS_DATA.photoCollection[j].photoAnimalName) {

                        objectivesAccomplishedCounter++;
                        Loader.PROGRESS_DATA.photoCollection[j].checkedForReview = true;

                        Debug.Log("###########CHECKEDBYNAME###############");
                        Debug.Log("Matched the organism by name");
                        Debug.Log("objectivesOrganisms[i].animalName: " + objectivesOrganisms[i].animalName);
                        Debug.Log("Loader.photoCollection[j].photoAnimalName: " + Loader.PROGRESS_DATA.photoCollection[j].photoAnimalName);
                        Debug.Log("Current counter: " + objectivesAccomplishedCounter);

                        if (objectivesAccomplishedCounter == objectivesOrganisms.Length) {
                            objectivesAccomplished = true;
                            ObjectivesAccomplished();
                            return;
                        }
                    }
                }
                else {

                    if (objectivesOrganisms[i].animalType == Loader.PROGRESS_DATA.photoCollection[j].photoAnimalType) {

                        objectivesAccomplishedCounter++;
                        Loader.PROGRESS_DATA.photoCollection[j].checkedForReview = true;

                        Debug.Log("###########CHECKEDBYTYPE###############");
                        Debug.Log("Matched the organism type");
                        Debug.Log("objectivesOrganisms[i].animalType: " + objectivesOrganisms[i].animalType);
                        Debug.Log("Loader.photoCollection[j].photoAnimalType: " + Loader.PROGRESS_DATA.photoCollection[j].photoAnimalType);
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

    private void UncheckReviewedPhotos() {
        
        for (int i = 0; i < Loader.PROGRESS_DATA.photoCollection.Length; i++) {
            Loader.PROGRESS_DATA.photoCollection[i].checkedForReview = false;
        }
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
        mySequence.Append(catHeadSilouette.transform.DOScale(new Vector3(6f, 6f, 1), Constants.SET_DURATION)).OnComplete(() => { newActionVoidDelegate?.Invoke();});
    }

    public void RemoveBlackCatSilhouetteForeground(NewActionVoidDelegate newActionVoidDelegate) {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(catHeadSilouette.transform.DOScale(new Vector3(0, 0, 0), Constants.REMOVE_DURATION)).OnComplete(()=> { newActionVoidDelegate?.Invoke();});
    }

    public void MovePlayerAwayFromNPC() {
        player.transform.position = playerStartingPosition.transform.position;
    }

    public void HelpConversation() {
        //playerIsTalking = true;
        helpConversation.TriggerTextAction();                
    }

    public void RestartCurrentScene() {
        SetBlackCatSilhouetteForeground(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));        
    }

    public void AddGameProgress() {
        
        Loader.PROGRESS_DATA.currentProgress++;
        LOLSDK.Instance.SubmitProgress(1, Loader.PROGRESS_DATA.currentProgress, Loader.PROGRESS_DATA.maxProgress);
        Loader.SaveData();
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


