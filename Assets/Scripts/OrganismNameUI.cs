using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoLSDK;
using SimpleJSON;
using UnityEngine.UI;

public class OrganismNameUI : MonoBehaviour
{
    public Camera polaroidCamera;
    public float positionOffsetY = 0;
    public float positionOffsetX = 0;
    private Text animalNameTxt;
    [HideInInspector]
    public GameObject animal;
    [HideInInspector]
    public Organism organism;

    JSONNode _lang;


    private void Start()
    {
        animalNameTxt = this.transform.GetChild(1).GetComponent<Text>();
        _lang = SharedState.LanguageDefs;
        string animalName = _lang[organism.nameId];
        animalNameTxt.text = animalName;
    }

    private void LateUpdate()
    {
        if (animal != null) {
            transform.position = polaroidCamera.WorldToScreenPoint(new Vector3(animal.transform.position.x, animal.transform.position.y, animal.transform.position.z) + new Vector3(positionOffsetX, positionOffsetY, 0));
        }
    }
}
