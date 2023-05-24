using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveField : MonoBehaviour
{
    [SerializeField] private string label;
    [HideInInspector]
    public int totalObjectivesAccomplished = 0;


    public OrganismIdentifier[] objectives;

    [SerializeField]
    public bool objectivesAccomplished = false;

    public GameObject checkedObjectiveImg;

    public bool checkObjectives(Photo photoForReview) {

        foreach (var objectiveOrganism in objectives) {

            if (objectiveOrganism.isChecked)
                continue;

            if (objectiveOrganism.checkByName) {
                //By Name
                if (photoForReview.photoAnimalName == objectiveOrganism.animalName) {
                    checkReviewedPhotoAndObjective(objectiveOrganism, photoForReview);
                    totalObjectivesAccomplished++;
                    break;
                }
            }
            else if(!objectiveOrganism.checkByName) {
                //By Type
                if (photoForReview.photoAnimalType == objectiveOrganism.animalType) {
                    checkReviewedPhotoAndObjective(objectiveOrganism, photoForReview);
                    totalObjectivesAccomplished++;
                    break;
                }
            }

        }

        if (totalObjectivesAccomplished == objectives.Length) {
            objectivesAccomplished = true;
            checkedObjectiveImg.SetActive(true);
        }

        return objectivesAccomplished;

    }

    private void checkReviewedPhotoAndObjective(OrganismIdentifier objectiveOrganism, Photo photoForReview) {
        objectiveOrganism.isChecked = true;
        objectiveOrganism.organismId = photoForReview.indexPhoto;
        photoForReview.checkedForReview = true;
    }

    public void checkForDeletedPhoto(int photoIndex) {

        foreach (var objectiveOrganism in objectives) {
            
            if (objectiveOrganism.organismId == photoIndex) {
                Debug.Log("Borramos al organismId: " + objectiveOrganism.organismId);
                totalObjectivesAccomplished = 0;
                objectiveOrganism.organismId = 0;
                objectivesAccomplished = false;
                objectiveOrganism.isChecked = false;
                checkedObjectiveImg.SetActive(false);
            }
        }

    }

}
