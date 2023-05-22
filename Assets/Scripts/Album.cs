using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LoLSDK;

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
    [SerializeField] 
    private TextMeshProUGUI animalName;

    public TextManager textManager;

    private void OnDestroy() {
        EventManager.instance.TakePicture -= FillPhotoAlbum;
        EventManager.instance.ShowPhotoPreview -= ShowPhotoPreview;
        EventManager.instance.HidePhotoPreview -= HidePhotoPreview;
    }


    private void Start()
    {
        EventManager.instance.TakePicture += FillPhotoAlbum;
        EventManager.instance.ShowPhotoPreview += ShowPhotoPreview;
        EventManager.instance.HidePhotoPreview += HidePhotoPreview;

        int i = 0;
        foreach (Transform child in photoGridParent.transform) {
            photoGridParentChilds[i] = child.gameObject;
            photoAlbumImage[i] = child.GetChild(0).gameObject.GetComponent<Image>();
            photoAlbumObjects[i] = child.GetChild(0).gameObject;
            photoAlbumObjects[i].SetActive(false);
            i++;
        }

        emptyPhoto = new Photo();
        emptyPhoto.photoAnimalName = OrganismObject.AnimalName.typeGeneric;
        emptyPhoto.checkedForReview = false;
        emptyPhoto.indexPhoto = 0;
        emptyPhoto.picture = null;
        emptyPhoto.photoIsSaved = false;
    }

    public void FillPhotoAlbum() {

        Debug.Log("Loader.Photocollection: " + Loader.photoCollection.Length);

        for (int i = 0; i < Loader.photoCollection.Length; i++)
        {
            if (!Loader.photoCollection[i].photoIsSaved)
            {
                Loader.photoCollection[i].photoAnimalName = SnapshotController.newPhoto.photoAnimalName;
                Loader.photoCollection[i].photoAnimalType = SnapshotController.newPhoto.photoAnimalType;
                Loader.photoCollection[i].picture = SnapshotController.newPhoto.picture;
                Loader.photoCollection[i].infoId = SnapshotController.newPhoto.infoId;
                Loader.photoCollection[i].photoIsSaved = true;
                Loader.photoCollection[i].indexPhoto = i;
                SnapshotController.newPhoto.indexPhoto = i;
                SnapshotController.newPhoto.checkedForReview = false;

                Sprite photoSprite = Sprite.Create(Loader.photoCollection[i].picture, new Rect(0.0f, 0.0f, Loader.photoCollection[i].picture.width, Loader.photoCollection[i].picture.height), new Vector2(0.5f, 0.5f), 100.0f);
                photoGridParentChilds[i].transform.localScale = new Vector3(0.8f, 0.8f, 1);
                photoAlbumObjects[i].SetActive(true);
                photoAlbumImage[i].sprite = photoSprite;
             
                break;
            }
        }

        //Update objectives
        foreach (var objective in GameManagerScript.instance.objectives) {

            Debug.Log("El estado checked de la foto es: " + SnapshotController.newPhoto.checkedForReview);

            if (SnapshotController.newPhoto.checkedForReview)
                return;

            if (!objective.objectivesAccomplished) {

                bool objectiveIsAccomplished = objective.checkObjectives(SnapshotController.newPhoto);

                if (objectiveIsAccomplished) {
                    return;
                }
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
            photoAlbumViewer.gameObject.GetComponent<Image>().sprite = photoAlbumImage[indexOfPhoto].sprite;

            if (Loader.photoCollection[indexOfPhoto].photoAnimalName != OrganismObject.AnimalName.typeGeneric) {
                animalName.text = Loader.photoCollection[indexOfPhoto].photoAnimalName.ToString();
            }            

            if (Loader.photoCollection[indexOfPhoto].photoAnimalType!= OrganismObject.AnimalType.typeGeneric) {
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
        Debug.Log("Borramos foto con index: " + selectedPhotoIndex);
        foreach (var objective in GameManagerScript.instance.objectives) {
            objective.checkForDeletedPhoto(selectedPhotoIndex);
        }

        SnapshotController.photosTakenQuantity--;
        Loader.photoCollection[selectedPhotoIndex] = new Photo();
        Loader.photoCollection[selectedPhotoIndex].photoAnimalName = OrganismObject.AnimalName.typeGeneric;
        Loader.photoCollection[selectedPhotoIndex].photoAnimalType = OrganismObject.AnimalType.typeGeneric;
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

        infoScreen.SetActive(true);
        string infoOfAnimal = "";

        infoOfAnimal = textManager.DisplayInfo(Loader.photoCollection[selectedPhotoIndex].infoId);
        LOLSDK.Instance.SpeakText(Loader.photoCollection[selectedPhotoIndex].infoId);


        if (infoOfAnimal != "") {
            infoText.text = infoOfAnimal;
        }
    }

    public void CloseInfo() {

        ((ILOLSDK_EXTENSION)LOLSDK.Instance.PostMessage).CancelSpeakText();

        infoScreen.SetActive(false);
    }

    public void ShowPhotoPreview(Photo photo ) {
        albumViewerCanvas.SetActive(true);
        Sprite photoSprite = Sprite.Create(photo.picture, new Rect(0.0f, 0.0f, photo.picture.width, photo.picture.height), new Vector2(0.5f, 0.5f), 100.0f);
        photoAlbumViewer.gameObject.GetComponent<Image>().sprite = photoSprite;
        animalName.text = photo.photoAnimalName.ToString();

    }

    public void HidePhotoPreview() {
        albumViewerCanvas.SetActive(false);
    }
}
