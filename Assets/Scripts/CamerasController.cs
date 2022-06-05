using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Delegates;

//Refactor this class
public class CamerasController : MonoBehaviour
{
    public bool isThirdPerson = true;
    
    public GameObject thirdPersonCharacter;
    public GameObject thirdPersonCamera;
    public GameObject boardCamera;
    public GameObject firstPersonCharacter;

    public GameObject fpsCanvas;
    public GameObject[] thirdPersonCanvases;
    public GameObject polaroidCameraUI;
    public GameObject boardCanvas;

    public GameManagerScript gameManager;
    public FirstPersonController fpsController;

    private void Start()
    {
        EventManager.instance.ShowIngameUI += ShowIngameTpsCanvas;
        EventManager.instance.ShowFpsUI += ShowFPSCanvas;
        EventManager.instance.GoToBoard += SwitchToBoardCamera;
    }

    private void Update(){
        if (Input.GetKeyDown(KeyCode.Space)) {

            if (fpsController.playerCanMove && !IngameUIController.instance.albumIsOpen) {
                SwitchCamera();
            }
        }
    }

    public void SwitchCamera() {

        if (!gameManager.playerIsTalking) {
            
            if (isThirdPerson)
            {
                //Go to first person
                firstPersonCharacter.transform.position = new Vector3(thirdPersonCharacter.transform.position.x, thirdPersonCharacter.transform.position.y + 0.5f, thirdPersonCharacter.transform.position.z);
                firstPersonCharacter.transform.rotation = thirdPersonCharacter.transform.rotation;

                polaroidCameraUI.SetActive(true);

                for (int i = 0; i < thirdPersonCanvases.Length; i++)
                {
                    thirdPersonCanvases[i].SetActive(false);
                }

                thirdPersonCharacter.SetActive(false);
                thirdPersonCamera.SetActive(false);
                firstPersonCharacter.SetActive(true);

                isThirdPerson = false;

                EventManager.instance.OhShowFpsUI(true);
                EventManager.instance.OnShowIngameUI(false);

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                //Got to third person
                polaroidCameraUI.SetActive(false);

                thirdPersonCharacter.SetActive(true);
                thirdPersonCamera.SetActive(true);
                firstPersonCharacter.SetActive(false);
                isThirdPerson = true;

                thirdPersonCharacter.transform.position = new Vector3(firstPersonCharacter.transform.position.x, thirdPersonCharacter.transform.position.y, firstPersonCharacter.transform.position.z);
                thirdPersonCharacter.transform.rotation = firstPersonCharacter.transform.rotation;

                EventManager.instance.OhShowFpsUI(false);
                EventManager.instance.OnShowIngameUI(true);

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

            }

            //Debug.Log("La posición del fps es de: " + firstPersonCharacter.transform.position);
            //Debug.Log("La posición del TPS es de: " + thirdPersonCharacter.transform.position);
        }
    }

    public void ShowIngameTpsCanvas(bool show) {

        if (show)
        {
            for (int i = 0; i < thirdPersonCanvases.Length; i++)
            {
                thirdPersonCanvases[i].SetActive(true);
            }
        }
        else {
            for (int i = 0; i < thirdPersonCanvases.Length; i++)
            {
                thirdPersonCanvases[i].SetActive(false);
            }
        }
    }

    public void ShowFPSCanvas(bool show) {

        if (show) {
            fpsCanvas.SetActive(true);
        }
        else
            fpsCanvas.SetActive(false);
    }

    public void SwitchToBoardCamera() {
        NewActionVoidDelegate newDelegate = ChangeCameras;
        GameManagerScript.instance.SetBlackCatSilhouetteForeground(newDelegate);
        GameManagerScript.instance.playerIsTalking = false;
        //boardCanvas.SetActive(true);
    }

    public void ChangeCameras() {
        NewActionVoidDelegate newDelegate = ChangeCameras;
        thirdPersonCharacter.SetActive(false);
        thirdPersonCamera.SetActive(false);
        boardCamera.SetActive(true);
        GameManagerScript.instance.RemoveBlackCatSilhouetteForeground(()=> { boardCanvas.SetActive(true); });
    }

    

}
