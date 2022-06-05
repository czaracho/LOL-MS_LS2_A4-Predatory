using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DottedLine : MonoBehaviour
{
    private Vector3 mousePos;
    private bool isDrawingLine;
    
    public Camera boardCamera;

    public LineRenderer line;
    public Material material;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isDrawingLine){
            
            CreateLine();

            mousePos = boardCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = -45;
            line.SetPosition(0, mousePos);
            line.SetPosition(1, mousePos);
        }
        else if (Input.GetMouseButtonDown(0) && isDrawingLine){
            line.transform.gameObject.SetActive(false);
            isDrawingLine = false;

        }

        if (isDrawingLine == true){
            mousePos = boardCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = -45;
            line.SetPosition(1, mousePos);
        }
    }

    private void CreateLine()
    {
        line.transform.gameObject.SetActive(true);
        line.gameObject.SetActive(true);
        line.material = material;
        line.positionCount = 2;
        line.startWidth = 1f;
        line.endWidth = 1f;
        line.useWorldSpace = true;
        line.numCapVertices = 50;
        isDrawingLine = true;
    }
}
