using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Organism : MonoBehaviour
{
    public enum Type { 
        type1,
        type2,
        type3,
        type4
    }

    public enum SubType
    {
        subtype1,
        subtype2,
        subtype3,
        subtype4
    }

    public Type organismType;
    public SubType organismSubtype;
    public GameObject organismNameUI;

    public string infoId;   //example: info_tiger

}
