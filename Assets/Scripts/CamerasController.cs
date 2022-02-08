using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Refactor this class
public class CamerasController : MonoBehaviour
{
    public GameObject thirdPersonCharacter;
    public GameObject thirdPersonCamera;
    public GameObject firstPersonCharacter;
    public GameObject polaroidCameraUI;

    public GameObject[] thirdPersonCanvases;
    public GameObject fpsCanvas;
    public bool isThirdPerson = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            
            if (!IngameUIController.instance.albumIsOpen) {
                SwitchCamera();
            }
        }
    }

    public void SwitchCamera() {

        if (isThirdPerson)
        {
            //Go to first person
            polaroidCameraUI.SetActive(true);

            firstPersonCharacter.transform.position = thirdPersonCharacter.transform.position;
            firstPersonCharacter.transform.rotation = thirdPersonCharacter.transform.rotation;

            thirdPersonCharacter.SetActive(false);
            firstPersonCharacter.SetActive(true);
            thirdPersonCamera.SetActive(false);
            isThirdPerson = false;

            for (int i = 0; i < thirdPersonCanvases.Length; i++) {
                thirdPersonCanvases[i].SetActive(false);
            }

            fpsCanvas.SetActive(true);

            EventManager.instance.OnShowIngameUI(false);

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else {
            //Got to third person
            polaroidCameraUI.SetActive(false);
            thirdPersonCharacter.transform.position = firstPersonCharacter.transform.position;
            thirdPersonCharacter.transform.rotation = firstPersonCharacter.transform.rotation;

            thirdPersonCharacter.SetActive(true);
            thirdPersonCamera.SetActive(true);
            firstPersonCharacter.SetActive(false);
            isThirdPerson = true;
            EventManager.instance.OnShowIngameUI(true);

            for (int i = 0; i < thirdPersonCanvases.Length; i++)
            {
                thirdPersonCanvases[i].SetActive(true);
            }

            fpsCanvas.SetActive(false);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }
    }
}
