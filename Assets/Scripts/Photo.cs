using DG.Tweening;
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
    public int indexPhoto = 999;
    [HideInInspector]
    public bool checkedForReview = false;
    [HideInInspector]
    public Vector3 startingPosition;
    public bool isOnBoardSlot = false;
    public bool slotIsOccupied = false;
   
    public OrganismIdentifier photoSlotForReview; //This is for the review

    private void Start() {
        photoSlotForReview = new OrganismIdentifier();
    }


    private void OnTriggerEnter(Collider other) {

        //If this is a photoslot, then it does nothing (YOU'LL DO NUTHING)
        if (!gameObject.CompareTag(GameStringConstants.photo)) return;


        if (other.gameObject.CompareTag(GameStringConstants.photoSlot)) {

            Photo photoSlot = other.GetComponent<Photo>();

            if (BoardController.instance.currentSelectedSlot != photoSlot) {
                return;
            }

            if (!photoSlot.slotIsOccupied) {
                isOnBoardSlot = true;
                //checkedForReview = true;
                photoSlot.id = id;
                photoSlot.photoSlotForReview.animalName = photoAnimalName;
                photoSlot.photoSlotForReview.animalType = photoAnimalType;
                photoSlot.slotIsOccupied = true;

            }
        }
    }

    private void OnTriggerExit(Collider other) {

        if (!gameObject.CompareTag(GameStringConstants.photoSlot)) return;

        if (other.gameObject.CompareTag(GameStringConstants.photo)) {
            Photo photo = other.GetComponent<Photo>();

            if (photo.id != id) {
                Debug.Log("Los ID no son iguales");
                return;
            }
            else {
                Debug.Log("Los ID SI son iguales");
            }


            if (slotIsOccupied) {
                photo.isOnBoardSlot = false;
                //checkedForReview = false;
                photoSlotForReview.animalName = OrganismObject.AnimalName.typeGeneric;
                photoSlotForReview.animalType = OrganismObject.AnimalType.typeGeneric;
                slotIsOccupied = false;
                id = 0;
            }

        }
    }

    public void AnimatePhoto() {
        gameObject.transform.DOScale(new Vector3(1.01f, 1.01f, 1), 0.35f).SetLoops(-1, LoopType.Yoyo).SetId("PhotoAnimation");
    }

    public void ResetAnimation() {
        Debug.Log("Reset animation test");
        DOTween.Kill("PhotoAnimation", true);
        gameObject.transform.DOScale(new Vector3(0.75f, 0.75f, 1), 0.35f);
    }

}
