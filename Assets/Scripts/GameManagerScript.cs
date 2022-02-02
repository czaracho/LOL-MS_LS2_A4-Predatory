using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    //vamos a poner todo lo relacionado a los objetivos
    public Organism[] organismsForObjectives;


    private void Start()
    {
        Loader.photoCollection = new Photo[15];

        for (int i = 0; i < Loader.photoCollection.Length; i++)
        {
            Loader.photoCollection[i] = new Photo();
        }
    }

    public void checkObjectives() { 
            
    }
}
