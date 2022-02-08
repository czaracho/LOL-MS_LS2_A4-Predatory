using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Album : MonoBehaviour
{
    public GameObject albumUI;
    public GameObject photoGridParent;
    [HideInInspector]
    public Image[] photoAlbumImage = new Image[15];
    public Button showAlbumButton;
    public GameObject albumViewerCanvas;
    public GameObject photoAlbumViewer;
    private Photo emptyPhoto;
    private int selectedPhotoIndex = 0;

    private void Start()
    {
        EventManager.instance.TakePicture += FillPhotoAlbum;

        int i = 0;
        foreach (Transform child in photoGridParent.transform) {
            photoAlbumImage[i] = child.GetChild(0).gameObject.GetComponent<Image>();
            i++;
        }

        emptyPhoto = new Photo();
        emptyPhoto.animalName = Organism.Type.typeGeneric;
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

        for (int i = 0; i < Loader.photoCollection.Length; i++)
        {

            if (Loader.photoCollection[i].photoIsSaved == false)
            {
                Loader.photoCollection[i].animalName = SnapshotController.newPhoto.animalName;
                Loader.photoCollection[i].picture = SnapshotController.newPhoto.picture;
                Loader.photoCollection[i].photoIsSaved = true;
                Loader.photoCollection[i].indexPhoto = i;

                Sprite photoSprite = Sprite.Create(Loader.photoCollection[i].picture, new Rect(0.0f, 0.0f, Loader.photoCollection[i].picture.width, Loader.photoCollection[i].picture.height), new Vector2(0.5f, 0.5f), 100.0f);
                photoAlbumImage[i].sprite = photoSprite;

                Debug.Log("animalName: " + Loader.photoCollection[i].animalName.ToString());
                Debug.Log("Guardamos la foto en la posición: " + i);
                Debug.Log("Loader.photoCollection[i].photoIsSaved " + Loader.photoCollection[i].photoIsSaved);


                break;
            }
        }

        Debug.Log("Al final queda como: ");
        for (int i = 0; i < Loader.photoCollection.Length; i++) {
            Debug.Log("Loader.photoCollection[" + i + "]:" + Loader.photoCollection[i].animalName);
        }

    }

    public void ShowAlbum() {

        if (!IngameUIController.instance.albumIsOpen)
        {
            IngameUIController.instance.albumIsOpen = true;
            albumUI.gameObject.SetActive(true);
            showAlbumButton.transform.GetChild(0).GetComponent<Text>().text = "Close album";
        }
        else {
            IngameUIController.instance.albumIsOpen = false;
            albumUI.gameObject.SetActive(false);
            showAlbumButton.transform.GetChild(0).GetComponent<Text>().text = "Check album";
        }
    }

    public void CheckSelectedPhoto(int indexOfPhoto) {

        if (Loader.photoCollection[indexOfPhoto].photoIsSaved)
        {
            selectedPhotoIndex = indexOfPhoto;
            albumViewerCanvas.SetActive(true);
            Sprite photoSprite = Sprite.Create(Loader.photoCollection[indexOfPhoto].picture, new Rect(0.0f, 0.0f, Loader.photoCollection[indexOfPhoto].picture.width, Loader.photoCollection[indexOfPhoto].picture.height), new Vector2(0.5f, 0.5f), 100.0f);
            photoAlbumViewer.gameObject.GetComponent<Image>().sprite = photoAlbumImage[indexOfPhoto].sprite;
        }
    }

    public void KeepPhoto() {
        albumViewerCanvas.SetActive(false);
    }

    public void DeletePhoto()
    {
        SnapshotController.photosTakenQuantity--;
        Debug.Log("Borramos la foto de la posición: " + selectedPhotoIndex);
        Loader.photoCollection[selectedPhotoIndex] = emptyPhoto;
        photoAlbumImage[selectedPhotoIndex].sprite = null;
        albumViewerCanvas.SetActive(false);
    }
}
