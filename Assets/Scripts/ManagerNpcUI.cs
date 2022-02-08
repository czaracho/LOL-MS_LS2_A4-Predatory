using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerNpcUI : MonoBehaviour
{
    public GameObject player;
    public Image prefabUI;
    private Image uiUse;
    public GameObject npcCanvas;
    public Camera tpCamera;
    public float positionOffsetY = 0;
    public float positionOffsetX = 0;
    private bool playerOnRange = false;
    public DialogTrigger dialogTrigger;
    private bool playerHasStartedDialog = false;

    private void Start()
    {
        uiUse = Instantiate(prefabUI, npcCanvas.transform).GetComponent<Image>();
    }

    private void Update()
    {
        if (playerOnRange) {

            if (Input.GetKeyDown(KeyCode.E) && !playerHasStartedDialog) {
                playerHasStartedDialog = true;
                dialogTrigger.TriggerDialogue();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        playerOnRange = true;
        IngameUIController.instance.ShowNotificationText();
    }

    private void OnTriggerExit(Collider other)
    {
        playerOnRange = false;
        playerHasStartedDialog = false;
        IngameUIController.instance.HideNotificationText();
    }

    private void LateUpdate()
    {
        uiUse.transform.position = tpCamera.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y, transform.position.z) + new Vector3(positionOffsetX, positionOffsetY, 0));
        float dist = 1 / Vector3.Distance(transform.position, player.transform.position) * 6f;
        dist = Mathf.Clamp(dist, 0.5f, 0.75f);
        uiUse.transform.localScale = new Vector3(dist, dist,0);
    }


}
