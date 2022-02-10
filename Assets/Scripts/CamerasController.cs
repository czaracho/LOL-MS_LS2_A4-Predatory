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

    public GameManagerScript gameManager;
    public FirstPersonController fpsController;

    private void Update()
    {
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
                polaroidCameraUI.SetActive(true);



                thirdPersonCharacter.SetActive(false);
                firstPersonCharacter.SetActive(true);
                thirdPersonCamera.SetActive(false);
                isThirdPerson = false;

                for (int i = 0; i < thirdPersonCanvases.Length; i++)
                {
                    thirdPersonCanvases[i].SetActive(false);
                }

                fpsCanvas.SetActive(true);

                EventManager.instance.OnShowIngameUI(false);

                firstPersonCharacter.transform.position = new Vector3(thirdPersonCharacter.transform.position.x, thirdPersonCharacter.transform.position.y + 0.5f, thirdPersonCharacter.transform.position.z) ;
                firstPersonCharacter.transform.rotation = thirdPersonCharacter.transform.rotation;

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
                EventManager.instance.OnShowIngameUI(true);

                thirdPersonCharacter.transform.position = new Vector3(firstPersonCharacter.transform.position.x, thirdPersonCharacter.transform.position.y, firstPersonCharacter.transform.position.z);
                thirdPersonCharacter.transform.rotation = firstPersonCharacter.transform.rotation;

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
}
