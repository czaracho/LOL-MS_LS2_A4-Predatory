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
        typeGeneric
    }

    public enum AnimalType { 
        predator,
        prey,
        plant,
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



    private void Start()
    {
        _lang = SharedState.LanguageDefs;
        string animalName = _lang[nameId];

        organismNameUI = Instantiate(organismNameUIPrefab, animalNameCanvas.transform).GetComponent<OrganismNameUI>();
        organismNameUI.InitilizeAnimalName(animalName, polaroidCamera, this.transform.gameObject);

        if (anim != null) {
            StartCoroutine(WaitForAnimationStart());
        }
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

