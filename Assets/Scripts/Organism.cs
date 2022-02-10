using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Organism : MonoBehaviour
{
    public enum AnimalName { 
        lion,
        hyena,
        elephant,
        deer,
        ant,
        rhino,
        pecker,
        typeGeneric
    }

    public enum AnimalType { 
        predator,
        prey
    }
    
    public AnimalName animalName;
    public AnimalType animalType;
    public GameObject organismNameUI;
    public string infoId;  
}
