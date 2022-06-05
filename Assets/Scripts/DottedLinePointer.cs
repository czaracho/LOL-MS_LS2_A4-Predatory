using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DottedLinePointer : MonoBehaviour
{
    public int dotAmount;
    
    private float m_dotGap;
    private bool isDrawingLine = false;
    private const float Z_POS_DOT = -43.9f;
    private Vector3 mousePos;
    private Vector3 clickedPos;

    public GameObject dotPrefab;
    GameObject[] m_dotArray;
    
    public Camera boardCamera;    


    private void Start()
    {
        m_dotGap = 1f / dotAmount;   //percentage
        SpawnDots();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isDrawingLine){
            isDrawingLine = true;
            SetDotsActive(true);
            clickedPos = boardCamera.ScreenToWorldPoint(Input.mousePosition);
            SetDotPos(clickedPos, clickedPos);
        }
        else if (Input.GetMouseButtonDown(0) && isDrawingLine){
            SetDotsActive(false);
            isDrawingLine = false;

        }

        if (isDrawingLine == true){
            mousePos = boardCamera.ScreenToWorldPoint(Input.mousePosition);
            SetDotPos(clickedPos, mousePos);
        }
    }

    void SpawnDots() {
        
        m_dotArray = new GameObject[dotAmount];

        for (int i = 0; i < dotAmount; i++) {
            GameObject _dot = Instantiate(dotPrefab);
            _dot.SetActive(false);
            m_dotArray[i] = _dot;
        }
    }

    void SetDotPos(Vector3 startPos, Vector3 endPos) {
        
        for (int i = 0; i < dotAmount; i++) {
            Vector3 _targetPos = Vector2.Lerp(startPos, endPos, i * m_dotGap);
            _targetPos.z = Z_POS_DOT;
            m_dotArray[i].transform.position = _targetPos;
        }
    }

    void SetDotsActive(bool isActive) {
        for (int i = 0; i < dotAmount; i++)
        {
            m_dotArray[i].SetActive(isActive);
        }
    }

}
