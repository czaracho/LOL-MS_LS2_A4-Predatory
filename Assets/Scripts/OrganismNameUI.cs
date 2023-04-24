using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class OrganismNameUI : MonoBehaviour
{
    private Camera polaroidCamera;
    public float positionOffsetY = 0;
    public float positionOffsetX = 0;
    private TextMeshProUGUI animalNameTxt;
    public GameObject animal;
    [HideInInspector]
    public OrganismObject organism;



    public void InitilizeAnimalName(string animalName, Camera m_polaroidCamera, GameObject thisAnimal) {
        polaroidCamera = m_polaroidCamera;
        animalNameTxt = this.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        animalNameTxt.text = animalName;
        animal = thisAnimal;
        this.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (animal != null) {
            transform.position = polaroidCamera.WorldToScreenPoint(new Vector3(animal.transform.position.x, animal.transform.position.y, animal.transform.position.z) + new Vector3(positionOffsetX, positionOffsetY, 0));
        }
    }
}
