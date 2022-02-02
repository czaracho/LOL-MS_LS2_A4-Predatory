using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Organism : MonoBehaviour
{
    public enum Type { 
        lion,
        hyena,
        elephant,
        deer,
        ant,
        typeGeneric
    }

    public string organismName;
    public Type organismType;
    public GameObject organismNameUI;

    public string infoId;   //example: info_tiger
}
