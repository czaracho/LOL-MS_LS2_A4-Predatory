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
        }
    }

    public void checkObjectives() {

        if (!objectivesAccomplished) {
            for (int i = 0; i < organismsForObjectives.Length; i++)
            {
                for (int j = 0; i < Loader.photoCollection.Length; j++)
                {
                    if (organismsForObjectives[i].organismName == Loader.photoCollection[j].animalName)
                    {
                        objectivesAccomplishedCounter++;
                    }
                }
            }
        }

        if (objectivesAccomplishedCounter == organismsForObjectives.Length) {
            Debug.Log("Todos los objetivos cumplidos");
        }
    }
}
