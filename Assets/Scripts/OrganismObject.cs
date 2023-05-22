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
        buffalo,
        fox,
        rabbit,
        earthworm,
        flower,
        squirrel,
        bee,
        deadtree,
        fungi,
        boltflies,
        acorn,
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

    public float positionOffsetY = 1.5f;
    public float positionOffsetX = 0f;



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

        AdjustLabelOffset();
    }

    private void AdjustLabelOffset() { 
        organismNameUI.positionOffsetX = positionOffsetX;
        organismNameUI.positionOffsetY = positionOffsetY;
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
    [SerializeField]
    public int organismId = 0;
}

[System.Serializable]
public class OrganismGroup {

    [SerializeField] 
    private string label;
    public bool checkForType = true;
    public bool checkedForReview = false;
    public List<OrganismIdentifier> organisms;
}

