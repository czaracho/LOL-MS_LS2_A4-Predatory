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
    [HideInInspector]
    public Photo[] photos = new Photo[15];
    public Button showAlbumButton;
    public GameObject albumViewerCanvas;
    public GameObject photoAlbumViewer;

    private void Start()
    {
        EventManager.instance.TakePicture += FillPhotoAlbum;
        int i = 0;

        foreach (Transform child in photoGridParent.transform) {
            photoAlbumImage[i] = child.GetChild(0).gameObject.GetComponent<Image>();
            i++;
        }
    }

    private void OnDestroy()
    {
        EventManager.instance.TakePicture -= FillPhotoAlbum;
    }

    public void FillPhotoAlbum() {

        photos = SnapshotController.photoList.ToArray();

        for (int i = 0; i < photos.Length; i++) {
            Sprite photoSprite = Sprite.Create(photos[i].picture, new Rect(0.0f, 0.0f, photos[i].picture.width, photos[i].picture.height), new Vector2(0.5f, 0.5f), 100.0f);
            photoAlbumImage[i].sprite = photoSprite;
        }
    }

    public void ShowAlbum() {
        Debug.Log("Entramos al show album");
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

    public void CheckSelectedPhoto() {
        albumViewerCanvas.SetActive(true);
        Sprite photoSprite = Sprite.Create(photos[0].picture, new Rect(0.0f, 0.0f, photos[0].picture.width, photos[0].picture.height), new Vector2(0.5f, 0.5f), 100.0f);
        photoAlbumViewer.GetComponent<Image>().sprite =  photoAlbumImage[0].sprite = photoSprite;
    }

    public void KeepPhoto() {
        albumViewerCanvas.SetActive(false);
    }

    public void DeletePhoto()
    {
        albumViewerCanvas.SetActive(false);
    }
}
