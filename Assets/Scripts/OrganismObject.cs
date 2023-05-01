using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoLSDK;
using SimpleJSON;
using static OrganismObject;

public class OrganismObject : MonoBehaviour
{
    public enum AnimalName { 
        lion,
        antelope,
        hyena,
        elephant,
        deer,
        ant,
        rhino,
        pecker,
        frog,
        typeGeneric,
        laptop
    }

    public enum AnimalType { 
        predator,
        prey,
        plant,
        parasite,
        typeGeneric
    }
    
    public AnimalName animalName;
    public AnimalType animalType;
    public OrganismNameUI organismNameUIPrefab;
    [HideInInspector]
    public OrganismNameUI organismNameUI;
    public string nameId;
    public string infoId;
    [HideInInspector]
    public bool checkedObjective;
    public Animator anim;
    public GameObject animalNameCanvas;
    public Camera polaroidCamera;
    JSONNode _lang;
    public bool isForMainMenu = false;



    private void Start()
    {
        if (anim != null) {
            StartCoroutine(WaitForAnimationStart());
        }

        if (isForMainMenu) {
            return;
        }

        _lang = SharedState.LanguageDefs;
        string animalName = _lang[nameId];

        organismNameUI = Instantiate(organismNameUIPrefab, animalNameCanvas.transform).GetComponent<OrganismNameUI>();
        organismNameUI.InitilizeAnimalName(animalName, polaroidCamera, this.transform.gameObject);
    }

    IEnumerator WaitForAnimationStart() {
        
        yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));
        anim.speed = Random.Range(0.8f, 1.0f);
        anim.SetBool("canMove", true);
    }
}

[System.Serializable]
public class OrganismIdentifier{
    public AnimalName animalName;
    public AnimalType animalType;
    public bool checkByName = false;
    [SerializeField]
    public bool isChecked;
}

[System.Serializable]
public class OrganismGroup {

    [SerializeField] 
    private string label;
    public bool checkForType = true;
    public bool checkedForReview = false;
    public List<OrganismIdentifier> organisms;
}

