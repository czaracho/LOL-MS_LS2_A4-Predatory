using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Photo : MonoBehaviour
{
    public int id;              //id of the photo
    public OrganismObject.AnimalName photoAnimalName;   //lion, etc
    public OrganismObject.AnimalType photoAnimalType;   //predator etc
    public OrganismObject.AnimalName photoAnimalNameAdditional;
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
    public bool slotIsOccupied = false;
    
    public OrganismIdentifier photoSlotForReview; //This is for the review

    private void Start() {
        photoSlotForReview = new OrganismIdentifier();
    }


    private void OnTriggerEnter(Collider other) {

        if (!gameObject.CompareTag(GameStringConstants.photo)) return;

        if (other.gameObject.CompareTag(GameStringConstants.photoSlot)) {

            Debug.Log("Entramos al slot capo: " + photoAnimalName);

            Photo photoSlot = other.GetComponent<Photo>();

            if (!photoSlot.slotIsOccupied) {
                isOnBoardSlot = true;
                checkedForReview = true;
                photoSlot.id = id;
                photoSlot.photoSlotForReview.animalName = photoAnimalName;
                photoSlot.photoSlotForReview.animalType = photoAnimalType;
                photoSlot.slotIsOccupied = true;

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!gameObject.CompareTag(GameStringConstants.photoSlot)) return;

        if (other.gameObject.CompareTag(GameStringConstants.photo))
        {
            Photo photo = other.GetComponent<Photo>();

            if (photo.id != id) {
                Debug.Log("Los ID no son iguales");
                return;
            }
            else {
                Debug.Log("Los ID SI son iguales");
            }
                

            if (photo.slotIsOccupied) {
                isOnBoardSlot = false;
                checkedForReview = false;
                photo.photoSlotForReview.animalName = OrganismObject.AnimalName.typeGeneric;
                photo.photoSlotForReview.animalType = OrganismObject.AnimalType.typeGeneric;
                photo.slotIsOccupied = false;
                id = 0;
            }

        }
    }

}
