using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoardController : MonoBehaviour
{
    public bool photoIsSelected = false;    
    public GameObject boardPhotosMain;   

    public Photo[] boardPhotos = new Photo[Constants.TOTAL_PHOTO_SLOTS];
    public Photo currentSelectedPhoto = null;
    public Photo currentSelectedSlot = null;

    public Camera boardCamera;
    public TextTrigger initialBoardConversation;

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
                if (hit.transform.gameObject.GetComponent<Photo>())
                {
                    Debug.Log("testeamos el hit transform");
                    //Transform objectHit = hit.transform;
                }
            }
        }
    }

    void InitializeBoard() {

        for (int i = 0; i < Constants.TOTAL_PHOTO_SLOTS; i++)
        {

            boardPhotos[i] = boardPhotosMain.transform.GetChild(i).GetComponent<Photo>();
            boardPhotos[i].id = i;
            boardPhotos[i].animalName = Loader.photoCollection[i].animalName;
            boardPhotos[i].animalType = Loader.photoCollection[i].animalType;
            boardPhotos[i].picture = Loader.photoCollection[i].picture;

            Material newMaterial = new Material(Shader.Find("Unlit/Texture"));

            boardPhotos[i].transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = newMaterial;
            boardPhotos[i].transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.mainTexture = boardPhotos[i].picture;

            if (boardPhotos[i].picture != null){
                boardPhotosMain.transform.GetChild(i).gameObject.SetActive(true);
            }
        }

        GameManagerScript.instance.StartBoardConversation();
        //EventManager.instance.OnGoToBoard();
    }

    public void StartBoardInitialConversation() {
        GameManagerScript.instance.playerIsTalking = true;
        initialBoardConversation.TriggerStartingDialog();
    }

    public void MovePhotoToSlot() {
        
        RaycastHit hit;
        Ray ray = boardCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.GetComponent<Photo>()) {
                
                //Transform objectHit = hit.transform;
            }
        }
    }

}
