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
    public GameObject playerStartPosition;
    public GameObject ingameCanvas;

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

    public void CellphoneTeleportCat() {
        Debug.Log("Gato teletransportado");
        playerCat.transform.position = playerStartPosition.transform.position;
    }


}
