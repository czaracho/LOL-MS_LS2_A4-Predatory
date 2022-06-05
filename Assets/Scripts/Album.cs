using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Album : MonoBehaviour
{
    public GameObject albumUI;
    public GameObject photoGridParent;
    [HideInInspector]
    public GameObject[] photoGridParentChilds = new GameObject[Constants.TOTAL_PHOTO_SLOTS];
    [HideInInspector]
    public Image[] photoAlbumImage = new Image[Constants.TOTAL_PHOTO_SLOTS];
    private GameObject[] photoAlbumObjects = new GameObject[Constants.TOTAL_PHOTO_SLOTS];
    public Button showAlbumButton;
    public GameObject albumViewerCanvas;
    public GameObject photoAlbumViewer;
    public GameObject infoScreen;
    public GameObject infoButton;
    public TextMeshProUGUI infoText;
    private Photo emptyPhoto;
    private int selectedPhotoIndex = 0;

    public TextManager textManager;

    private void Start()
    {
        EventManager.instance.TakePicture += FillPhotoAlbum;

        int i = 0;
        foreach (Transform child in photoGridParent.transform) {
            photoGridParentChilds[i] = child.gameObject;
            photoAlbumImage[i] = child.GetChild(0).gameObject.GetComponent<Image>();
            photoAlbumObjects[i] = child.GetChild(0).gameObject;
            photoAlbumObjects[i].SetActive(false);
            i++;
        }

        emptyPhoto = new Photo();
        emptyPhoto.animalName = Organism.AnimalName.typeGeneric;
        emptyPhoto.checkedForReview = false;
        emptyPhoto.indexPhoto = 0;
        emptyPhoto.picture = null;
        emptyPhoto.photoIsSaved = false;
    }

    private void OnDestroy()
    {
        EventManager.instance.TakePicture -= FillPhotoAlbum;
    }

    public void FillPhotoAlbum() {

        Debug.Log("Loader.Photocollection: " + Loader.photoCollection.Length);

        for (int i = 0; i < Loader.photoCollection.Length; i++)
        {
            if (!Loader.photoCollection[i].photoIsSaved)
            {
                Loader.photoCollection[i].animalName = SnapshotController.newPhoto.animalName;
                Loader.photoCollection[i].animalType = SnapshotController.newPhoto.animalType;
                Loader.photoCollection[i].picture = SnapshotController.newPhoto.picture;
                Loader.photoCollection[i].infoId = SnapshotController.newPhoto.infoId;
                Loader.photoCollection[i].photoIsSaved = true;
                Loader.photoCollection[i].indexPhoto = i;

                Sprite photoSprite = Sprite.Create(Loader.photoCollection[i].picture, new Rect(0.0f, 0.0f, Loader.photoCollection[i].picture.width, Loader.photoCollection[i].picture.height), new Vector2(0.5f, 0.5f), 100.0f);
                photoGridParentChilds[i].transform.localScale = new Vector3(0.8f, 0.8f, 1);
                photoAlbumObjects[i].SetActive(true);
                photoAlbumImage[i].sprite = photoSprite;
               
                break;
            }
        }
    }

    public void ShowAlbum() {

        if (!IngameUIController.instance.albumIsOpen)
        {
            IngameUIController.instance.albumIsOpen = true;
            IngameUIController.instance.ingameCanvas.SetActive(false);

            albumUI.gameObject.SetActive(true);

        }
        else {
            IngameUIController.instance.albumIsOpen = false;
            albumUI.gameObject.SetActive(false);
            IngameUIController.instance.ingameCanvas.SetActive(true);
        }
    }

    public void CheckSelectedPhoto(int indexOfPhoto) {

        if (Loader.photoCollection[indexOfPhoto].photoIsSaved)
        {
            selectedPhotoIndex = indexOfPhoto;
            albumViewerCanvas.SetActive(true);
            //Sprite photoSprite = Sprite.Create(Loader.photoCollection[indexOfPhoto].picture, new Rect(0.0f, 0.0f, Loader.photoCollection[indexOfPhoto].picture.width, Loader.photoCollection[indexOfPhoto].picture.height), new Vector2(0.5f, 0.5f), 100.0f);
            photoAlbumViewer.gameObject.GetComponent<Image>().sprite = photoAlbumImage[indexOfPhoto].sprite;

            Debug.Log("Current animal type: " + Loader.photoCollection[indexOfPhoto].animalType);

            if (Loader.photoCollection[indexOfPhoto].animalType!= Organism.AnimalType.typeGeneric) {
                infoButton.SetActive(true);
            }
            else
                infoButton.SetActive(false);
        }
    }

    public void KeepPhoto() {
        albumViewerCanvas.SetActive(false);
    }

    public void DeletePhoto()
    {
        SnapshotController.photosTakenQuantity--;
        Loader.photoCollection[selectedPhotoIndex] = new Photo();
        Loader.photoCollection[selectedPhotoIndex].animalName = Organism.AnimalName.typeGeneric;
        Loader.photoCollection[selectedPhotoIndex].checkedForReview = false;
        Loader.photoCollection[selectedPhotoIndex].indexPhoto = selectedPhotoIndex;
        Loader.photoCollection[selectedPhotoIndex].picture = null;
        Loader.photoCollection[selectedPhotoIndex].photoIsSaved = false;

        photoAlbumImage[selectedPhotoIndex].sprite = null;
        photoAlbumObjects[selectedPhotoIndex].SetActive(false);
        photoGridParentChilds[selectedPhotoIndex].transform.localScale = new Vector3(0.92f, 0.92f, 1);
        albumViewerCanvas.SetActive(false);
    }

    public void ShowInfo() {

        Debug.Log("Entramos al show info");
        Debug.Log("Selected photo index: " + selectedPhotoIndex.ToString());
        infoScreen.SetActive(true);
        string infoOfAnimal = "";

        Debug.Log("Info ID: " + Loader.photoCollection[selectedPhotoIndex].infoId);
        infoOfAnimal = textManager.DisplayInfo(Loader.photoCollection[selectedPhotoIndex].infoId);
        
        if (infoOfAnimal != "") {
            infoText.text = infoOfAnimal;
            Debug.Log(infoOfAnimal);
        }
    }

    public void CloseInfo() {
        infoScreen.SetActive(false);
    }
}
