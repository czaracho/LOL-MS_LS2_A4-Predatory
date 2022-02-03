﻿using System.Collections;
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
        rhino,
        pecker,
        typeGeneric
    }
    public Type organismName;
    public GameObject organismNameUI;
    public string infoId;   //example: info_tiger
}
