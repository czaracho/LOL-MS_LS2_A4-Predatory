using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Photo : MonoBehaviour
{
    public int id;              //id of the photo
    public Organism.Type animalType;         //predator etc
    public string animalName;         //animal name
    [HideInInspector]
    public string infoId;       //id of the info
    public Texture2D picture;   //picture of the animal
    [HideInInspector]
    public bool photoIsSaved = false;
    public int indexPhoto;
}
