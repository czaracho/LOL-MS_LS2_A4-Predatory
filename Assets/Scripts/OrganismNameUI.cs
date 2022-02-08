using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganismNameUI : MonoBehaviour
{
    public Camera polaroidCamera;
    public float positionOffsetY = 0;
    public float positionOffsetX = 0;

    public GameObject animal;
    
    private void LateUpdate()
    {
        transform.position = polaroidCamera.WorldToScreenPoint(new Vector3(animal.transform.position.x, animal.transform.position.y, animal.transform.position.z) + new Vector3(positionOffsetX, positionOffsetY, 0));    
    }
}
