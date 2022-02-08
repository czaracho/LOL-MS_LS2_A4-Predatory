using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    //vamos a poner todo lo relacionado a los objetivos
    public Organism[] organismsForObjectives;
    int objectivesAccomplishedCounter = 0;
    public bool objectivesAccomplished = false;

    private void Start()
    {
        Loader.photoCollection = new Photo[15];

        for (int i = 0; i < Loader.photoCollection.Length; i++)
        {
            Loader.photoCollection[i] = new Photo();
            Loader.photoCollection[i].photoIsSaved = false;
            Loader.photoCollection[i].animalName = Organism.Type.typeGeneric;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)) {
            checkObjectives();
        }

        if (Input.GetKeyDown(KeyCode.C)) {
            Debug.Log("-Loader.photoCollection.lenght: " + Loader.photoCollection.Length);
            for (int i = 0; i < Loader.photoCollection.Length; i++)
            {
                Debug.Log("-Loader.photoCollection[i].animalName: " + Loader.photoCollection[i].animalName);
            }
        }


    }

    public void checkObjectives() {

        objectivesAccomplishedCounter = 0;

        if (!objectivesAccomplished) {
            for (int i = 0; i < organismsForObjectives.Length; i++)
            {
                //Debug.Log("organismsForObjectives for i: " + i);
                for (int j = 0; j < Loader.photoCollection.Length; j++)
                {
                    //Debug.Log("Loader.photoCollection for j: " + j);

                    if (organismsForObjectives[i].organismName == Loader.photoCollection[j].animalName)
                    {
                        //Debug.Log("*******************************");
                        //Debug.Log("organismsForObjectives[i].organismName: " + organismsForObjectives[i].organismName.ToString() + " - Loader.photoCollection[j].animalName: " + Loader.photoCollection[j].animalName.ToString());
                        //Debug.Log("Epa, encontramos una igualdad");

                        objectivesAccomplishedCounter++;
                        //Debug.Log("objectivesAccomplishedCounter: " + objectivesAccomplishedCounter);
                        //Debug.Log("organismsForObjectiveslenght: " + organismsForObjectives.Length);


                        if (objectivesAccomplishedCounter == organismsForObjectives.Length)
                        {
                            Debug.Log("contadores igualados");
                            objectivesAccomplished = true;
                            ObjectivesAcomplished();
                            break;
                        }
                    }
                }
            }
        }


    }

    public void ObjectivesAcomplished() {
        Debug.Log("Todos los objetivos cumplidos");

    }
}
