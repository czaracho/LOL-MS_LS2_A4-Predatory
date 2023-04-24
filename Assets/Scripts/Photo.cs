using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Photo : MonoBehaviour
{
    public int id;              //id of the photo
    public OrganismObject.AnimalName photoAnimalName;   //lion, etc
    public OrganismObject.AnimalType photoAnimalType;   //predator etc
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


    private void OnTriggerEnter(Collider other)
    {
        if (!gameObject.CompareTag(GameStringConstants.photo)) return;

        if (other.gameObject.CompareTag(GameStringConstants.photoSlot))
        {
            Photo photoSlot = other.GetComponent<Photo>();            

            if (!photoSlot.slotIsOccupied) {
                isOnBoardSlot = true;

                Debug.Log("Antes de asignar");
                Debug.Log("Intentamos asignar photoAnimalName: " + photoAnimalName);
                Debug.Log("Intentamos asignar photoAnimalType: " + photoAnimalType);

                id = photoSlot.id;
                photoSlot.photoSlotForReview.animalName = photoAnimalName;
                photoSlot.photoSlotForReview.animalType = photoAnimalType;
                photoSlot.slotIsOccupied = true;

                Debug.Log("Despues de asignar");
                Debug.Log("photoSlot.photoSlotForReview.animalName: " + photoSlot.photoSlotForReview.animalName);
                Debug.Log("photoSlot.photoSlotForReview.animalType: " + photoSlot.photoSlotForReview.animalType);
                Debug.Log("the id of the other photoslot: " + photoSlot.id);
                Debug.Log("Gameobject name: " + photoSlot.name);

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!gameObject.CompareTag(GameStringConstants.photo)) return;

        if (other.gameObject.CompareTag(GameStringConstants.photoSlot))
        { 
            Photo photoSlot = other.GetComponent<Photo>();

            if (photoSlot.id != id) {
                Debug.Log("Los ID no son iguales");
                return;
            }
            else {
                Debug.Log("Los ID SI son iguales");
            }
                

            if (photoSlot.slotIsOccupied) {
                isOnBoardSlot = false;

                photoSlot.photoSlotForReview.animalName = OrganismObject.AnimalName.typeGeneric;
                photoSlot.photoSlotForReview.animalType = OrganismObject.AnimalType.typeGeneric;
                photoSlot.slotIsOccupied = false;
                id = 0;
            }

        }
    }

}

[System.Serializable]
public class PhotoGroup {
    public List<Photo> photos;
}
