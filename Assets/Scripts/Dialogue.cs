﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{    
    public dialog[] sentences;
}

[System.Serializable]
public class dialog {
    public enum NameOfCat
    {
        Lana,
        Pebbles,
        None
    }

    public NameOfCat nameOfCat; 
    public string sentenceId;

}
