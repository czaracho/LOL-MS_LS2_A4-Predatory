using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Networking.Types;
using LoLSDK;
using SimpleJSON;

public class MinimapIcon : MonoBehaviour
{
    [HideInInspector]
    public string animalName;
    JSONNode _lang;
    public string nameId;

    public void Start() {
        _lang = SharedState.LanguageDefs;
        animalName = _lang[nameId];
        Debug.Log("This animal name is: " + animalName);
    }
}
