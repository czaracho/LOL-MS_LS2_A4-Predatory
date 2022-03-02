using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Organism : MonoBehaviour
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
        plant
    }
    
    public AnimalName animalName;
    public AnimalType animalType;
    public OrganismNameUI organismNameUI;
    public string nameId;
    public string infoId;
    [HideInInspector]
    public bool checkedObjective;
    public Animator anim;


    private void Start()
    {
        organismNameUI.animal = this.transform.gameObject;
        organismNameUI.organism = this;

        if (anim != null) {
            StartCoroutine(WaitForAnimationStart());
        }
    }

    IEnumerator WaitForAnimationStart() {
        
        yield return new WaitForSeconds(Random.RandomRange(1.0f, 3.0f));
        anim.speed = Random.Range(0.8f, 1.0f);
        anim.SetBool("canMove", true);
    }
}
