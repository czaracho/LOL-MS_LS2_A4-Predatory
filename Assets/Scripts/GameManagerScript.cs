using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoLSDK;
using SimpleJSON;

public class GameManagerScript : MonoBehaviour
{
    //vamos a poner todo lo relacionado a los objetivos
    public Organism[] organismsForObjectives;
    int objectivesAccomplishedCounter = 0;
    [HideInInspector]
    public bool objectivesAccomplished = false;
    [HideInInspector]
    public bool playerIsTalking = false;

    private void Start()
    {
        JSONNode _lang;
        Loader.photoCollection = new Photo[15];

        for (int i = 0; i < Loader.photoCollection.Length; i++)
        {
            Loader.photoCollection[i] = new Photo();
            Loader.photoCollection[i].photoIsSaved = false;
            Loader.photoCollection[i].animalName = Organism.AnimalName.typeGeneric;
        }

        //dialog dialog = sentences.Dequeue();
        //string dialogeLine = _lang[dialog.sentenceId];
        //currentTalkingCat = dialog.GetCharacterName();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)) {
            checkObjectives();
        }
    }

    public void checkObjectives() {

        objectivesAccomplishedCounter = 0;

        if (!objectivesAccomplished) {
            for (int i = 0; i < organismsForObjectives.Length; i++)
            {
                for (int j = 0; j < Loader.photoCollection.Length; j++)
                {

                    if (organismsForObjectives[i].animalName == Loader.photoCollection[j].animalName)
                    {
                        objectivesAccomplishedCounter++;

                        organismsForObjectives[i].checkedObjective = true;

                        if (objectivesAccomplishedCounter == organismsForObjectives.Length)
                        {
                            objectivesAccomplished = true;
                            ObjectivesAccomplished();
                            break;
                        }
                    }
                }
            }

            int uncheckedObjectiveCounter = 0;

            for (int i = 0; i < organismsForObjectives.Length; i++) {
                if (!organismsForObjectives[i].checkedObjective) {
                    uncheckedObjectiveCounter++;
                }
            }            
        }
    }

    public void ObjectivesAccomplished() {
        Debug.Log("Todos los objetivos cumplidos");
    }

    public void ObjectivesUnaccomplished(int quantity) {
        Debug.Log("Objectives not accomplished");


    }
}
