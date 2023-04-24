using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Runtime.CompilerServices;

public class BoardController : MonoBehaviour
{
    public GameManagerScript gameManager;
    public bool canTouchPhotos = true;    
    public GameObject boardPhotosMain;   

    public Photo[] boardPhotos = new Photo[Constants.TOTAL_PHOTO_SLOTS];
    private Photo currentSelectedPhoto = null;
    private Photo currentSelectedSlot = null;

    public Camera boardCamera;
    public TextTrigger initialBoardConversation;

    public OrganismGroup[] objectives;

    private List<OrganismIdentifier> currentPhotosOrganismsOnBoard = new List<OrganismIdentifier>();

    private const float DISTANCE_FROM_SLOT = 0.3f;

    private void Awake()
    {
        EventManager.instance.StartBoardInitialConversation += StartBoardInitialConversation;
        EventManager.instance.AddOrganismToBoardToReview += addPhotoOrganismOnBoard;
        EventManager.instance.RemoveOrganismFromBoardToReview += removePhotoOrganismOnBoard;
    }

    private void OnDestroy()
    {
        EventManager.instance.StartBoardInitialConversation -= StartBoardInitialConversation;
        EventManager.instance.AddOrganismToBoardToReview -= addPhotoOrganismOnBoard;
        EventManager.instance.RemoveOrganismFromBoardToReview -= removePhotoOrganismOnBoard;
    }

    private void Start(){         
        InitializeBoard();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) {

            if (!canTouchPhotos) {
                return;
            }

            RaycastHit hit;
            Ray ray = boardCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == GameStringConstants.photo)
                {
                  currentSelectedPhoto = hit.transform.gameObject.GetComponent<Photo>();

                    Debug.Log("Current selected photo photoOrganism animal name: " + currentSelectedPhoto.photoOrganism.animalName);

                    if (currentSelectedPhoto.isOnBoardSlot)
                    {
                        MovePhotoToStartingPosition();
                    }
                }
                else if (hit.transform.gameObject.tag == GameStringConstants.photoSlot) 
                {
                    if (currentSelectedPhoto != null) {
                        MovePhotoToSlot(hit);
                    }
                }
            }
        }
    }

    private void MovePhotoToSlot(RaycastHit newHit) {

        canTouchPhotos = false;

        currentSelectedSlot = newHit.transform.gameObject.GetComponent<Photo>();

        Vector3 newPhotoPos = new Vector3(currentSelectedSlot.transform.position.x,
                                          currentSelectedSlot.transform.position.y,
                                          currentSelectedSlot.transform.position.z - DISTANCE_FROM_SLOT);

        currentSelectedPhoto.transform.DOMove(newPhotoPos, 0.5f).OnComplete(() => { 
            canTouchPhotos = true; 
            currentSelectedPhoto = null; 
        });

    }

    private void MovePhotoToStartingPosition() {
        currentSelectedPhoto.transform.DOMove(currentSelectedPhoto.startingPosition, 0.5f).OnComplete(() => { currentSelectedPhoto.isOnBoardSlot = false; });
    }

    void InitializeBoard() {

        gameManager.RemoveBlackCatSilhouetteForeground(null);

        for (int i = 0; i < Constants.TOTAL_PHOTO_SLOTS; i++)
        {

            boardPhotos[i] = boardPhotosMain.transform.GetChild(i).GetComponent<Photo>();
            boardPhotos[i].id = i;          
            boardPhotos[i].photoAnimalName = Loader.photoCollection[i].photoAnimalName;
            boardPhotos[i].photoAnimalType = Loader.photoCollection[i].photoAnimalType;
            boardPhotos[i].picture = Loader.photoCollection[i].picture;
            boardPhotos[i].startingPosition = boardPhotos[i].transform.position;

            Material newMaterial = new Material(Shader.Find("Unlit/Texture"));

            boardPhotos[i].transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = newMaterial;
            boardPhotos[i].transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.mainTexture = boardPhotos[i].picture;

            if (boardPhotos[i].picture != null)
            {
                boardPhotosMain.transform.GetChild(i).gameObject.SetActive(true);
            }

            boardPhotos[i].initPhotoOrganism();
        }

    }

    public void StartBoardInitialConversation() {
        GameManagerScript.instance.playerIsTalking = true;
        initialBoardConversation.TriggerStartingDialog();
    }

    public void addPhotoOrganismOnBoard(OrganismIdentifier organism) {         
        
        currentPhotosOrganismsOnBoard.Add(organism);

        Debug.Log("Added organism: " + organism.animalName);
        Debug.Log("Organisms: " + currentPhotosOrganismsOnBoard.Count);
    }

    public void removePhotoOrganismOnBoard(OrganismIdentifier organism) { 
        currentPhotosOrganismsOnBoard.Remove(organism);
    }

    public void CheckBoardObjectivesOld() {

        int correctPhotos = 0;

        for (int i = 0; i < objectives.Length; i++) {

            if (!objectives[i].checkedForReview) {
                
                for (int x = 0; x < objectives[i].organisms.Count; x++) {
                    //Check by the animal type
                    if (objectives[i].checkForType) {

                        for (int z = 0; z < currentPhotosOrganismsOnBoard.Count; z++) {

                            if (!currentPhotosOrganismsOnBoard[z].isChecked) {
                                if (objectives[i].organisms[x].animalType == currentPhotosOrganismsOnBoard[z].animalType) {
                                    currentPhotosOrganismsOnBoard[z].isChecked = true;
                                    correctPhotos++;
                                }
                            }

                        }

                    } //Check by the animal name
                    else if (!objectives[i].checkForType) {
                        
                        for (int z = 0; z < currentPhotosOrganismsOnBoard.Count; z++) {

                            if (!currentPhotosOrganismsOnBoard[z].isChecked) {
                                if (objectives[i].organisms[x].animalName == currentPhotosOrganismsOnBoard[z].animalName) {
                                    currentPhotosOrganismsOnBoard[z].isChecked = true;
                                    correctPhotos++;
                                }
                            }

                        }

                    }
                } 
            }

            if (correctPhotos == objectives.Length) {
                //Do something and go to the next group
                return;
            }
            else {
                //Do Something to tell the player that the objectives were not fulfilled
                return;
            }

        }
    }

    public void CheckBoardObjectives() {
        
        int correctPhotos = 0;

        foreach (var objective in objectives) {
            if (objective.checkedForReview)
                continue;

            foreach (var organism in objective.organisms) {
                bool isObjectiveMet = CheckObjective(organism, objective.checkForType);
                if (isObjectiveMet)
                    correctPhotos++;
            }

            if (correctPhotos == objectives.Length) {
                // Do something and go to the next group
                Debug.Log("Objectives fulfilled");
                return;
            }
            else {
                // Do something to tell the player that the objectives were not fulfilled
                Debug.Log("Objectives NOT fulfilled");

                return;
            }
        }
    }

    private bool CheckObjective(OrganismIdentifier targetOrganism, bool checkForType) {
        foreach (var photoOrganism in currentPhotosOrganismsOnBoard) {
            if (photoOrganism.isChecked)
                continue;

            bool isMatch = checkForType ? targetOrganism.animalType == photoOrganism.animalType : targetOrganism.animalName == photoOrganism.animalName;

            if (isMatch) {
                photoOrganism.isChecked = true;
                return true;
            }
        }

        return false;
    }

}
