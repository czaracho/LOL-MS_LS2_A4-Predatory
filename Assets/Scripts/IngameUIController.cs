using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LoLSDK;
using SimpleJSON;

public class IngameUIController : MonoBehaviour
{
    public GameObject IngameUI;
    [HideInInspector]
    public bool albumIsOpen = false;
    public static IngameUIController instance;
    public GameObject notificationBlock;
    public GameObject animalNamesLabelCanvas;
    public GameObject objectivesList;
    public GameObject playerCat;
    public GameObject playerCatFirstPerson;
    public GameObject playerStartPosition;
    public GameObject ingameCanvas;

    //Objectives
    public GameObject showObjectivesArrow;
    public GameObject hideObjectivesArrow;
    public GameObject objectivesListText;


    private bool showObjectives = true;

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        EventManager.instance.ShowPromptActionUI += ShowNotificationText;
        EventManager.instance.ShowAnimalNames += ShowAnimalNames;
    }

    private void OnDestroy()
    {
        EventManager.instance.ShowPromptActionUI -= ShowNotificationText;
        EventManager.instance.ShowAnimalNames -= ShowAnimalNames;
    }


    public void ShowNotificationText(bool show) {

        if (show)
        {
            notificationBlock.gameObject.SetActive(true);
        }
        else {
            notificationBlock.gameObject.SetActive(false);
        }
    }

    public void ShowAnimalNames(bool show) {

        if (show){
            animalNamesLabelCanvas.SetActive(true);
        }
        else {
            animalNamesLabelCanvas.SetActive(false);
        }
    }

    public void CellphoneTeleportCatTransition() {
        GameManagerScript.instance.SetBlackCatSilhouetteForeground(() => TeleportPlayerFadeOut());
    }

    public void TeleportPlayerFadeOut() {
        PlayerTeleport();
       GameManagerScript.instance.RemoveBlackCatSilhouetteForeground(null);
    }

    public void PlayerTeleport() {
        playerCat.transform.position = playerStartPosition.transform.position;
        //playerCatFirstPerson.transform.position = new Vector3(playerCat.transform.position.x, playerCat.transform.position.y, playerCat.transform.position.z);
        playerCatFirstPerson.transform.position = new Vector3(playerCat.transform.position.x, playerCat.transform.position.y + 0.5f, playerCat.transform.position.z); ;
    }

    public void ShowHideObjectives() {

        if (showObjectives) {
            objectivesListText.SetActive(false);
            showObjectivesArrow.SetActive(false);
            hideObjectivesArrow.SetActive(true);
            showObjectives = false;
        }
        else {
            objectivesListText.SetActive(true);
            showObjectivesArrow.SetActive(true);
            hideObjectivesArrow.SetActive(false);
            showObjectives = true;
        }

    }
}
