using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardReviewObject : MonoBehaviour
{
    public bool checkedForObjectivesReview = false;
    public bool checkObjectivesByType = true;
    public bool checkByRelationShip = false;
    public TextTrigger successMessage;
    public Photo[] photoSlotsForReview;
    private bool isMatch = false;

    public void ReviewBoard() {

        int objectivesCounter = 0;

        //if (checkByRelationShip) {

        //    foreach (var photoSlotForReview in photoSlotsForReview) {

        //        isMatch = photoSlotForReview.photoAnimalName == photoSlotForReview.photoSlotForReview.animalName;

        //        if (isMatch) {
        //            objectivesCounter++;
        //        }

        //    }
        //}


        foreach (var photoSlotForReview in photoSlotsForReview) {

            isMatch = checkObjectivesByType ? photoSlotForReview.photoAnimalType == photoSlotForReview.photoSlotForReview.animalType : 
                (photoSlotForReview.photoAnimalName == photoSlotForReview.photoSlotForReview.animalName || 
                photoSlotForReview.photoAnimalNameAdditional == photoSlotForReview.photoSlotForReview.animalName);

            if (isMatch) {
                //Do something
                Debug.Log("Is match ok");
            }
            else {
                //Do something
                Debug.Log("Not matched");
                return;
            }

        }

        Debug.Log("Board successful.");
        checkedForObjectivesReview = true;
        EventManager.instance.OnResetNPCFirstDialogue();
        successMessage.TriggerTextAction();
    }

}
