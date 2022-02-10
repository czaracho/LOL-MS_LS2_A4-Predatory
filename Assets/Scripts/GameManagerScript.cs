using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    //vamos a poner todo lo relacionado a los objetivos
    public Organism[] organismsForObjectives;
    int objectivesAccomplishedCounter = 0;
    public bool objectivesAccomplished = false;
    [HideInInspector]
    public bool playerIsTalking = false;

    private void Start()
    {
        Loader.photoCollection = new Photo[15];

        for (int i = 0; i < Loader.photoCollection.Length; i++)
        {
            Loader.photoCollection[i] = new Photo();
            Loader.photoCollection[i].photoIsSaved = false;
            Loader.photoCollection[i].animalName = Organism.AnimalName.typeGeneric;
        }
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

                        if (objectivesAccomplishedCounter == organismsForObjectives.Length)
                        {
                            objectivesAccomplished = true;
                            ObjectivesAcomplished();
                            break;
                        }
                    }
                }
            }

            //Check what objectives are missing


        }
    }

    public void ObjectivesAcomplished() {
        Debug.Log("Todos los objetivos cumplidos");
    }
}
