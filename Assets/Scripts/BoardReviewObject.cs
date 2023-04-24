using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardReviewObject : MonoBehaviour
{
    public bool checkedForObjectivesReview = false;
    public bool checkObjectivesByType = true;
    public TextTrigger successMessage;
    public Photo[] photoSlotsForReview;

    public void ReviewBoard() {

        foreach (var photoSlotForReview in photoSlotsForReview) {

            Debug.Log("ReviewBoard");
            Debug.Log("photoSlotsForReview[] gameobject name: " + photoSlotForReview.name);
            Debug.Log("The value on the board by type original value: " + photoSlotForReview.photoAnimalType);
            Debug.Log("The animaltype value assigned by the photo: " + photoSlotForReview.photoSlotForReview.animalType);
            Debug.Log("The animal name value assigned by the photo: " + photoSlotForReview.photoSlotForReview.animalName);
            Debug.Log("The current Id is: " + photoSlotForReview.id);

            bool isMatch = checkObjectivesByType ? photoSlotForReview.photoAnimalType == photoSlotForReview.photoSlotForReview.animalType : 
                photoSlotForReview.photoAnimalName == photoSlotForReview.photoSlotForReview.animalName;

            if (isMatch) {
                //Do something
                Debug.Log("Is match ok");
            }
            else {
                //Do something
                Debug.Log("Not matched");
                //return;
            }

        }

        Debug.Log("Board successful.");
        checkedForObjectivesReview = true;
        successMessage.TriggerTextAction();
    }

    //public void ChangePhotoSlotValues(Photo photoSlot, OrganismObject.AnimalName newAnimalName, OrganismObject.AnimalType animalType) { 
    
    //}
}
