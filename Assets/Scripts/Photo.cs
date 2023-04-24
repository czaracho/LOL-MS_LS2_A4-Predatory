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
    [HideInInspector]
    public bool slotIsOccupied = false;

    public OrganismIdentifier photoOrganism;

    private void OnTriggerEnter(Collider other)
    {
        if (!gameObject.CompareTag(GameStringConstants.photo)) return;

        if (other.gameObject.CompareTag(GameStringConstants.photoSlot))
        {
            Photo photoSlot = other.GetComponent<Photo>();            

            if (!photoSlot.slotIsOccupied) {
                EventManager.instance.OnAddOrganismToBoardToReview(photoOrganism);
                isOnBoardSlot = true;
                photoSlot.slotIsOccupied = true;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!gameObject.CompareTag(GameStringConstants.photo)) return;

        if (other.gameObject.CompareTag(GameStringConstants.photoSlot))
        {
            Photo photoSlot = other.GetComponent<Photo>();

            if (photoSlot.slotIsOccupied) {
                EventManager.instance.OnRemoveOrganismFromBoardToReview(photoOrganism);
                isOnBoardSlot = true;
                photoSlot.slotIsOccupied = false;
            }

        }
    }

    public void initPhotoOrganism() {

        Debug.Log("Before");
        Debug.Log("photoAnimalName: " + photoAnimalName);
        Debug.Log("photoAnimalNamee: " + photoAnimalName);

        photoOrganism = new OrganismIdentifier();
        photoOrganism.animalName = photoAnimalName;
        photoOrganism.animalType = photoAnimalType;

        Debug.Log("After");
        Debug.Log("InitPhotoOrganism animalName: " + photoOrganism.animalName);
        Debug.Log("InitPhotoOrganism animalType: " + photoOrganism.animalType);

    }

}

[System.Serializable]
public class PhotoGroup {
    public List<Photo> photos;
}
