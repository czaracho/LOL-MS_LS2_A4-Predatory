using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Supercyan.AnimalPeopleSample;

public class Tutorial : MonoBehaviour
{
    public GameObject pebblesTutorialCharacter;
    public GameObject cameraTutorialLayout;
    public GameObject tutorialLayout;
    public GameObject ingameLayout;
    public GameObject npcLayout;
    public GameObject ingameCamera;
    public GameObject tutorialCamera;
    public GameObject clickText;

    public SimpleSampleCharacterControl playerController;

    private bool canClickScreen = false;
    private bool movementTutorialAllowed = true;

    private void Awake() {
        EventManager.instance.GenericAction += showCameraTutorialLayout;
    }

    private void OnDestroy() {
        EventManager.instance.GenericAction -= showCameraTutorialLayout;
    }

    public void Start() {
        StartCoroutine(InitTutorialElements());        
    }

    public void Update() {

        if (!movementTutorialAllowed)
            return;

        if (!canClickScreen) {
            return;
        }
        else {
            if (Input.GetMouseButtonDown(0)) {
                HideTutorialLayout();
            }
        }

    }

    public void HideTutorialLayout() {
        
        playerController.playerCanMoveTps = true;
        GameManagerScript.instance.playerIsTalking = false;
        pebblesTutorialCharacter.SetActive(false);
        movementTutorialAllowed = false;

        //Layouts
        tutorialLayout.SetActive(false);
        npcLayout.SetActive(true);
        ingameLayout.SetActive(true);

        //Cameras
        ingameCamera.SetActive(true);
        tutorialCamera.SetActive(false);

    }

    public void HideCameraTutorialLayout() {
        cameraTutorialLayout.SetActive(false);
        npcLayout.SetActive(true);
        ingameLayout.SetActive(true);
    }

    private IEnumerator InitTutorialElements() {

        yield return new WaitForSeconds(0.5f);

        playerController.playerCanMoveTps = false;
        GameManagerScript.instance.playerIsTalking = true;
        pebblesTutorialCharacter.SetActive(true);

        //Layouts
        tutorialLayout.SetActive(true);
        npcLayout.SetActive(false);
        ingameLayout.SetActive(false);

        //Cameras
        ingameCamera.SetActive(false);
        tutorialCamera.SetActive(true);

        StartCoroutine(WaitForTutorialClickInstructions());  
    }

    private IEnumerator WaitForTutorialClickInstructions() {
        clickText.SetActive(false);
        yield return new WaitForSeconds(3.0f);
        clickText.SetActive(true);
        canClickScreen = true;
    }

    public void showCameraTutorialLayout() {
        cameraTutorialLayout.SetActive(true);
    }
}
