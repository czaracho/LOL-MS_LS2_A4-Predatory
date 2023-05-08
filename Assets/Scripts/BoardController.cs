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
    public TextTrigger nextLevelBoardConversation;
    public TextTrigger helpConversation;
    public TextTrigger incorrectBoardConversation;

    private const float DISTANCE_FROM_SLOT = 0.3f;

    public BoardReviewObject[] boardsForReview;

    public Transform checkedPhotosLocation;

    private bool isShowingPhoto = false;
    private bool playerIsOnBoard = false;

    private void Awake()
    {
        EventManager.instance.GenericAction += ShowNextBoard;
        EventManager.instance.ShowIncorrectBoardDialogue += IncorrectBoardConversation;
        EventManager.instance.MakePlayerOnBoardStatus += PlayerIsOnTheBoard;
    }

    private void OnDestroy()
    {
        EventManager.instance.GenericAction -= ShowNextBoard;
        EventManager.instance.ShowIncorrectBoardDialogue -= IncorrectBoardConversation;
        EventManager.instance.MakePlayerOnBoardStatus -= PlayerIsOnTheBoard;
    }

    private void Start(){         
        InitializeBoard();
    }

    private void Update()
    {
        if (GameManagerScript.instance.playerIsTalking)
            return;

        if (playerIsOnBoard) {
            CheckForPhotos();
        }
        
        if (Input.GetMouseButtonDown(0)) {

            if (!canTouchPhotos) {
                return;
            }

            //if (GameManagerScript.instance.playerIsTalking)
            //    return;

            RaycastHit hit;
            Ray ray = boardCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == GameStringConstants.photo)
                {
                    if (currentSelectedPhoto != null) {
                        currentSelectedPhoto.ResetAnimation();
                        currentSelectedPhoto = null;
                    }

                    currentSelectedPhoto = hit.transform.gameObject.GetComponent<Photo>();

                    if (!currentSelectedPhoto.isOnBoardSlot)
                        currentSelectedPhoto.AnimatePhoto();

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

    private void CheckForPhotos() {        

        RaycastHit hit;
        Ray ray = boardCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit)) {
            if (hit.transform.gameObject.tag == GameStringConstants.photo) {

                if (isShowingPhoto)
                    return;

                Photo photo = hit.transform.gameObject.GetComponent<Photo>();

                if (photo.isOnBoardSlot)
                    return;

                isShowingPhoto = true;

                if (!photo.isOnBoardSlot) 
                    EventManager.instance.OnShowPhotoPreview(photo);
            }
            else {
                isShowingPhoto = false;
                EventManager.instance.OnHidePhotoPreview();
            }
        }
    }

    private void MovePhotoToSlot(RaycastHit newHit) {

        currentSelectedPhoto.ResetAnimation();

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

    private void MovePhotoToStartingPosition(Photo photo) {
        photo.transform.DOMove(photo.startingPosition, 0.5f).OnComplete(() => { photo.isOnBoardSlot = false; });
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

        }

    }

    private void HideCheckedPhotos() {

        foreach (var photo in boardPhotos) {

            if (photo.isOnBoardSlot) {
                photo.transform.position = checkedPhotosLocation.position;
                photo.gameObject.SetActive(false);
            }

        }
    }


    public void ReviewBoards() {

        Debug.Log("Entramos al review Boards");

        if (GameManagerScript.instance.playerIsTalking) {
            return;
        }

        foreach (var board in boardsForReview) {

            if (board.checkedForObjectivesReview)
                continue;

            board.ReviewBoard();
            
            return;

        }

    }

    public void ShowNextBoard() {
        
        Debug.Log("We show the next board. First review successful");
        HideCheckedPhotos();

        foreach (var board in boardsForReview) {

            if (board.checkedForObjectivesReview) {
                board.gameObject.SetActive(false);
            }
            else {
                board.gameObject.SetActive(true);
                return;
            }                

        }

    }

    public void ResetBoard() {
        
        foreach (var photo in boardPhotos) {

            if (photo.isOnBoardSlot) {
                MovePhotoToStartingPosition(photo);
            }

        }
    }

    public void HelpConversation() {
        helpConversation.TriggerTextAction();
    }

    public void IncorrectBoardConversation() { 
        incorrectBoardConversation.TriggerTextAction();
    }

    public void PlayerIsOnTheBoard() {
        playerIsOnBoard = true;
    }

}
