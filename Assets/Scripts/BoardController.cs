using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoardController : MonoBehaviour
{
    public GameManagerScript gameManager;
    public bool photoIsSelected = false;    
    public GameObject boardPhotosMain;   

    public Photo[] boardPhotos = new Photo[Constants.TOTAL_PHOTO_SLOTS];
    private Photo currentSelectedPhoto = null;
    private Photo currentSelectedSlot = null;


    public Camera boardCamera;
    public TextTrigger initialBoardConversation;

    private const float DISTANCE_FROM_SLOT = 0.3f;

    private void Awake()
    {
        EventManager.instance.StartBoardInitialConversation += StartBoardInitialConversation;
    }

    private void OnDestroy()
    {
        EventManager.instance.StartBoardInitialConversation -= StartBoardInitialConversation;
    }

    private void Start(){         
        InitializeBoard();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) {

            RaycastHit hit;
            Ray ray = boardCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "Photo")
                {
                    currentSelectedPhoto = hit.transform.gameObject.GetComponent<Photo>();

                    if (currentSelectedPhoto.isOnBoardSlot)
                    {
                        MovePhotoToStartingPosition();
                    }
                }
                else if (hit.transform.gameObject.tag == "PhotoSlot") 
                {
                    if (currentSelectedPhoto != null) {
                        MovePhotoToSlot(hit);
                    }
                }
            }
        }
    }

    private void MovePhotoToSlot(RaycastHit newHit) {

        currentSelectedSlot = newHit.transform.gameObject.GetComponent<Photo>();

        Vector3 newPhotoPos = new Vector3(currentSelectedSlot.transform.position.x,
                                          currentSelectedSlot.transform.position.y,
                                          currentSelectedSlot.transform.position.z - DISTANCE_FROM_SLOT);

        currentSelectedPhoto.transform.DOMove(newPhotoPos, 0.5f);

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
            boardPhotos[i].animalName = Loader.photoCollection[i].animalName;
            boardPhotos[i].animalType = Loader.photoCollection[i].animalType;
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

        //GameManagerScript.instance.StartBoardConversation();
        //EventManager.instance.OnGoToBoard();
    }

    public void StartBoardInitialConversation() {
        GameManagerScript.instance.playerIsTalking = true;
        initialBoardConversation.TriggerStartingDialog();
    }

}
