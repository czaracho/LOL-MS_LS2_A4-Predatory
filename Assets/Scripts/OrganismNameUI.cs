using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganismNameUI : MonoBehaviour
{
    public Camera polaroidCamera;
    public float positionOffset = 0;
    public GameObject animal;
    
    private void LateUpdate()
    {
        transform.position = polaroidCamera.WorldToScreenPoint(animal.transform.position + Vector3.up * positionOffset);    
    }
}
