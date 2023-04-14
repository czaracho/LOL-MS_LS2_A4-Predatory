using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerNpcUI : MonoBehaviour
{
    public GameObject player;
    public GameObject npcCanvas;
    public Image exclamationSignPrefab;
    public Image interactionSignPrefab;
    private Image exclamationSign;
    private Image interactionBgSign;
    
    public Camera tpCamera;
    
    public float exclamationPositionOffsetY = 0;
    public float exclamationPositionOffsetX = 0;
    public float interactionSignPositionOffsetX = 0;
    public float interactionSignPositionOffsetY = 0;
    private int currentConversationId = 0;
    private bool playerOnRange = false;
    //public bool reviewedObjectives = false;
    
    public TextTrigger[] dialogTrigger;

    private void Start()
    {
        exclamationSign = Instantiate(exclamationSignPrefab, npcCanvas.transform).GetComponent<Image>();
        interactionBgSign = Instantiate(interactionSignPrefab, npcCanvas.transform).GetComponent<Image>();
        interactionBgSign.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (playerOnRange) {

            interactionBgSign.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E) && !GameManagerScript.instance.playerIsTalking)
            {
                GameManagerScript.instance.playerIsTalking = true;
                EventManager.instance.OnShowPromptActionUI(false);

                if (dialogTrigger != null) {
                    
                    dialogTrigger[currentConversationId].TriggerTextAction();

                    if (currentConversationId < dialogTrigger.Length - 1)
                    {
                        currentConversationId++;
                    }
                }
            }
        }
        else{
            if (interactionBgSign.gameObject.active == true)
                interactionBgSign.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") {
            Debug.Log("Player is on range");
            playerOnRange = true;
            EventManager.instance.OnShowPromptActionUI(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") {
            playerOnRange = false;
            GameManagerScript.instance.playerIsTalking = false;
            EventManager.instance.OnShowPromptActionUI(false);
        }
    }

    private void LateUpdate()
    {
        exclamationSign.transform.position = tpCamera.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y, transform.position.z) + new Vector3(exclamationPositionOffsetX, exclamationPositionOffsetY, 0));
        interactionBgSign.transform.position = tpCamera.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y, transform.position.z) + new Vector3(interactionSignPositionOffsetX, interactionSignPositionOffsetY, 0));
        float dist = 1 / Vector3.Distance(transform.position, player.transform.position) * 6f;
        dist = Mathf.Clamp(dist, 0.5f, 0.75f);
        exclamationSign.transform.localScale = new Vector3(dist, dist,0);
        interactionBgSign.transform.localScale = new Vector3(dist, dist, 0);
    }
}
