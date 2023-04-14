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
    [HideInInspector]
    public bool photoIsSaved = false;
    [HideInInspector]
    public int indexPhoto;
    [HideInInspector]
    public bool checkedForReview = false;
    [HideInInspector]
    public Vector3 startingPosition;
    [HideInInspector]
    public bool isOnBoardSlot = false;
    [HideInInspector]
    public bool slotIsOccupied = false;


    private void OnTriggerEnter(Collider other)
    {
        if (!gameObject.CompareTag("Photo")) return;

        if (other.gameObject.CompareTag("PhotoSlot") && !other.gameObject.GetComponent<Photo>().slotIsOccupied)
        {
            isOnBoardSlot = true;
            other.gameObject.GetComponent<Photo>().slotIsOccupied = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!gameObject.CompareTag("Photo")) return;

        if (other.gameObject.CompareTag("PhotoSlot") && other.gameObject.GetComponent<Photo>().slotIsOccupied)
        {
            isOnBoardSlot = false;
            other.gameObject.GetComponent<Photo>().slotIsOccupied = false;
        }
    }

}
