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

    private const float DISTANCE_FROM_SLOT = 0.3f;

    public BoardReviewObject[] boardsForReview;

    private void Awake()
    {
        //EventManager.instance.StartBoardInitialConversation += StartBoardInitialConversation;
        EventManager.instance.GenericAction += ShowNextBoard;
    }

    private void OnDestroy()
    {
        //EventManager.instance.StartBoardInitialConversation -= StartBoardInitialConversation;
        EventManager.instance.GenericAction -= ShowNextBoard;
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

        }

    }


    public void ReviewBoards() {

        Debug.Log("Entramos al review Boards");

        foreach (var board in boardsForReview) {

            if (board.checkedForObjectivesReview)
                continue;

            board.ReviewBoard();
            
            return;

        }

    }

    public void ShowNextBoard() {
        Debug.Log("We show the next board. First review successful");
    }


}
