using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{

    public Photo boardPhoto;
    public Photo boardPhoto2;


    private void Start()
    {
        Material newMaterial = new Material(Shader.Find("Unlit/Texture"));


        boardPhoto.animalName = Loader.photoCollection[0].animalName;
        boardPhoto.animalType = Loader.photoCollection[0].animalType;
        boardPhoto.picture = Loader.photoCollection[0].picture;
        boardPhoto.infoId = Loader.photoCollection[0].infoId;
        boardPhoto.photoIsSaved = true;
        boardPhoto.indexPhoto = 0;

        boardPhoto.transform.GetChild(0).GetComponent<Renderer>().material = newMaterial;
        boardPhoto.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = boardPhoto.picture;

        boardPhoto2.animalName = Loader.photoCollection[1].animalName;
        boardPhoto2.animalType = Loader.photoCollection[1].animalType;
        boardPhoto2.picture = Loader.photoCollection[1].picture;
        boardPhoto2.infoId = Loader.photoCollection[1].infoId;
        boardPhoto2.photoIsSaved = true;
        boardPhoto2.indexPhoto = 0;

        boardPhoto2.transform.GetChild(0).GetComponent<Renderer>().material = newMaterial;
        boardPhoto2.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = Loader.photoCollection[1].picture;
    }
}
