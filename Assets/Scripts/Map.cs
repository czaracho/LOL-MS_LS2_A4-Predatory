using Supercyan.AnimalPeopleSample;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using static OrganismObject;

public class Map : MonoBehaviour
{
    public GameObject mapCanvas;
    public GameObject ingameCanvas;
    public GameObject objectivesCanvas;
    public GameObject npcCanvas;
    public GameObject mapIcons;
    public TextMeshProUGUI animalName;
    public GameObject animalNameBG;
    public SimpleSampleCharacterControl playerController;
    public CamerasController camerasController;
    public Camera mapCamera;
    public GameObject mainCamera;
    public MapPlayerIcon mapPlayerIcon;
    
    public void Update() {

        if (Input.GetKeyDown(KeyCode.M) && camerasController.isThirdPerson && !camerasController.mapIsActive) {           
            OpenMap();
        }

        RaycastHit hit;
        Ray ray = mapCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit)) {

            if (hit.transform.tag == "Icon") {
                MinimapIcon animalIcon = hit.transform.gameObject.GetComponent<MinimapIcon>();
                animalNameBG.SetActive(true);
                animalName.text = animalIcon.animalName;
            }
            else {
                animalNameBG.SetActive(false);
                animalName.text = "";
            }

        }

    }

    public void CloseMap() {
        objectivesCanvas.SetActive(true);
        npcCanvas.SetActive(true);
        mainCamera.SetActive(true);
        mapCamera.gameObject.SetActive(false);
        mapCanvas.SetActive(false);
        mapIcons.SetActive(false);
        ingameCanvas.SetActive(true);
        playerController.playerCanMoveTps = true;
        camerasController.mapIsActive = false;
    }

    public void OpenMap() {
        objectivesCanvas.SetActive(false);
        npcCanvas.SetActive(false);
        mainCamera.SetActive(false);
        mapCamera.gameObject.SetActive(true);
        mapCanvas.SetActive(true);
        mapIcons.SetActive(true);
        ingameCanvas.SetActive(false);
        playerController.playerCanMoveTps = false;
        camerasController.mapIsActive = true;
    }


}
