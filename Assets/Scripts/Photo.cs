using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Photo : MonoBehaviour
{
    public int id;              //id of the photo
    public Organism.AnimalName animalName;   //lion, etc
    public Organism.AnimalType animalType;   //predator etc
    [HideInInspector]
    public string infoId;       //id of the info
    public Texture2D picture;   //picture of the animal
    public bool photoIsSaved = false;
    public int indexPhoto;
    [HideInInspector]
    public bool checkedForReview = false;

}
